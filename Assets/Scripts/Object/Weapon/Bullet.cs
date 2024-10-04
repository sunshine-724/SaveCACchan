using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 0.1f; //銃弾のスピード

    private void FixedUpdate()
    {
        float direction = this.transform.localScale.x;
        Vector3 firePos = this.transform.position;
        if (direction >= 0)
        {
            firePos.x += bulletSpeed;
        }
        else
        {
            firePos.x -= bulletSpeed;
        }

        this.transform.position = firePos;
    }
}
