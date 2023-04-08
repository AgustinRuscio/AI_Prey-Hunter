using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform targetPoint;

    [HideInInspector] public float minZoom = 30;
    [HideInInspector] public float maxZoom = 30;

    private bool AIStarted;
    private float speed = 10;

    void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        //if (!AIStarted) return;

        if (Input.GetKey(KeyCode.W))
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);

            if (distanceToTarget >= minZoom)
                transform.position += transform.forward * speed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);

            if (distanceToTarget <= maxZoom)
                transform.position -= transform.forward * speed * Time.deltaTime;
        }
    }

    public void StartSimulation()
    {
        AIStarted = true;
    }
}
