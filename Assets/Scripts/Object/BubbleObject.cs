using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleObject : MonoBehaviour
{
    private bool isMove = false;
    private Direction direction;
    private float movedDistance = 0.0f;
    [SerializeField] float canMoveDistance = 35.0f;

    [SerializeField] float speed = 0.5f;

    private void FixedUpdate()
    {
        if (isMove)
        {
            Vector3 nextPosition = this.transform.position;
            switch (direction)
            {
                case Direction.left:
                    nextPosition.x -= speed;
                    break;

                case Direction.right:
                    nextPosition.x += speed;
                    break;

                default:
                    break;
            }

            movedDistance += speed;
            this.transform.position = nextPosition;
        }

        if(movedDistance > canMoveDistance)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(Direction playerDirection)
    {
        isMove = true;
        this.direction = playerDirection;
        Debug.Log("directionは" + this.direction + "です");
    }
}
