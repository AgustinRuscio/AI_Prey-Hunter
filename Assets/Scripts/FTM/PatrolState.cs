using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : States
{
    private Transform[] _waypoints;

    private Transform _transform;

    private float _actualEnergy;

    private int _currentWaypoint;

    private float waypointsRadius = 3;

    private Vector3 _velocity;

    private Agent _agent;

    public PatrolState(Transform transform, Transform[] waypoints, float actualenergy, Vector3 velocity, Agent agent)
    {

        _transform = transform;
        _waypoints = waypoints;
        _actualEnergy = actualenergy;
        _velocity = velocity;
        _agent = agent;
    }


    public override void Update()
    {
        Debug.Log("Entre al patrol");

        _agent.ApplyForce(Waypoints() * _agent._speed);
        


    }


    private void PatrolEnergy()

    {
        _actualEnergy -=  Time.deltaTime;
      
        if (_actualEnergy == 0)
        {
            finiteStateMach.ChangeState(AgentStates.Rest);
            Debug.Log("cambie de estado a rest");
        }

    }


    private Vector3 Waypoints()
    {

        if (Vector3.Distance(_transform.position, _waypoints[_currentWaypoint].position) <= waypointsRadius)
        {
            _currentWaypoint++;


            if (_currentWaypoint >= _waypoints.Length)
            {
                _currentWaypoint = 0;
            }


        }


        //_tranform.position += (_waypoints[_currentWaypoint].position - _tranform.position).normalized * _agent._speed * Time.deltaTime;


        //_agent.ApplyForce((_waypoints[_currentWaypoint].position - _tranform.position).normalized * _agent._speed);

        return new Vector3(_waypoints[_currentWaypoint].position.x,0, _waypoints[_currentWaypoint].position.z);
    }




    private void ViewPrey()
    {
        //veo a la presa y cambio a chase state
        finiteStateMach.ChangeState(AgentStates.Chase);

    }

    public override void OnStart()
    {
        Debug.Log("Entre al patrol");
       

    }

    public override void OnStop()
    {
        
    }
}
