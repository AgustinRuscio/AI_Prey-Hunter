using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IA2;

public class Hunter : Agent
{
    //private FiniteStateMachine _finiteStateMach;

    #region Rest Status Variables

       
    public float _actualEnergy;

    
    public float _maxEnergy;

    [SerializeField]
    private float _speedRecovery;

    #endregion

    #region Patrol State Variables

    [SerializeField]
    private Transform[] _waypoints;

    [SerializeField] 
    private float _waypointDetectionRadius;

    [SerializeField] 
    private LayerMask _preyLayerMask;
    
    #endregion
    
    #region Chase State Variables
    
    [SerializeField] 
    private float _KillRadius;

    
    public Prey _target;

    private bool _shooting;

    #endregion

    private bool _resting;

    #region New State machine variables

    public enum PlayerInputs { Patrol, Chase, Rest }
    private EventFSM<PlayerInputs> _myFsm;

    #endregion

    [SerializeField] 
    private Queries _query;
    
    private void Awake() => EventManager.Subscribe(EventEnum.HunterRest, CheckRestState);
    

    protected override void Start()
    {
        #region  OLD STATE MACHINE

        //_finiteStateMach = new FiniteStateMachine();
        //
        //_finiteStateMach.AddState(AgentStates.Patrol, new PatrolState(this).SetLayerMask(_preyLayerMask).SetWayPoints(_waypoints)
        //                                        .SetPatrolAgentTransform(transform).SetPreyViewRadius(_generalViewRadius).SetWaypointsViewRadius(_waypointDetectionRadius));
        //
        //_finiteStateMach.AddState(AgentStates.Chase, new ChaseState(this, _KillRadius));
        //_finiteStateMach.AddState(AgentStates.Rest, new RestState(this));
        
        #endregion


        #region NEW STATE MACHINE

        //Create states
        var patrol = new State<PlayerInputs>("Patrol");
        var chase = new State<PlayerInputs>("Chase");
        var rest = new State<PlayerInputs>("Rest");

        
        //transitions
        StateConfigurer.Create(patrol)
            .SetTransition(PlayerInputs.Chase, chase)
            .SetTransition(PlayerInputs.Rest, rest)
            .Done();
        
        StateConfigurer.Create(rest)
            .SetTransition(PlayerInputs.Patrol, patrol)
            .Done();

        
        StateConfigurer.Create(chase)
            .SetTransition(PlayerInputs.Patrol, patrol)
            .SetTransition(PlayerInputs.Rest, rest)
            .Done();

        //Patrol
        //State Methods
        patrol.OnEnter += inputs =>
        {
            ApplyForce(Seek(Waypoints()));
        };

        patrol.OnUpdate += () =>
        {
            PatrolEnergy();
            ViewPrey();
            
            ApplyForce(Seek(Waypoints()));
        };
        
        //Chase
        //State methods

        chase.OnEnter += inputs =>
        {
            _timer = new GenericTimer(_coolDown);
            
            ApplyForce(Persuit(_target.transform) * _speed);
            
            EventManager.Trigger(EventEnum.HuntingAnims, true);
        };


        chase.OnUpdate += () =>
        {
            _timer.RunTimer();
            ReduceEnergy();
            
            if (_actualEnergy <= 0)
            {
                EventManager.Trigger(EventEnum.HuntingAnims, false);
                _myFsm.SendInput(PlayerInputs.Rest);
                return;
            }
            if (_target == null) 
            {
                EventManager.Trigger(EventEnum.HuntingAnims, false);

                _myFsm.SendInput(PlayerInputs.Patrol);
                return;
            }
            
            if(_target != null)
            {
                if (Vector3.Distance(transform.position, _target.transform.position) <= _KillRadius && _timer.CheckCoolDown())
                {
                    _timer.ResetTimer();
                    _target.OnDeath();

                    EventManager.Trigger(EventEnum.PreyDeath, true);
                    EventManager.Trigger(EventEnum.HuntingAnims, false);

                    OnShoot(transform);
                    _myFsm.SendInput(PlayerInputs.Patrol);
                    return;
                }
                else
                {
                    ApplyForce(Persuit(_target.transform) * _speed);
                    _target.OnPersuit(true);
                }
            }
        };

        chase.OnExit += inputs =>
        {
            EventManager.Trigger(EventEnum.PreyDeath, false);
        };
        
        //Rest
        //State methods

        rest.OnEnter += inputs =>
        {
            ApplyForce(Vector3.zero);
            EventManager.Trigger(EventEnum.HunterRest, true);
        };

        rest.OnUpdate += () =>
        {
            if (_actualEnergy !< _maxEnergy)
                RecoverEnergy();
            else
            {
                _myFsm.SendInput(PlayerInputs.Patrol);
                return;
            }
        };

        rest.OnExit += inputs =>
        {
            EventManager.Trigger(EventEnum.HunterRest, false);
        };
        
        
        _myFsm = new EventFSM<PlayerInputs>(patrol);
        
        #endregion
    }

