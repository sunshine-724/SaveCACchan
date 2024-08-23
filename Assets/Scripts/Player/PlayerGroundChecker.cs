using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGroundObserver
{
    void OnGroundStateChanged(bool isGrounded);
}

public class PlayerGroundChecker : MonoBehaviour
{
    private bool isGrounded;
    private List<IGroundObserver> observers = new List<IGroundObserver>();
    BoxCollider2D coll;


    private void Awake()
    {
        isGrounded = true;
        coll = this.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        //coll.enabled = false;

        Vector3 tmp = transform.position;
        tmp.y -= 3.0f; //自分自身を判定しないように補正を加える

        //3つのrayで接地判定する
        Vector3 raypos = tmp;
        Vector3 rayposLeft = tmp;
        Vector3 rayposRight = tmp;

        rayposLeft.x -= 1.0f;
        rayposRight.x += 1.0f;


        RaycastHit2D hit = Physics2D.Raycast(raypos, Vector2.down, 3.0f);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayposLeft, Vector2.down,3.0f);
        RaycastHit2D hitRight = Physics2D.Raycast(rayposRight, Vector2.down, 3.0f);


        if(hit.collider != null)
        {
            Debug.Log(hit.collider);
            if (Collision(hit.collider))
            {
                isGrounded = true;
                return;
            }
            else
            {
                isGrounded = false;
            }
        }

        if(hitLeft.collider != null)
        {
            if (Collision(hitLeft.collider))
            {
                isGrounded = true;
                return;
            }
            else
            {
                isGrounded = false;
            }
        }

        if(hitRight.collider != null)
        {
            if (Collision(hitRight.collider))
            {
                isGrounded = true;
                return;
            }
            else
            {
                isGrounded = false;
            }
        }

        //coll.enabled = true;
    }

    private bool Collision(Collider2D collider)
    {
        if (collider.CompareTag("Ground") || collider.CompareTag("Obstacles"))
        {
            Debug.Log("true");
            return true;
        }
        else
        {
            Debug.Log("false");
            return false;
        }
    }

    public void AddObserver(IGroundObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IGroundObserver observer)
    {
        observers.Remove(observer);
    }

    void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnGroundStateChanged(isGrounded);
        }
    }

    public bool CanJump()
    {
        if (isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

