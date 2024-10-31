using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// サークルにアタッチ
public class Circle : MonoBehaviour
{
    private Vector2 centerPoint; // サークルの中心座標
    [SerializeField] private float footingRadius; // サークルの半径
    [SerializeField] private float notAreaRadius = 3.0f; // この半径内は領域外とする

    // Start is called before the first frame update
    void Start()
    {
        centerPoint = transform.position;
    }

    // 与えられた座標がサークルの領域内にあるかどうか
    public bool IsPointInCircle(Vector2 clickPoint)
    {
        float clickRadius = Vector2.Distance(centerPoint, clickPoint);

        return notAreaRadius <= clickRadius && clickRadius <= footingRadius;
    }
}
