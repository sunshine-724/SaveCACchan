using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
//using System.Diagnostics;
//using System.Numerics;

public class enemycontrol : MonoBehaviour
{
    //探索系
    [SerializeField] float valid_range = 90;//探索状態に遷移する距離
    [SerializeField] float serch_range = 15;//発見することができる距離
    [SerializeField] float serch_speed = 3;//探索中の速度
    [SerializeField] float serch_limit = 5;
    [SerializeField] float serch_spintime = 3;//探索中何秒ごとに旋回するか
    private Vector2 RayDirection;
    private int state;//現在の状態(0:有効距離外により停止、1:探索状態、2:発見状態)
    private bool patrol_switch;//trueなら右、falseなら左
    private bool start_patrol;

    //速度系
    [SerializeField] float limited_speed = 20;//制限速度
    [SerializeField] float move_speed = 10;//移動速度
    [SerializeField] float jump_power = 30;//ジャンプの高さ
    private Vector3 beforePos;
    private float now_speed;
    private float xdiff;
    private bool isjump;

    //攻撃系
    public GameObject player_obj;//攻撃を行う対象
    [SerializeField] float atack_value = 10;//攻撃力
    [SerializeField] float atack_span = 1;//攻撃スパン(秒)
    private bool isattack;
    // Start is called before the first frame update
    void Start()
    {
        RayDirection = Vector2.right;
        patrol_switch = false;
        state = 0;
        beforePos = transform.position;
        xdiff = 0;

        isjump = true;
        isattack = true;
        start_patrol = true;
    }
    //state対応表
    //0:範囲外により停止
    //1:探索状態rayがプレイヤーを捉えたら2に遷移
    //2:追跡状態により一定範囲を越えるまで基本ずっと追いかける

    //Update関数はフレームごとに１回ずつ実行されます.
    void FixedUpdate()
    {

        if (player_obj == null) return;


        //有効範囲内に入ったら起動、待機状態を解除、探索状態に移行
        if (state == 0)
        {
            state = (Math.Pow(transform.position.x - player_obj.transform.position.x, 2) < Math.Pow(valid_range, 2)) ? 1 : 0;
            Debug.Log("state0");
            if (state == 1)
            {
                Debug.Log("state0end");
            }


        }

        //探索状態
        if (state == 1)
        {
            now_speed = (transform.position.x - beforePos.x) / Time.deltaTime;
            beforePos = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDirection, 15f, 64);
            //Raycastがなにかに当たったとき目的とするプレイヤーと同等かを検査,同等の場合追跡状態へ.
            if (patrol_switch && now_speed < serch_limit)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(move_speed, 0f));
                RayDirection = Vector2.right;
            }
            else if (now_speed > -serch_limit)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-move_speed, 0f));
                RayDirection = Vector2.left;
            }

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == player_obj)
                {
                    state = 2;
                    Debug.Log("state1end");
                }
            }
            if (start_patrol)
            {
                start_patrol = false;
                InvokeRepeating("patrol", 0f, 3f);
            }

        }

        //追跡状態
        if (state == 2)
        {
            now_speed = (transform.position.x - beforePos.x) / Time.deltaTime;
            beforePos = transform.position;
            xdiff = player_obj.transform.position.x - transform.position.x;
            if (xdiff > 0 && now_speed < limited_speed)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(move_speed, 0f));
                RayDirection = Vector2.right;
            }
            if (xdiff < 0 && now_speed > -limited_speed)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-move_speed, 0f));
                RayDirection = Vector2.left;
            }
            if (transform.position.y < player_obj.transform.position.y && isjump)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0f));
                isjump = false;
            }
            if (Vector3.Distance(transform.position, player_obj.transform.position) < 2)
            {
                if (isattack)
                {
                    isattack = false;
                    StartCoroutine(attackProcess(atack_span));
                }

            }


        }

        if (state == 1 || state == 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDirection, 5f, 64);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "Obstacles")
                {
                    Debug.Log("Dammd");
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 300f));
                }
            }

        }

    }

    void patrol()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (patrol_switch)
        {
            RayDirection = Vector2.right;//右に飛ばす(正)
            patrol_switch = false;
        }
        else
        {
            RayDirection = Vector2.left;//左に飛ばす(負)
            patrol_switch = true;
        }

        return;
    }
    private void OnCollisionEnter2D(Collision2D c)
    {
        Debug.Log("false");
        if (c.gameObject.CompareTag("Ground"))
        {
            isjump = true;
        }
    }
    private IEnumerator attackProcess(float delay1)
    {
        float temp = move_speed;
        move_speed = 0f;
        //攻撃処理
        Debug.Log("Start wait");

        yield return new WaitForSeconds(delay1);

        move_speed = temp;
        isattack = true;
        Debug.Log("Value reset");
    }


}
