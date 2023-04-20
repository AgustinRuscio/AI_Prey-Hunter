using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : States
{
    protected float _actualEnergy;
    float _maxEnergy;
    float _minEnergy;



    public RestState(float actualEnergy, float maxEnergy, float minEnergy)
    {

        _actualEnergy = actualEnergy;
        _maxEnergy = maxEnergy;
        _minEnergy = minEnergy;
    }

    public override void OnStart()
    {
        
    }

    public override void OnStop()
    {
       
    }

    public override void Update()
    {

        if (_actualEnergy == _minEnergy)
        {
            Rest();
           
            
        }
        else if (_actualEnergy == _maxEnergy)
        {
            ReturntoPatrol();

        }


    }

   

    private void Rest()
    {

        // accion de rest de no moverse y una animacion de descanso 


        if (_actualEnergy == _minEnergy || _actualEnergy < _maxEnergy)
        {
            _actualEnergy += Time.deltaTime;
                        
        }
        else if (_actualEnergy == _maxEnergy)
        {
            ReturntoPatrol();

        }
        



    }

    private void ReturntoPatrol()
    {

        finiteStateMach.ChangeState(AgentStates.Patrol);

    }


}
