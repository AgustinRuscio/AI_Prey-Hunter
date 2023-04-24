using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Agent
{
    private bool _alive = true;
    
    #region Radius Variables

        [SerializeField]
        private float _arriveRadius;

        //-------Flocking

        [SerializeField] 
        private float _separationRadius;
        
        #endregion
        
    #region Layer Mask Variables

        [SerializeField]
        private LayerMask _foodMask;

        [SerializeField]
        private LayerMask _hunterMask;
        
    #endregion

    #region Flocking Variables

        [SerializeField] [Range(0f, 3f)]
        private float _separationWeight;
        
        [SerializeField] [Range(0f, 3f)]
        private float _cohesionWeight;
        
        [SerializeField] [Range(0f, 3f)]
        private float _alignmentWeight;
        
    #endregion
    
    private void Awake()
    {
        EventManager.Subscribe(EventEnum.ChangePreyDirection, Redirection);
        EventManager.Subscribe(EventEnum.PreyDeath, OnDeath);
    }

    protected override void Start()
    {
        base.Start();
        FlokckingManager.instance.AddPrey(this);
        
    }

    protected override void Update()
    {
        if(!_alive) return;
        
        base.Update();
        Move();
        
        //-------------------------------------------------Flocking Movement
        ApplyForce(Alignment(FlokckingManager.instance.flockMates) * _alignmentWeight);
        
        ApplyForce(Cohesion(FlokckingManager.instance.flockMates) * _cohesionWeight);
        
        ApplyForce(Separation(FlokckingManager.instance.flockMates) * _separationWeight);

        //------------------------------------------------------------------
        
        //-------------------------------------------------Hunter Detection
        Collider[] hunters = Physics.OverlapSphere(transform.position, _generalViewRadius, _hunterMask);
        
        for (int i = 0; i < hunters.Length; i++)
        {
            if (hunters[i] != null)
            {
                Debug.Log("Corre");
                ApplyForce(-Persuit(hunters[i].transform));
                
            }
            return;
        }
        //----------------------------------------------------------------

        
        //-------------------------------------------------Obstacle Avoice
        ObstacleAvoidanceMovement(true);
        //----------------------------------------------------------------
        
        
        //-------------------------------------------------Food Detection
        Collider[] food = Physics.OverlapSphere(transform.position, _generalViewRadius, _foodMask);
        
        if(food == null)
            ApplyForce(CalculateStreering(_velocity.normalized * _speed));
            
        for (int i = 0; i < food.Length; i++)
        { 
            if (food[i] != null)
                    ApplyForce(Arrive(food[i].transform.position));
        }
        //---------------------------------------------------------------

    }

    #region Movement Methods

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

        
        private void Redirection(params object[] parameters)
        {
            ApplyForce(ChangeDirection(-1,1 ,-1,1)); 
        }
    

    #endregion

    #region Flocking Movement Methods

        private Vector3 Alignment(HashSet<Prey> preys)
        {
            Vector3 desired = default;
            
            foreach (var flockMate in preys)
            {
                if(Vector3.Distance(flockMate.transform.position, transform.position) <= _generalViewRadius)
                    desired += flockMate._velocity;
            }

            desired /= preys.Count;

            desired.Normalize();
            desired *= _speed;
            
            return CalculateStreering(desired);
        }


        private Vector3 Cohesion(HashSet<Prey> preys)
        {
            Vector3 desired = default;

            int localFlockMatesCount = 0;
            
            foreach (var flockMate in preys)
            {
                if (flockMate == this) continue;
                
                if (Vector3.Distance(flockMate.transform.position, transform.position) <= _generalViewRadius)
                {
                    localFlockMatesCount++;
                    desired += flockMate.transform.position;
                }
            }

            if(localFlockMatesCount == 0) return Vector3.zero;
            
            desired /= localFlockMatesCount;

            return Seek(desired);
        }
        
        private Vector3 Separation(HashSet<Prey> preys)
        {
            Vector3 desired = default;

            foreach (var flockMate in preys)
            {
                Vector3 dist = flockMate.transform.position - transform.position;
                
                if (dist.magnitude <= _separationRadius)
                {
                    desired += dist;
                }
            }

            if (desired == Vector3.zero) return desired;
            
            
            desired *= -1;

            desired.Normalize();
            desired *= _speed;
            

            return CalculateStreering(desired);
        }
        

    #endregion

    private void OnDeath(params object[] parameters)
    {
        _alive = false;
        _velocity = Vector3.zero;
    }
    
    private void PreyDeath()
    {
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventEnum.ChangePreyDirection, Redirection);
        EventManager.Unsubscribe(EventEnum.PreyDeath, OnDeath);
    }

    private void OnDrawGizmos()
    {
        //----------------View Radius
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, _generalViewRadius);

        //----------------Arrive radius
        
        Gizmos.color = Color.green;
        
        Gizmos.DrawWireSphere(transform.position, _arriveRadius);
        
        //----------------Obstacle Avoidance
        Gizmos.color =Color.magenta;

        Vector3 orpos = (transform.position + new Vector3(0,1,0)) + transform.right/2;
        
        Gizmos.DrawLine(orpos, orpos+transform.forward * _viewObstacleRadius);
        
        Vector3 o2rpos = (transform.position + new Vector3(0,1,0)) - transform.right/2;
        
        Gizmos.DrawLine(o2rpos, o2rpos+transform.forward * _viewObstacleRadius);
        
        //----------------Fence radius
        Gizmos.color = Color.white;
        
        Gizmos.DrawWireSphere(transform.position + transform.forward, _viewFenceRadius);

        //----------------Separation radius
        Gizmos.color = Color.grey;
        
        Gizmos.DrawWireSphere(transform.position, _separationRadius);
    }
}
