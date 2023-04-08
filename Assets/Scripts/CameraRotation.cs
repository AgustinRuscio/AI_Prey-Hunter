using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private float cameraSpeed = 100.0f;
    private bool AIStarted;

    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        //if (!AIStarted) return;

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, cameraSpeed * Time.deltaTime, 0);

        else if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, cameraSpeed * Time.deltaTime * -1, 0);
    }

    public void StartSimulation()
    {
        AIStarted = true;
    }
}
