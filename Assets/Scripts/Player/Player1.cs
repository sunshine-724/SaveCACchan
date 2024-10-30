using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMode
{
    keyboard, //デバッグ用
    keyboardAndGamePad, //本番環境
}

public enum Direction
{
    left,
    right,
}

public enum PlayerAction
{
    jump,
    attack,
}

public enum PlayerState
{
    LeftIdle,
    LeftRun,
    RightIdle,
    RightRun,
}

//CACちゃん本体にアタッチするスクリプト
public class Player1 : MonoBehaviour
{
    /*コンポーネント*/
    Rigidbody2D rb;
    PlayerInput playerInput;

    /*他クラス*/
    [SerializeField] Player2 player2;
    [SerializeField] PlayerGroundChecker playerGroundChecker;
    [SerializeField] CameraSensor cameraSensor;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] UIManager uiManager;

    //ステータス
    public Vector3 point { get; private set; }//このプレイヤーの座標
    [SerializeField] int HP = 3; //残機
    public WeaponType weaponType; //持っている武器
    [SerializeField] float horizonSpeed;
    [SerializeField] float verticalSpeed;
    [SerializeField] Direction direction = Direction.right; //現在向いている向き
    private PlayerState playerState = PlayerState.RightIdle; //プレイヤーの状態

    private bool isPriorityAnimation = false; //特定のアニメーションを優先するかどうか
    private bool invincible = false; //このキャラが無敵かどうか
    [SerializeField] float invincibleTime = 1.5f; //無敵時間

    //アニメーター
    [SerializeField] Animator animPlayer1; //プレイヤー1のアニメーター
    private Animator animator; //武器のアニメーター

    //プレイヤー1に関するSE
    [SerializeField] PlayerSoundSource playerSoundSource;

    //Inputの種類
    [SerializeField] InputMode inputMode = InputMode.keyboardAndGamePad;
    Vector2 moveInput; //スティックの入力量


    //デバッグモード(InputMode.keyboard)のみ
    float lastInputTime;
    [SerializeField] float timeoutDuration = 0.3f; // 何秒間の無入力状態を許容するか
    private InputAction HoldDKeyAction;
    private InputAction HoldAKeyAction;

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
        else
        {
            if(inputMode == InputMode.keyboard)
            {
                HoldDKeyAction = playerInput.actions["OnRightMove"];
                HoldAKeyAction = playerInput.actions["OnLeftMove"];
            }
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
        moveInput = playerInput.actions["OnMove"].ReadValue<Vector2>();
        point = rb.transform.position;
        player2.chaseCAC(this.transform.position);

        if(inputMode == InputMode.keyboard)
        {
            // ReadValue<float>()でキーの値を取得（押されている間は1、離されたら0）
            float DkeyHeld = HoldDKeyAction.ReadValue<float>();
            float AkeyHeld = HoldAKeyAction.ReadValue<float>();

            if (DkeyHeld == 1f)
            {
                Debug.Log("Dキーが押され続けています。");
                Vector2 value = new Vector2(DkeyHeld, 0.0f);
                Move(value);
            }else if(AkeyHeld == 1f)
            {
                Debug.Log("Aキーが押され続けています。");
                Vector2 value = new Vector2(-1 * AkeyHeld, 0.0f);
                Move(value);
            }else
            {
                Debug.Log("キーが離されています。");
                //もし一定時間入力がなければ止まる(debugモードのみ)
                if (Time.time - lastInputTime > timeoutDuration)
                {
                    Vector2 stopVector2 = new Vector2(0.0f, rb.velocity.y);
                    Move(stopVector2);
                }
            }
        }

        if(Mathf.Abs(rb.velocity.x) > horizonSpeed)
        {
            if(direction == Direction.left)
            {
                rb.velocity = new Vector2(-1 * horizonSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(horizonSpeed, rb.velocity.y);
            }
        }

        if(playerGroundChecker.isGrounded && Mathf.Abs(moveInput.x) < 0.2f)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }

        //if((playerState == PlayerState.LeftIdle) || (playerState == PlayerState.RightIdle)){
        //    if (playerGroundChecker.isGrounded)
        //    {
        //        this.rb.velocity = Vector2.zero;
        //    }
        //}

        //if (direction == Direction.left)
        //{
        //    animPlayer1.SetFloat("BlendParam", 0);
        //}
        //else
        //{
        //    animPlayer1.SetFloat("BlendParam", 1);
        //}


        //if (direction == Direction.left)
        //{
        //    animPlayer1.SetFloat("BlendParam", (float)PlayerState.LeftIdle);
        //}
        //else if(direction == Direction.right)
        //{
        //    animPlayer1.SetFloat("BlendParam", (float)PlayerState.RightIdle);
        //}
    }

    private void FixedUpdate()
    {
        
    }

    private void OnEnable()
    {
        //イベント登録
        if(inputMode == InputMode.keyboardAndGamePad)
        {
            playerInput.actions["OnMove"].started += OnMove;
            playerInput.actions["OnMove"].performed += OnMove;
            playerInput.actions["OnMove"].canceled += OnMove;
            playerInput.actions["OnJump"].started += OnJump;
            playerInput.actions["Attack"].performed += Attack;
        }
        else if(inputMode == InputMode.keyboard)
        {
            playerInput.actions["OnRightMove"].performed += OnRightMove;
            playerInput.actions["OnRightMove"].canceled += OnRightMove;
            playerInput.actions["OnLeftMove"].performed += OnLeftMove;
            playerInput.actions["OnLeftMove"].canceled += OnLeftMove;
            playerInput.actions["OnJump"].started += OnJump;
            playerInput.actions["Attack"].performed += Attack;

            playerInput.onActionTriggered += OnActionTriggered; //任意のアクション時起動
        }
    }

    private void OnDisable()
    {
        //イベント削除
        if(inputMode == InputMode.keyboardAndGamePad)
        {
            playerInput.actions["OnMove"].started -= OnMove;
            playerInput.actions["OnMove"].performed -= OnMove;
            playerInput.actions["OnMove"].canceled -= OnMove;
            playerInput.actions["OnJump"].started -= OnJump;
            playerInput.actions["Attack"].performed -= Attack;
        }else if(inputMode == InputMode.keyboard)
        {
            playerInput.actions["OnRightMove"].performed -= OnRightMove;
            playerInput.actions["OnRightMove"].canceled -= OnRightMove;
            playerInput.actions["OnLeftMove"].performed -= OnLeftMove;
            playerInput.actions["OnLeftMove"].canceled -= OnLeftMove;
            playerInput.actions["OnJump"].started -= OnJump;
            playerInput.actions["Attack"].performed -= Attack;

            playerInput.onActionTriggered -= OnActionTriggered;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext ctx)
    {
        lastInputTime = Time.time; //入力された最新の時間を記録する
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        value.y = 0.0f;
        Move(value);
    }

    private void OnRightMove(InputAction.CallbackContext ctx)
    {
        //float f_value = ctx.ReadValue<float>();
        //Vector2 value = new Vector2(f_value, 0.0f);

        //Debug.Log(value);
        //Move(value);
    }

    private void OnLeftMove(InputAction.CallbackContext ctx)
    {
        //float f_value = ctx.ReadValue<float>();
        //Vector2 value = new Vector2(-1 * f_value, 0.0f);
        //Move(value);
    }

    private void Move(Vector2 value)
    {
        //2値化処理
        if (value.x >= 0.2f)
        {
            value = Right(value);

            //animPlayer1.SetBool("isLeftRun", false);
            //animPlayer1.SetBool("isRightRun", true);
            playerState = PlayerState.RightRun;
            animPlayer1.SetFloat("BlendParam", (float)PlayerState.RightRun);

            animPlayer1.SetBool("isRight", true);
            animPlayer1.SetBool("isLeft", false);

            direction = Direction.right;
        }
        else if (value.x <= -0.2f)
        {
            value = Left(value);
            //animPlayer1.SetBool("isRightRun", false);
            //animPlayer1.SetBool("isLeftRun", true);
            playerState = PlayerState.LeftRun;
            animPlayer1.SetFloat("BlendParam", (float)PlayerState.LeftRun);

            animPlayer1.SetBool("isRight", false);
            animPlayer1.SetBool("isLeft", true);
            direction = Direction.left;
        }
        else
        {
            //もし接地していなかったら、停止しない
            if (!playerGroundChecker.isGrounded)
            {
                //this.rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y); //停止しない
                StartCoroutine(DelayStopPlayer()); //接地した段階で停止する
            }
            else
            {
                this.rb.velocity = new Vector2(0.0f,rb.velocity.y); //停止
            }

            //animPlayer1.SetBool("isLeftRun", false);
            //animPlayer1.SetBool("isRightRun", false);

            if (direction == Direction.right)
            {
                playerState = PlayerState.RightIdle;
                animPlayer1.SetFloat("BlendParam", (float)PlayerState.RightIdle);
            }
            else if(direction == Direction.left)
            {
                playerState = PlayerState.LeftIdle;
                animPlayer1.SetFloat("BlendParam", (float)PlayerState.LeftIdle);
            }
            cameraSensor.StopPlayer();
            return;
        }
        value.x = value.x * horizonSpeed;

        rb.AddForce(value, ForceMode2D.Impulse); //プレイヤーに力を加え移動させる
    }

    IEnumerator DelayStopPlayer()
    {
        while (!playerGroundChecker.isGrounded)
        {
            Debug.Log("空中で停止しました");

            Debug.Log(playerGroundChecker.isGrounded);
            yield return null;
        }
        this.rb.velocity = new Vector2(0.0f,rb.velocity.y);
        Debug.Log("停止しました");
    }

    Vector2 Right(Vector2 value)
    {
        if (this.rb.velocity.x <= 0)
        {
            value.x = 1.0f;
            //ChangePlayerRotation(value);
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
            //ChangePlayerRotation(value);
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
        Vector2 v = new Vector2(rb.velocity.x, value*verticalSpeed);

        //if(animPlayer1.GetBool("isRightRun") || animPlayer1.GetBool("isLeftRun"))
        //{
        //    isPriorityAnimation = true;
        //    animPlayer1.SetBool("isRightRun", false);
        //    animPlayer1.SetBool("isLeftRun", false);
        //}

        if(((int)animPlayer1.GetFloat("BlendParam") == (float)PlayerState.LeftRun) || ((int)animPlayer1.GetFloat("BlendParam") == (float)PlayerState.LeftRun))
        {
            isPriorityAnimation = true;
        }

        animPlayer1.SetTrigger("JumpTrigger"); //ジャンプアニメーション起動
        playerSoundSource.PlaySound(SEType.Jump); //ジャンプSEを起動
        rb.AddForce(v,ForceMode2D.Impulse);
        StartCoroutine(WaitAnimation(PlayerAction.jump)); //ジャンプしている間は走るアニメーションをオフにし、ジャンプし終わったらアニメーションを再びおん
        Debug.Log("ジャンプしています");
    }

    private IEnumerator WaitAnimation(PlayerAction playerAction) {

        switch (playerAction)
        {
            case PlayerAction.jump:
                while (!isJumpedAnimation())
                {
                    yield return null;
                }
                break;

            case PlayerAction.attack:
                while (!isAttackedAnimation())
                {
                    yield return null;
                }
                break;

            default:
                break;
        }

        if (isPriorityAnimation)
        {
            isPriorityAnimation = false;

            if (direction == Direction.right)
            {
                animPlayer1.SetFloat("BlendParam", (float)PlayerState.RightRun);
                //animPlayer1.SetBool("isRightRun", true);
            }
            else
            {
                animPlayer1.SetFloat("BlendParam", (float)PlayerState.LeftRun);
                //animPlayer1.SetBool("isLeftRun", true);
            }
        }
    }

    private bool isJumpedAnimation()
    {
        // ジャンプアニメーションの進行状態を確認
        AnimatorStateInfo stateInfo = animPlayer1.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Jump"))
        {
            return true;
        }

        return false;
    }

    private bool isAttackedAnimation()
    {
        // アタックアニメーションの進行状態を確認
        AnimatorStateInfo stateInfo = animPlayer1.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack"))
        {
            return true;
        }

        return false;
    }

    //プレイヤーの向き(左右)を変更する
    //void ChangePlayerRotation(Vector2 value)
    //{
    //    Vector3 vector3 = rb.transform.localScale;
    //    if(value.x > 0)
    //    {
    //        vector3.x = 1.0f;
    //    }
    //    else
    //    {
    //        vector3.x = -1.0f;
    //    }

    //    rb.transform.localScale = vector3;
    //    Debug.Log("rotationしました");
    //}



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
            playerSoundSource.PlaySound(SEType.Damaged); //ダメージSEを起動
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
            case WeaponType.bubble:
                if(((int)animPlayer1.GetFloat("BlendParam") == (float)PlayerState.LeftRun) || ((int)animPlayer1.GetFloat("BlendParam") == (float)PlayerState.LeftRun))
                {
                    isPriorityAnimation = true;
                }
                //if (animPlayer1.GetBool("isRightRun") || animPlayer1.GetBool("isLeftRun"))
                //{
                //    isPriorityAnimation = true;
                //    animPlayer1.SetBool("isRightRun", false);
                //    animPlayer1.SetBool("isLeftRun", false);
                //}

                if(direction == Direction.left)
                {
                    animPlayer1.SetFloat("AttackBlendParam", 0.0f);
                }else if(direction == Direction.right)
                {
                    animPlayer1.SetFloat("AttackBlendParam", 1.0f);
                }

                animPlayer1.SetTrigger("AttackTrigger"); //アタックアニメーション起動
                playerSoundSource.PlaySound(SEType.Attack); //アタックSE起動
                StartCoroutine(WaitAnimation(PlayerAction.attack));
                weaponManager.Attack(WeaponType.bubble, this.transform.position,this.direction);
                break;
        }
    }
}
