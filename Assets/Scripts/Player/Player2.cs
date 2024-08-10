using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    //コンポーネント
    PlayerInput playerInput;

    /*他クラス*/
    [SerializeField] Circle circle;

    //プロパティ
    Vector3 point; //このオブジェクトの座標

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private void OnEnable()
    {
        //イベント登録
        playerInput.actions["putFooting"].started += PutFooting;
    }

    private void OnDisable()
    {
        playerInput.actions["putFooting"].started -= PutFooting;
    }

    //CACちゃんを常時追いかける(Update)
    public void chaseCAC(Vector3 position)
    {
        point = position;
        this.transform.position = position;
    }

    void PutFooting(InputAction.CallbackContext ctx)
    {
        
    }
}
