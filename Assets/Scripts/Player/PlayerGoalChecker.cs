using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoalChecker : MonoBehaviour
{
    [SerializeField] ChangeScene changeScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            switch (changeScene.ReadCurrntScene())
            {
                case Scenes.TutorialStage:
                    changeScene.MoveGameClear_Tutorial();
                    break;
                case Scenes.MainStage:
                    changeScene.MoveGameClear_MainStage();
                    break;
            }
        }
    }
}
