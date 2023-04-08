using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFactory
{
    private ObjectPool<Food> _foodPool;

    private int _foodPreWarm = 15;

    public FoodFactory(Food food)
    {
        _foodPool = new ObjectPool<Food>(food, _foodPreWarm, Factory, TurnOn, TurnOff);
    }

    public Food MakeFood(Vector3 foodPosition)
    {
        Food newFood = _foodPool.GetObjects();
        newFood.Initialize(foodPosition, ReturnBullet);
        return newFood;
    }

    Food Factory(Food prefab)
    {
        Food newFood = MonoBehaviour.Instantiate(prefab);
        return newFood;
    }

    public void ReturnBullet(Food foodToReturn)
    {
        _foodPool.ReturnObjects(foodToReturn);
    }

    void TurnOn(Food food) => food.gameObject.SetActive(true);

    void TurnOff(Food food)
    {
        food.ReturnFuit();
        food.gameObject.SetActive(false);
    }

}