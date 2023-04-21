using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{
    [HideInInspector]
    protected Vector3 _velocity;

    [SerializeField] [Range(1, 10)]
    protected float _maxForce;

    [SerializeField]
    public float _speed;

    [SerializeField]
    protected float _viewRadius;

    [SerializeField]
    protected LayerMask _obstacleMask;
    

    protected virtual void Start()
    {
        ApplyForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * _speed);
    }

    protected virtual void Update()
    {
        Move();
        
       Collider[] a = Physics.OverlapSphere(transform.position, _viewRadius, _obstacleMask);

       //if (a == null)
       //{
       //    ApplyForce(CalculateStreering(_velocity.normalized * _speed));
      // }
       
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != null)
            {
                float xNegative = transform.position.x * -1;
                float zNegative = transform.position.z * -1;

                ApplyForce(ChangeDirection(xNegative, 5 ,zNegative, 5));
            }
        }

       

    }
    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;

        desired.Normalize();

        desired *= _speed;

        return CalculateStreering(desired);
    }

    public void Move()
    {
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
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
    
    public Vector3 CalculateStreering(Vector3 desired)
    {
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        return steering;
    }

    protected Vector3 ChangeDirection(float x, float xRange,float z, float zRange)
    {
        Vector3 newDirection = new Vector3(Random.Range(x, xRange),0,Random.Range(z, zRange)).normalized * _speed;

        return CalculateStreering(newDirection);
    }

    protected Vector3 ObstacleAvoidanceLogic(bool a)
    {
        if (a)
        {
            if (Physics.Raycast((transform.position + new Vector3(0, 1, 0)) + transform.right / 2, transform.forward,
                    _viewRadius, _obstacleMask))
            {
                Debug.Log("if");
                return  CalculateStreering((transform.position - transform.right) * _speed);
            }
            else if (Physics.Raycast((transform.position + new Vector3(0, 1, 0)) - transform.right / 2, transform.forward,
                         _viewRadius, _obstacleMask))
            {
                Debug.Log("else");
                return  CalculateStreering((transform.position + transform.right) * _speed);
            }
        }
        else
        {
            Collider[] b = Physics.OverlapSphere((transform.position + new Vector3(0,1,0)) + transform.forward, _viewRadius, _obstacleMask);

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
            //Debug.Log("avoidancec");
            ApplyForce(obstacle);
            return true;
        }
    }
    
    
    public void ApplyForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _speed);
    }

    public void AutoDestruction()
    {
        Destroy(gameObject);
    }
    
}