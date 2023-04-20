using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : States
{
    private float _actualEnergy;
    private float _maxEnergy;

    private Hunter _myHunter;
    public RestState(float actualEnergy, float maxEnergy, Hunter hunter)
    {
        _myHunter = hunter;
    }

    public override void OnStart()
    {
        _myHunter.ApplyForce(Vector3.zero);
    }

    public override void OnStop() { }

    public override void Update() 
    {
        if (_myHunter._actualEnergy !< _myHunter._maxEnergy)
            Rest();
        else 
            ReturntoPatrol();
    }

    private void Rest()
    {
        // accion de rest de no moverse y una animacion de descanso 

        _myHunter._actualEnergy += Time.deltaTime;
    }

    private void ReturntoPatrol()
    {
        //finiteStateMach.ChangeState(AgentStates.Patrol);
        Debug.Log("Cambio");
    }


}
