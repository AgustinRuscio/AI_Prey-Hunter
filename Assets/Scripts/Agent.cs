using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    protected Vector3 _velocity;

    [SerializeField]
    [Range(1, 10)]
    protected float _maxForce;

    [SerializeField]
    protected float _speed;

    [SerializeField]
    protected float _viewRadius;

    [SerializeField]
    private LayerMask _obstacleMask;


    private void Start()
    {
        ApplyForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * _speed);
    }

    protected virtual void Update()
    {
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;

       Collider[] a = Physics.OverlapSphere(transform.position, _viewRadius, _obstacleMask);

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

    protected Vector3 CalculateStreering(Vector3 desired)
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


    protected void ApplyForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _speed);
    }
}
