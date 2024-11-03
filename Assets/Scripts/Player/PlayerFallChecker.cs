using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーにアタッチ
public class PlayerFallChecker : MonoBehaviour
{
    [SerializeField] PlayerGroundChecker playerGroundChecker;
    [SerializeField] float YERROR = 2.0f; //補正

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("何か当たりました");
        if (collision.CompareTag("FallSensor"))
        {
            Debug.Log("落下しました");
            Vector3 targetPosition = playerGroundChecker.lastGround.transform.position;
            targetPosition.z = 0.0f;
            targetPosition.y += YERROR; //そのままだとゲームオブジェクトに対して埋め込まれるので補正をかける
            this.transform.position = targetPosition;//最後に接地したゲームオブジェクトの中心座標に移動する
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("FallSensor"))
        {
            Debug.Log("落下しました");
            Vector3 targetPosition = playerGroundChecker.lastGround.transform.position;
            targetPosition.z = 0.0f;
            targetPosition.y += YERROR; //そのままだとゲームオブジェクトに対して埋め込まれるので補正をかける
            this.transform.position = targetPosition;//最後に接地したゲームオブジェクトの中心座標に移動する
        }
    }


}
