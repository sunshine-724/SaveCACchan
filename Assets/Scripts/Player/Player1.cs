using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

//CACちゃん本体にアタッチするスクリプト
public class Player1 : MonoBehaviour
{
    /*コンポーネント*/
    Rigidbody2D rb;
    PlayerInput playerInput;

    /*他クラス*/
    [SerializeField] Player2 player2;
    [SerializeField] PlayerGroundChecker playerGroundChecker;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] UIManager uiManager;


    public Vector3 point { get; private set; }//このプレイヤーの座標
    [SerializeField] int HP = 3; //残機
    public WeaponType weaponType; //持っている武器
    [SerializeField] float horizonSpeed;
    [SerializeField] float verticalSpeed;

    private bool invincible = false; //このキャラが無敵かどうか
    [SerializeField] float invincibleTime = 1.5f; //無敵時間

    //武器のアニメーター
    private Animator animator;

    private void Awake()
    {
        //初期武器を装備する
        weaponManager.ChangeWeapon(weaponType);

        rb = this.GetComponent<Rigidbody2D>();
        playerInput = this.GetComponent<PlayerInput>();
        animator = this.GetComponent<Animator>();

        if(playerInput == null)
        {
            Debug.Log("キー入力が登録されていません");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(horizonSpeed == 0 || verticalSpeed == 0)
        {
            Debug.Log("スピードが設定されていません");
        }

        point = this.transform.position; //初期座標を格納する
    }


    //private void FixedUpdate()
    //{
    //    player2.chaseCAC(this.transform.position);
    //}

    private void Update()
    {
        point = rb.transform.position;
        player2.chaseCAC(this.transform.position);
    }

    private void OnEnable()
    {
        //イベント登録
        playerInput.actions["OnMove"].started += OnMove;
        playerInput.actions["OnMove"].performed += OnMove;
        playerInput.actions["OnMove"].canceled += OnMove;
        playerInput.actions["OnJump"].started += OnJump;
        playerInput.actions["Attack"].performed += Attack;
    }

    private void OnDisable()
    {
        //イベント削除
        playerInput.actions["OnMove"].started -= OnMove;
        playerInput.actions["OnMove"].performed -= OnMove;
        playerInput.actions["OnMove"].canceled -= OnMove;
        playerInput.actions["OnJump"].started -= OnJump;
        playerInput.actions["Attack"].performed -= Attack;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        value.y = 0.0f;
        Move(value);
    }

    private void Move(Vector2 value)
    {
        //2値化処理
        if (value.x >= 0.3f)
        {
            value = Right(value);
        }
        else if (value.x <= -0.3f)
        {
            value = Left(value);
        }
        else
        {
            Debug.Log("止まりました");
            this.rb.velocity = new Vector2(0.0f, 0.0f); //停止
            return;
        }
        value.x = value.x * horizonSpeed;

        rb.AddForce(value, ForceMode2D.Impulse);
    }

    Vector2 Right(Vector2 value)
    {
        if (this.rb.velocity.x <= 0)
        {
            value.x = 1.0f;
            ChangePlayerRotation(value);
            //Debug.Log("右に移動しています");
        }
        else
        {
            value.x = 0.0f;
            //Debug.Log("右に移動していますが力を加えていません");
        }

        return value;
    }

    Vector2 Left(Vector2 value)
    {
        if (this.rb.velocity.x >= 0)
        {
            value.x = -1.0f;
            ChangePlayerRotation(value);
            //Debug.Log("左に移動しています");
        }
        else
        {
            value.x = 0.0f;
            //Debug.Log("左に移動していますが力を加えていません");
        }

        return value;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        //もし空中にいる場合ジャンプできない
        if (!playerGroundChecker.CanJump())
        {
            Debug.Log("空中にいるためジャンプできませんでした");
            return;
        }
        float value = ctx.ReadValue<float>();
        Vector2 v = new Vector2(0, value*verticalSpeed);
        rb.AddForce(v,ForceMode2D.Impulse);
        //Debug.Log("ジャンプしています");
    }

    //プレイヤーの向き(左右)を変更する
    void ChangePlayerRotation(Vector2 value)
    {
        Vector3 vector3 = rb.transform.localScale;
        if(value.x > 0)
        {
            vector3.x = 0.6f;
        }
        else
        {
            vector3.x = -0.6f;
        }
        
        rb.transform.localScale = vector3;
        Debug.Log("rotationしました");
    }

    //HPを減らす
    public void DecreaseHP()
    {
        //無敵ではなかったら
        if (!invincible)
        {
            //プレイヤーを止める
            if(rb != null)
            {
                Vector3 tmp = new Vector3(0.0f, rb.velocity.y, 0);
                rb.velocity = tmp;
            }
           

            HP--;
            uiManager.DecreaseDisplayHealth();
            StartCoroutine(AffectInvincible()); //一定時間無敵を付与する
        }

        //もしHPが0になったら
        if (HP == 0)
        {

        }
    }

    IEnumerator AffectInvincible()
    {
        invincible = true; //一定時間無敵にする
        yield return new WaitForSeconds(invincibleTime);
        invincible = false; //無敵解除
    }

    //武器で攻撃する
    //受け取った武器に応じて攻撃する
    public void Attack(InputAction.CallbackContext ctx)
    {
        switch (weaponType)
        {
            case WeaponType.gun:
                StartCoroutine(AttackGun());
                break;
        }
    }

    private IEnumerator AttackGun()
    {
        Debug.Log("攻撃を開始しました");
        if(animator != null)
        {

        }

        yield return null;
    }
}
