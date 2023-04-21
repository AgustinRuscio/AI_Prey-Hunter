using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : States
{
    private Hunter _myHunter;
    
    public RestState(Hunter hunter)
    {
        _myHunter = hunter;
    }

    public override void OnStart(params object[] parameters)
    {
        _myHunter.ApplyForce(Vector3.zero);
        EventManager.Trigger(EventEnum.HunterRest, true);
    }

    public override void OnStop()
    { 
        EventManager.Trigger(EventEnum.HunterRest, false);
        
    }

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
