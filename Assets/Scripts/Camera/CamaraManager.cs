using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraManager : MonoBehaviour
{
    public static CamaraManager instance;

    private List<Camera> cameras = new List<Camera>();

    private int _cameraIndex;

    private Camera _actualCamera;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCamera(Camera c)
    {
        if(!cameras.Contains(c))
        cameras.Add(c);
    }

    private void Update()
    {
        if (!GameManager.instance.SimulationOn()) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextCamera();
            ChangeActualCamera();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevCamera();
            ChangeActualCamera();
        }
    }

    private void ChangeActualCamera()
    {
        cameras[_cameraIndex].gameObject.SetActive(false);

        if (_cameraIndex > cameras.Count - 1)
            _cameraIndex = 0;

        _actualCamera = cameras[_cameraIndex];
        _actualCamera.gameObject.SetActive(true);
    }

    private void NextCamera()
    {
        _cameraIndex++;

        if (_cameraIndex > cameras.Count - 1)
            _cameraIndex = 0;
    }


    private void PrevCamera()
    {
        _cameraIndex--;

        if (_cameraIndex < 0)
            _cameraIndex = cameras.Count-1;
    }
}