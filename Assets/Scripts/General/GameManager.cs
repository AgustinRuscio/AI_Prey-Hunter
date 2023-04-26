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
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;

        _preysAlive = FlokckingManager.instance.flockMates.Count;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            SimulationEnd();
    }

    public bool SimulationStatus()
    {
        return _simulationStatus;
    }


    public void check()
    {
        Debug.Log("Entre al check check");
        _preysAlive--;

        if (_preysAlive <= 0)
        {
            SimulationEnd();
            Debug.Log("End");
        }

        Debug.Log("alive" + _preysAlive + "Sigo vivo");
    }

    private void SimulationEnd()
    {
        _simulationStatus = false;

        Time.timeScale = 0;
        _canvas.SetActive(true);
    }

}