using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 100.0f;
 
    void Update() => MoveCamera();
    

    private void MoveCamera()
    {
        if (!GameManager.instance.SimulationOn()) return;

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, cameraSpeed * Time.deltaTime * -1, 0);
        else if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, cameraSpeed * Time.deltaTime , 0);
    }
}