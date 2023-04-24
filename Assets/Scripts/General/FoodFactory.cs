using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFactory
{
    private ObjectPool<Food> _foodPool;

    private int _foodPreWarm = 15;

    
    public FoodFactory(Food food) => _foodPool = new ObjectPool<Food>(food, _foodPreWarm, Factory, TurnOn, TurnOff);
    
    public Food MakeFood(Vector3 foodPosition)
    {
        Food newFood = _foodPool.GetObjects();
        newFood.Initialize(foodPosition, ReturnBullet);
        return newFood;
    }

    private Food Factory(Food prefab)
    {
        Food newFood = MonoBehaviour.Instantiate(prefab);
        return newFood;
    }

    private void ReturnBullet(Food foodToReturn) => _foodPool.ReturnObjects(foodToReturn);

    private void TurnOn(Food food) => food.gameObject.SetActive(true);

    private void TurnOff(Food food)
    {
        food.ReturnFuit();
        food.gameObject.SetActive(false);
    }

}