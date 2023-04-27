using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    private Action<Food> _destroyMethod;

    private void Awake() => EventManager.Subscribe(EventEnum.ReturnFruit, ReturnFruit);

    public virtual void Initialize(Vector3 initPosition, Action<Food> destroyMethod)
    {
        transform.position = initPosition;
        _destroyMethod = destroyMethod;
    }

    public void ReturnFruit(params object[] parameters)
    {
        EventManager.Trigger(EventEnum.ChangePreyDirection);
        EventManager.Trigger(EventEnum.RemoveItemFromList, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Prey"))
            DestroyFood();
    }

    private void DestroyFood()
    {
        _destroyMethod(this);
    }

    private void OnDestroy() => EventManager.Unsubscribe(EventEnum.ReturnFruit, ReturnFruit);
}