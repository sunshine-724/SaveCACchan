using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoovingSc : MonoBehaviour
{
    //移動先の相対座標を設定
    [SerializeField] float block_destx;
    [SerializeField] float block_desty;

    [SerializeField] float moving_speed;
    private bool moving_switch;
    private Vector2 block_destpos;

    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        block_destpos = new Vector2(transform.position.x + block_destx, transform.position.y + block_desty);
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //目的地切り替え
        if (moving_switch == false && Vector2.Distance(transform.position, block_destpos) < 0.5)
        {
            moving_switch = true;
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if (moving_switch && Vector2.Distance(transform.position, initialPosition) < 0.5)
        {
            moving_switch = false;
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (moving_switch == false && Vector2.Distance(transform.position, block_destpos) > 0.5)
        {
            Vector2 patrol_direction = (block_destpos - new Vector2(transform.position.x, transform.position.y)).normalized;
            gameObject.GetComponent<Rigidbody2D>().AddForce(patrol_direction * moving_speed);
        }
        else if (moving_switch && Vector2.Distance(transform.position, initialPosition) > 0.5)
        {
            Vector2 patrol_direction = (initialPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
            transform.GetComponent<Rigidbody2D>().AddForce(patrol_direction * moving_speed);
        }
    }

}
