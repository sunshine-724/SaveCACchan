using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//サークルにアタッチ
public class Circle : MonoBehaviour
{
    Vector3 centerPoint; //サークルの中心座標
    [SerializeField] float footingRadius; //サークルの半径

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        centerPoint = this.transform.position;
    }

    //与えられた座標がサークルの領域内にあるかどうか
    public bool CheckPointinCircle(Vector2 point)
    {
        if(Mathf.Abs(centerPoint.x - point.x) <= footingRadius)
        {
            if(Mathf.Abs(centerPoint.y - point.y) <= footingRadius)
            {
                return true;
            }
        }

        return false;
    }
}
