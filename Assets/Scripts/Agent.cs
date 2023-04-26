using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{
    #region Movement Variables

    [HideInInspector]
    protected Vector3 _velocity;

    [SerializeField] [Range(1, 10)]
    protected float _maxForce;

    [SerializeField]
    public float _speed;
        
    #endregion
    
    #region All Radius variables

    [SerializeField]
    protected float _viewFenceRadius;

    [SerializeField]
    protected float _viewObstacleRadius;
    
    [FormerlySerializedAs("_viewRadius")] [SerializeField]
    protected float _generalViewRadius;
        
    #endregion

    #region Layer mask varibles needed

    [SerializeField]
    protected LayerMask _fenceMask;
    
    [SerializeField]
    protected LayerMask _obstacleMask;
    
    #endregion

    protected virtual void Start() => ApplyForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * _speed);
    

    protected virtual void Update()
    {
        //-------------------------------------------------Fence Avoidance
        FenceDetection();
       //-------------------------------------------------------------------
    }



    #region Particular type of movement Methods

    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;
    
        desired.Normalize();
    
        desired *= _speed;
    
        return CalculateStreering(desired);
    }
    
    
    public Vector3 Persuit(Transform target)
    {
        if (target == null) return Vector3.zero;
    
        Vector3 targetDist = target.position - transform.position;
        Vector3 desired = targetDist + target.forward;
        
        //En vez de pedir un TRansform pide un Agent. Cual es mjoer?
        //Vector3 futurePos = (target.transform.position + target._velocity);
        //Vector3 desired = futurePos - _myAgent.transform.position;
        
        return CalculateStreering(desired);
    }
    
    
    #endregion


    #region Obstacle Avoidance Methods

    protected void FenceDetection()
    {
        Collider[] fenceDetection = Physics.OverlapSphere(transform.position + transform.forward, _viewFenceRadius, _fenceMask);

        if (fenceDetection == null)
        {
            ApplyForce(CalculateStreering(_velocity.normalized * _speed));
        }

        foreach (var t in fenceDetection)
        {
            if (t != null)
            {
                float xNegative = transform.position.x * -1;
                float zNegative = transform.position.z * -1;

                ApplyForce(ChangeDirection(xNegative, 5, zNegative, 5));
                return;
            }
        }
    }

    
    protected Vector3 ChangeDirection(float x, float xRange,float z, float zRange)
    {
        Vector3 newDirection = new Vector3(Random.Range(x, xRange),0,Random.Range(z, zRange)).normalized * _speed;
        
        return CalculateStreering(newDirection);
    }

    private Vector3 ObstacleAvoidanceLogic(bool a)
    {
        if (a)
        {
            if (Physics.Raycast((transform.position + new Vector3(0, 1, 0)) + transform.right / 2, transform.forward, _viewObstacleRadius, _obstacleMask))
            {
                return  CalculateStreering(-transform.right * _speed);
            }
            else if (Physics.Raycast((transform.position + Vector3.up) - transform.right / 2, transform.forward, _viewObstacleRadius, _obstacleMask))
            {
                return  CalculateStreering(transform.right * _speed);
            }
        }
        else
        {                                                                       //Vector3.Up ne vez de crear un vector
            Collider[] b = Physics.OverlapSphere((transform.position + Vector3.up) + transform.forward,
                                    _viewObstacleRadius, _obstacleMask);

            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] != null)
                {
                    Debug.Log("Obstacle detected");

                    return CalculateStreering((transform.position + transform.right) * _speed);
                }
            }
                
        }
        
        
        
        return Vector3.zero;
    }

    protected bool ObstacleAvoidanceMovement(bool a)
    {
        Vector3 obstacle = ObstacleAvoidanceLogic(a);

        if (obstacle == Vector3.zero)
        {
            ApplyForce(CalculateStreering(_velocity.normalized * _speed));
            return false;
        }
        else
        {
            ApplyForce(obstacle);
            return true;
        }
    }
    
    #endregion


    #region General Movement Methods
    
    protected void Move()
    {
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }

    protected Vector3 CalculateStreering(Vector3 desired)
    {
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        return steering;
    }
    
    public void ApplyForce(Vector3 force)
    {
        force.y = 0; // Probar Deberia arrgelar el 4
        _velocity = Vector3.ClampMagnitude(_velocity + force, _speed);
    }
        
    #endregion
    
}