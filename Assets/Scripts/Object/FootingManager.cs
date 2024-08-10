using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FootingManager : MonoBehaviour
{
    int maxFootingNumber = 3; //置ける足場の最大個数(3個に設定)
    int nowfootingNumber; //現在ある足場の数

    // Start is called before the first frame update
    void Start()
    {
        nowfootingNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //足場を置くメソッド
    void PutFooting(InputAction.CallbackContext ctx)
    {
        if (CanPutFooting())
        {
            
        }
    }

    //足場を置けるかどうかを判断するメソッド
    bool CanPutFooting()
    {
        //条件1.現在ある足場の個数が最大個数に達していないかどうか
        if (nowfootingNumber < maxFootingNumber)
        {
            //条件2.マウスの場所がサークル内にあるかどうか

        }
        return false;
    }

    //現在ある足場の数を増やす
    void incrementFootingNumber()
    {
        nowfootingNumber++;
    }
}
