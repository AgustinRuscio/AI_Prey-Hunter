using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Agent
{
    [SerializeField]
    private float _arriveRadius;

    [SerializeField]
    private LayerMask _foodMask;

    [SerializeField]
    private LayerMask _hunterMask;


    private void Awake()
    {
        EventManager.Subscribe(EventEnum.ChangePreyDirection, Redirection);
    }
    
    protected override void Update()
    {
        //base.Update();
        Move();
        
        Collider[] hunters = Physics.OverlapSphere(transform.position, _viewRadius, _hunterMask);
        
        for (int i = 0; i < hunters.Length; i++)
        {
            if (hunters[i] != null)
            {
                Debug.Log("Corre");
                ApplyForce(-Persuit(hunters[i].transform));
                
            }
            return;
        }

        Collider[] food = Physics.OverlapSphere(transform.position, _viewRadius, _foodMask);

            
        if(food == null)
            ApplyForce(CalculateStreering(_velocity.normalized * _speed));
            
        for (int i = 0; i < food.Length; i++)
        { 
            if (food[i] != null)
                    ApplyForce(Arrive(food[i].transform.position));
        }
       
        


        /*new 

        Collider[] a = Physics.OverlapSphere(transform.position, _viewRadius, _obstacleMask);

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != null)
            {
                float xNegative = transform.position.x * -1;
                float zNegative = transform.position.z * -1;

                ApplyForce(ChangeDirection(xNegative, 5, zNegative, 5));
            }
        }

        Debug.Log("soy velocity de prey " + _velocity);*/

    }

    private Vector3 Arrive(Vector3 arriveTarget)
    {
        float dist = Vector3.Distance(transform.position, arriveTarget);

        if (dist > _arriveRadius)
            return Seek(arriveTarget);

        Vector3 desired = arriveTarget - transform.position;

        desired.Normalize();

        desired *= _speed * (dist / _arriveRadius);

        return CalculateStreering(desired);
    }

    
    public void Redirection(params object[] parameters)
    {
        ApplyForce(ChangeDirection(-1,1 ,-1,1)); 
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventEnum.ChangePreyDirection, Redirection);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, _arriveRadius);
        
        Gizmos.color =Color.magenta;

        Vector3 orpos = (transform.position + new Vector3(0,1,0)) + transform.right/2;
        
        Gizmos.DrawLine(orpos, orpos+transform.forward * _viewRadius);
        
        Vector3 o2rpos = (transform.position + new Vector3(0,1,0)) - transform.right/2;
        
        Gizmos.DrawLine(o2rpos, o2rpos+transform.forward * _viewRadius);
        
    }
}
