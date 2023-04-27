using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlokckingManager : MonoBehaviour
{
    public static FlokckingManager instance;

    private int _totalPreys;
    
    public HashSet<Prey> flockMates = new HashSet<Prey>();
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPrey(Prey p)
    {
        if (!flockMates.Contains(p))
        {
            flockMates.Add(p);
            _totalPreys += 1;
        }
    }

    public int ReturnTotalPreys()
    {
        return _totalPreys;
    }

    public void RemovePrey(Prey p)
    {
        if (flockMates.Contains(p))
        {
            flockMates.Remove(p);
            GameManager.instance.CheckPreysRemained();
        }
    }
}