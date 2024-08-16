using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageObjectChecker : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] Player1 player1;

    public bool isDamaged;

    private void Awake()
    {
        isDamaged = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("障害物と衝突しました");
            isDamaged = true;
            DecreaseHealth();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            isDamaged = false;
        }
    }

    void DecreaseHealth()
    {
        player1.DecreaseHP();
        uiManager.DecreaseDisplayHealth();
    }
}