    #region New patrol Methods

    private int _currentWaypoint = 0;
    private Vector3 Waypoints()
    {
        if (Vector3.Distance(transform.position, _waypoints[_currentWaypoint].position) <= _waypointDetectionRadius)
        {
            _currentWaypoint++;
            
            if (_currentWaypoint >= _waypoints.Length)
                _currentWaypoint = 0;
        }

        return _waypoints[_currentWaypoint].position;
    }

    private void PatrolEnergy()
    {
        ReduceEnergy();

        if (_actualEnergy <= 0)
        {
            _myFsm.SendInput(PlayerInputs.Rest);
            return;
        }
    }
    
    private void ViewPrey()
    {
        #region Con OverlapSphere

            //Collider[] preyDetector = Physics.OverlapSphere(transform.position, _generalViewRadius, _preyLayerMask);

            //for (int i = 0; i < preyDetector.Length; i++)
            //{
            //    if (preyDetector[i] != null)
            //    { 
            //        GetTarget(preyDetector[i].GetComponent<Prey>());
            //        _myFsm.SendInput(PlayerInputs.Chase);
            //        return;
            //    }
            //}
        
        #endregion

        #region Con Grid

        if (_query.selected.Any())
        {
            var check = _query.selected.Aggregate(Tuple.Create<float, Prey>(float.MaxValue,null),(currentClosest, currentOnCheck) =>
            {
                float currentDistance = Vector3.Distance(currentOnCheck.transform.position, transform.position);
          
                if (currentDistance < currentClosest.Item1)
                    currentClosest = Tuple.Create(currentDistance, currentOnCheck.GetComponent<Prey>());
                
                return currentClosest;
            });
            
            GetTarget(check.Item2);
            _myFsm.SendInput(PlayerInputs.Chase);
            return;
        }

        #endregion
    }
    
    #endregion

    #region New Chase Methods

    private GenericTimer _timer;
    
    private float _coolDown = 2f;

    #endregion

    protected override void Update()
    {
        if (_shooting) return;

        //_finiteStateMach.Update();
        
        _myFsm.Update();
        
        if(_resting) return;
        
        base.Update();
        
        Move();

        if (ObstacleAvoidanceMovement())
            ObstacleAvoidanceMovement();
    }

    public Prey GetTarget(Prey target)
    {
        _target = target;

        return _target;
    }

    #region Chase Methods

    public void OnShoot(Transform targetpos)
    {
        _velocity = Vector3.zero;
        transform.LookAt(targetpos);
        _shooting = true;
    }
    public void DisableShootState() => _shooting = false;

    #endregion

    #region Rest State Methods

    public void ReduceEnergy() => _actualEnergy -= Time.deltaTime;
    public void RecoverEnergy() => _actualEnergy += Time.deltaTime * _speedRecovery;
    private void CheckRestState(params object[] isResting) => _resting = (bool)isResting[0];

    #endregion

    private void OnDestroy() => EventManager.Unsubscribe(EventEnum.HunterRest, CheckRestState);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _generalViewRadius);

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, _KillRadius);
        
        
        //ObstaclesTectection
        Gizmos.color =Color.magenta;

        Vector3 orpos = (transform.position + Vector3.up) + transform.right/2;
        
        Gizmos.DrawLine(orpos, orpos+transform.forward * _viewObstacleRadius);
        
        Vector3 o2rpos = (transform.position + Vector3.up) - transform.right/2;
        
        Gizmos.DrawLine(o2rpos, o2rpos+transform.forward * _viewObstacleRadius);
        
        //Second OPtion
        Gizmos.color =Color.magenta;
        //Gizmos.DrawWireSphere(transform.position + new Vector3(0,1,0) + transform.forward, _viewObstacleRadius);
        
        //WaypointsDetecttion
        Gizmos.color =Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _waypointDetectionRadius);

        //Fence radius
        Gizmos.color =Color.white;
        Gizmos.DrawWireSphere(transform.position + transform.forward, _viewFenceRadius);
    }
}