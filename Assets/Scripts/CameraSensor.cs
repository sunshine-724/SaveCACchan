using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraState
{
    Move,
    fix,
}
public class CameraSensor : MonoBehaviour
{
    [SerializeField] Player1 player1;
    [SerializeField] float enableSensorDistance = 7.5f;
    CameraState cameraState = CameraState.fix;

    bool isAim;

    private void Awake()
    {
        isAim = false;
    }

    private void Update()
    {
        if (Mathf.Abs(this.transform.position.x - player1.transform.position.x) > enableSensorDistance)
        {
            isAim = true;
        }
    }

    private void FixedUpdate()
    {
        if (isAim)
        {
            Vector3 nextPositon = player1.transform.position;
            //nextPositon.x += enableSensorDistance;
            this.transform.position = nextPositon;
        }
    }

    public void StopPlayer()
    {
        isAim = false;
    }
}
