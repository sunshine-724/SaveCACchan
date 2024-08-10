using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public Vector2 point {get;private set; }//マウスの座標(readOnly)

    // Start is called before the first frame update
    void Start()
    {
        // カーソルを自由に動かせる
        //Cursor.lockState = CursorLockMode.None;
        // カーソルを画面内で動かせる
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        // カーソルを画面中央にロックする
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //随時マウスの座標を更新する
        UpdateMousePosition();
    }

    void UpdateMousePosition()
    {
        // Mouse型でカーソル位置座標を取得する
        Vector2 position = Mouse.current.position.ReadValue();
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        Vector2 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
        point = screenToWorldPointPosition;
        //Debug.Log(point);
    }

    //カーソルの状態を変える
    void changeStateCursor()
    {
        if (Cursor.visible)
        {
            InvisibleCursor();
        }
        else
        {
            VisibleCursor();
        }
    }

    //カーソルを表示する
    void VisibleCursor()
    {
        Cursor.visible = true;
    }

    //カーソルを非表示にする
    void InvisibleCursor()
    {
        Cursor.visible = false;
    }

    
}
