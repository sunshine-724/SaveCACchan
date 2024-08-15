using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//地面オブジェクトにはこのスクリプトをアタッチするのと地面オブジェクトにGroundタグをつける
public class GroundObserver : MonoBehaviour, IGroundObserver
{
    void Start()
    {
        PlayerGroundChecker groundChecker = FindObjectOfType<PlayerGroundChecker>();
        if (groundChecker != null)
        {
            groundChecker.AddObserver(this);
        }
    }

    public void OnGroundStateChanged(bool isGrounded)
    {
        if (isGrounded)
        {
            //Debug.Log("Player is grounded.");
        }
        else
        {
            //Debug.Log("Player is not grounded.");
        }
    }
}
