using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : States
{
    #region Needed Variables

    private Prey _chaseTarget; 
    
    private Hunter _myAgent;

    private float _killRadius;

    #endregion

    #region Local variables

    private float _coolDown = 2f;

    private GenericTimer _timer;

    #endregion

    public ChaseState(Hunter agent, float killRadius)
    {
        _myAgent = agent;
        _killRadius = killRadius;
    }

    public override void OnStart(params object[] parameters)
    {
        _timer = new GenericTimer(_coolDown);

        Debug.Log("chase");

        _chaseTarget = _myAgent._target;

        _myAgent.ApplyForce(_myAgent.Persuit(_chaseTarget.transform) * _myAgent._speed);
        
        EventManager.Trigger(EventEnum.HuntingAnims, true);
    }

    public override void OnStop() => EventManager.Trigger(EventEnum.PreyDeath, false);
    

    public override void Update()
    {
        _timer.RunTimer();
        _myAgent.ReduceEnergy();

        if (_myAgent._actualEnergy <= 0)
        {
            Debug.Log("Cambie a Rest ");
            EventManager.Trigger(EventEnum.HuntingAnims, false);
            finiteStateMach.ChangeState(AgentStates.Rest);
        }

        if (_chaseTarget == null) 
        {
            EventManager.Trigger(EventEnum.HuntingAnims, false);

            finiteStateMach.ChangeState(AgentStates.Patrol);
        }

        if(_chaseTarget != null)
        {
            if (Vector3.Distance(_myAgent.transform.position, _chaseTarget.transform.position) >= _killRadius || !_timer.CheckCoolDown())
            {
                _myAgent.ApplyForce(_myAgent.Persuit(_chaseTarget.transform) * _myAgent._speed);
                _chaseTarget.OnPersuit(true);
            }
            else if(Vector3.Distance(_myAgent.transform.position, _chaseTarget.transform.position) <= _killRadius && _timer.CheckCoolDown())
            {
                _timer.ResetTimer();

                _chaseTarget.OnDeath();

                EventManager.Trigger(EventEnum.PreyDeath, true);
                EventManager.Trigger(EventEnum.HuntingAnims, false);
                
                _myAgent.OnShoot(_chaseTarget.transform);
                finiteStateMach.ChangeState(AgentStates.Patrol);
            }
        }
    }
    
}