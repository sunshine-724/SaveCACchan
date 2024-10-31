using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ending : MonoBehaviour
{
    [SerializeField] GameObject creditObj;
    [SerializeField] float afterActiveTime;

    [SerializeField] ChangeScene changeScene;

    [SerializeField] PlayerInput playerInput;

    private void Awake()
    {
        Invoke("ActiveCredit", afterActiveTime);
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["ClickToMoveTitle"].started -= ChangeTitle;
        }
    }

    private void ActiveCredit()
    {
        creditObj.SetActive(true);
        if (playerInput != null)
        {
            playerInput.actions["ClickToMoveTitle"].started += ChangeTitle;
        }
    }

    private void ChangeTitle(InputAction.CallbackContext ctx)
    {
        changeScene.MoveTitle();
    }
}
