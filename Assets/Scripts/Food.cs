using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void Awake() => EventManager.Subscribe(EventEnum.ReturnFruit, ReturnFuit);
    

    private void ReturnFuit(params object[] parameters)
    {
        gameObject.SetActive(false); 
        EventManager.Trigger(EventEnum.ChangePreyDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Prey"))
            ReturnFuit();
    }

    private void OnDestroy() => EventManager.Unsubscribe(EventEnum.ReturnFruit, ReturnFuit);
    
}
