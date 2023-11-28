using System;
using System.Collections.Generic;
using UnityEngine;

public class PreyReSpawner : MonoBehaviour
{
    public static PreyReSpawner instance;
    
    private List<Tuple<GenericTimer, Prey>> allPreysOnCD = new ();
    
    private GenericTimer _timer;
    
    [SerializeField]
    private float _reSpawnCD;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (allPreysOnCD.Count == 0) return;

        foreach (var VARIABLE in allPreysOnCD)
        {
            VARIABLE.Item1.RunTimer();

            if (VARIABLE.Item1.CheckCoolDown())
            {
                EventManager.Trigger(EventEnum.PreyRespawn, VARIABLE.Item2);
                allPreysOnCD.Remove(VARIABLE);
                return;
            }
        }
        return;
    }

    public void SetNewDeathPrey(Prey p)
    {
        _timer = new GenericTimer(_reSpawnCD);
        allPreysOnCD.Add(Tuple.Create(_timer, p));
    }
}
