using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    //コンポーネント
    PlayerInput playerInput;

    /*他クラス*/
    [SerializeField] Player1 player1;
    [SerializeField] CursorController cursorController;
    [SerializeField] FootingManager footingManager;
    [SerializeField] Circle circle;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] FootingType footingType = FootingType.stopRectangle; //初期は動かない長方形

    //プロパティ
    Vector3 point; //このオブジェクトの座標
    private GameObject obj;
    bool isClicked; //クリックしたかどうか

    //プレイヤー2に関するSE
    [SerializeField] PlayerSoundSource playerSoundSource;

    private const float POSITONZ = 300f;

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        footingManager.ChangeFooting(footingType);
        isClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private void OnEnable()
    {
        //イベント登録
        playerInput.actions["putFooting"].started += PutFooting;
        playerInput.actions["changeLengthOfFooting"].performed += StartExtendFooting;
        playerInput.actions["changeKindOfFooting"].performed += ChangeFootingType;
        playerInput.actions["changeKindOfWeapon"].performed += ChangeWeapon;
    }

    private void OnDisable()
    {
        playerInput.actions["putFooting"].started -= PutFooting;
        playerInput.actions["changeLengthOfFooting"].performed -= StartExtendFooting;
        playerInput.actions["changeKindOfFooting"].performed -= ChangeFootingType;

        playerInput.actions["changeKindOfWeapon"].performed -= ChangeWeapon;
    }

    //CACちゃんを常時追いかける(Update)
    public void chaseCAC(Vector3 position)
    {
        point = position;
        position.z = POSITONZ;
        this.transform.position = position;
    }

    //出現する足場を変える
    void ChangeFootingType(InputAction.CallbackContext ctx)
    {
        Debug.Log("足場を変更しました");
        // インクリメントし、範囲外に出たら最初に戻す
        footingType = (FootingType)(((int)footingType+1) % Enum.GetValues(typeof(FootingType)).Length);
        footingManager.ChangeFooting(footingType);
    }

    //足場を置く
    void PutFooting(InputAction.CallbackContext ctx)
    {
        bool isInstalled;
        isInstalled = footingManager.PutFooting(cursorController.point,footingType);
        if (isInstalled)
        {
            playerSoundSource.PlaySound(SEType.Asiba_put);
        }
    }

    //足場を伸ばす(クリックしている時)
    void StartExtendFooting(InputAction.CallbackContext ctx)
    {
        if (!isClicked)
        {
            obj = footingManager.SearchFootingObject(cursorController.point); //クリック座標にある足場をとってくる
            if(obj != null)
            {
                Debug.Log("足場をクリックしました");
            }
            else
            {
                Debug.Log("クリックを開始しました");
            }
            isClicked = true;
        }
        else
        {
            Debug.Log("クリック終了しました");
            if(obj != null)
            {
                Footing footing = obj.GetComponent<Footing>();
                if (footing != null)
                {
                    footing.FinishChangeRight();
                }
            }
            isClicked = false;
        }
        //if (ctx.phase == InputActionPhase.Started)
        //{
        //    Debug.Log("クリックを開始しました");
        //    gameObject = footingManager.SearchFootingObject(cursorController.point); // クリック座標にある足場を取得する
        //}
        //else if (ctx.phase == InputActionPhase.Canceled)
        //{
        //    Debug.Log("クリック終了しました");
        //    Footing footing = gameObject.GetComponent<Footing>();
        //    if (footing != null)
        //    {
        //        footing.FinishChangeRight();
        //    }
        //}

    }

    //武器を変更する
    void ChangeWeapon(InputAction.CallbackContext ctx)
    {
        Debug.Log("変更");
        //今持っている武器を取得する
        WeaponType currentType = player1.weaponType;
        //武器を更新する
        currentType = (WeaponType)(((int)currentType+1) % Enum.GetValues(typeof(WeaponType)).Length);
        player1.weaponType = currentType;
        Debug.Log(currentType+"に変更しました");

        weaponManager.ChangeWeapon(currentType);
    }
}
