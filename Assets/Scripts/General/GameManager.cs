using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool _simulationStatus = true;
    
    private int _preysAlive = 0;

    [SerializeField]
    private GameObject _canvas;


    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1;

        StartCoroutine(WaitForAdded());
    }

    IEnumerator  WaitForAdded()
    {
        yield return new WaitForSeconds(.5f);
        _preysAlive = FlokckingManager.instance.ReturnTotalPreys();
    }

    public void ChangeSimulationStatus(bool status)
    {
        _simulationStatus = status;
    }

    public bool SimulationOn()
    {
        return _simulationStatus;
    }

    public void CheckPreysRemained()
    {
        _preysAlive--;

        if (_preysAlive <= 0)
            StartCoroutine(WaitforEnd());
    }

    private void SimulationEnd()
    {
        _simulationStatus = false;

        Time.timeScale = 0;
        _canvas.SetActive(true);
    }

    IEnumerator WaitforEnd()
    {
        yield return new WaitForSeconds(2);
        SimulationEnd();
    }

}