using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoalChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            Debug.Log("ゴールしました");
        }
    }
}
