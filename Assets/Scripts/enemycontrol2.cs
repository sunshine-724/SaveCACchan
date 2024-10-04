using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;

//Purpose: This object is chase to player like sharks

//if enter to this detect range, attached object will activated by change of state value(state = 1)
//state 0   this status is not actived attached object
//          state 0 is wait for enter player to detect area 
//state 1   this status is patorol mode
//          patrol mode is cycle thorough two relative coordinates
//state 2   this status is chase mode
//          chase mode is chase to player like sharks


public class enemycontrol2 : MonoBehaviour
{
    // 状態を管理する変数
    private int state = 0;

    // パトロール用座標（相対座標）
    public float patrol_speed = 10;
    public float patrol1_coordinatesX = 0;
    public float patrol1_coordinatesY = 0;
    private bool patrolcheck;//falseなら初期位置、trueなら相対座標から初期位置に移動してる最中
    private Vector2 patrol_point = new Vector2(0,0);

    // 検知範囲
    public float detect_range = 5.0f;

    // 追跡用の設定
    public float chase_speed = 5.0f;
    public float chase_span = 0.5f;
    public bool ischased;
    private bool chasemode;
    public GameObject playerobj;
    

    // 初期位置
    private Vector2 initialPosition;

    void Start()
    {
        patrolcheck = false;
        initialPosition = transform.position;
        patrol_point = new Vector2(initialPosition.x + patrol1_coordinatesX,initialPosition.y + patrol1_coordinatesY);
        

        //追跡
        ischased = false;
        chasemode = false;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case 0:
                // プレイヤーを検知する処理
                DetectPlayer();
                Debug.Log("case 0");
                break;
            case 1:
                // パトロール処理
                Patrol();
                Debug.Log("case 1");
                break;
            case 2:
                // 追跡処理
                if(chasemode == false){
                    InvokeRepeating("ChaseRepeat", 0f, chase_span);
                    chasemode = true;
                }
                Debug.Log("case 0");
                break;
        }
    }

    // プレイヤーを検知する処理
    void DetectPlayer()
    {
        if(playerobj != null){
            if(Vector2.Distance(gameObject.transform.position,playerobj.transform.position) < detect_range){
                state = 1;
                Debug.Log("trans1");
            }
        }
    }

    // パトロール処理
    void Patrol() 
    {
        //子からのプレイヤー感知
        if(ischased){
            state = 2;
        }
        //目的地切り替え
        if(patrolcheck == false && Vector2.Distance(transform.position,patrol_point) < 0.5){
            patrolcheck = true;
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if(patrolcheck && Vector2.Distance(transform.position,initialPosition) < 0.5){
            patrolcheck = false;
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        //目的とする位置に移動する際、見た目をスケールを変更し合わせる
        if(patrolcheck == false && Vector2.Distance(transform.position,patrol_point) > 0.5){
            if(transform.position.x < patrol_point.x && transform.localScale.x == 1)
                transform.localScale = new Vector3(-1,1,1);
            else if(transform.position.x > patrol_point.x && transform.localScale.x == -1)
                transform.localScale = new Vector3(1,1,1);
            Vector2 patrol_direction = (patrol_point - new Vector2(transform.position.x,transform.position.y)).normalized;
            gameObject.GetComponent<Rigidbody2D>().AddForce(patrol_direction * patrol_speed);
        }else if(patrolcheck && Vector2.Distance(transform.position,initialPosition) > 0.5){
            if(transform.position.x < initialPosition.x && transform.localScale.x == 1)
                transform.localScale = new Vector3(-1,1,1);
            else if(transform.position.x > initialPosition.x && transform.localScale.x == -1)
                transform.localScale = new Vector3(1,1,1);
            Vector2 patrol_direction = (initialPosition - new Vector2(transform.position.x,transform.position.y)).normalized;
            transform.GetComponent<Rigidbody2D>().AddForce(patrol_direction * patrol_speed);
        }
    }

    // 追跡処理
    void ChaseRepeat()
    {
        if (playerobj != null)
        {
            if(transform.position.x < playerobj.transform.position.x && transform.localScale.x == 1)
                transform.localScale = new Vector3(-1,1,1);
            else if(transform.position.x > playerobj.transform.position.x && transform.localScale.x == -1)
                transform.localScale = new Vector3(1,1,1);
            Vector2 direction = (new Vector2(playerobj.transform.position.x,playerobj.transform.position.y) - new Vector2(transform.position.x,transform.position.y)).normalized;
            gameObject.transform.GetComponent<Rigidbody2D>().AddForce(direction * chase_speed);
            Debug.Log("endtrans");
        }
    }

    // 衝突時の処理(攻撃時処理)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == playerobj)
        {
            // 3秒間追跡スピードを0にする
            StartCoroutine(StopChaseTemporarily());
            Debug.Log(collision.gameObject.name);
            //攻撃処理です、下のコメント外したら使えます。
            collision.gameObject.GetComponent<Player1>().DecreaseHP();
        }
    }

    // 追跡を3秒間停止するコルーチン
    IEnumerator StopChaseTemporarily()
    {
        Debug.Log("stoped!");
        float originalSpeed = chase_speed;
        chase_speed = 0;
        yield return new WaitForSeconds(3);
        chase_speed = originalSpeed;
    }
}
