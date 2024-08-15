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

    private void Awake()
    {
        isGrounded = true;
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

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            NotifyObservers();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            NotifyObservers();
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

