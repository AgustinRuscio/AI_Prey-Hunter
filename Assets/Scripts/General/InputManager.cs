using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseCamera;


    private bool _isPaused;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
            Pause();
        else if(Input.GetKeyDown(KeyCode.Escape) && _isPaused)
            Resume();
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        GameManager.instance.ChangeSimulationStatus(false);
        _pauseCamera.SetActive(true);
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        GameManager.instance.ChangeSimulationStatus(true);
        _pauseCamera.SetActive(false);
    }
}
