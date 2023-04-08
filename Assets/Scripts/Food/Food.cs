using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private Action<Food> _destroyMethod;


    private void Awake() => EventManager.Subscribe(EventEnum.ReturnFruit, ReturnFuit);

    public virtual void Initialize(Vector3 initPosition, Action<Food> destroyMethod)
    {
        transform.position = initPosition;
        _destroyMethod = destroyMethod;
    }

    public void ReturnFuit(params object[] parameters)
    {
        EventManager.Trigger(EventEnum.ChangePreyDirection);
        EventManager.Trigger(EventEnum.RemoveItemFromList, this);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Prey"))
            DestroyBullet();
    }

    void DestroyBullet()
    {
        _destroyMethod(this);
    }
    

    private void OnDestroy() => EventManager.Unsubscribe(EventEnum.ReturnFruit, ReturnFuit);
    
}
