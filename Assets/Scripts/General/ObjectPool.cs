using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T>
{
    private List<T> pool;

    private Action<T> TurnOnObejct;
    private Action<T> TurnOffObejct;

    private Func<T, T> PoolFactory;

    private T refObejct;

    public ObjectPool(T _refObejct, int preWarm, Func<T, T> _poolFactory, Action<T> _turnOn, Action<T> _turnOff)
    {
        TurnOnObejct = _turnOn;
        TurnOffObejct = _turnOff;

        PoolFactory = _poolFactory;

        refObejct = _refObejct;

        pool = new List<T>();

        for (int i = 0; i < preWarm; i++)
        {
            CreateNewObejcts();
        }
    }

    public T GetObjects()
    {
        T objToReturn = default(T);

        if (pool.Count <= 0)
            CreateNewObejcts();

        objToReturn = pool[0];
        pool.RemoveAt(0);

        TurnOnObejct(objToReturn);

        return objToReturn;
    }

    public void ReturnObjects(T returnedObject)
    {
        TurnOffObejct(returnedObject);

        pool.Add(returnedObject);
    }

    private void CreateNewObejcts()
    {
        T newObj = PoolFactory(refObejct);

        TurnOffObejct(newObj);
        pool.Add(newObj);
    }
}