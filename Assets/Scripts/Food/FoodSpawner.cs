using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private Food _foodPrefab;

    private FoodFactory _foodFactory;

    private GenericTimer _timer;

    [SerializeField]
    private float _coolDown;

    private List<Food> _foodList = new List<Food>();

    private void Awake()
    {
        _timer = new GenericTimer(_coolDown);
        _foodFactory = new FoodFactory(_foodPrefab);
        
        EventManager.Subscribe(EventEnum.RemoveItemFromList, RemoveFoodFormList);
    }

    void Update()
    {
        _timer.RunTimer();

        if (_timer.CheckCoolDown() && _foodList.Count < 10)
        {
            Food newFood =  _foodFactory.MakeFood(GenerateRandomPosition());
            _foodList.Add(newFood);

            _timer.ResetTimer();
        }
    }

    private void RemoveFoodFormList(params object[] parameters)
    {
        Food food = (Food)parameters[0];

        if(_foodList.Contains(food))
            _foodList.Remove(food);
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 newPosition = new Vector3(Random.Range(-12,12), 0.5f, Random.Range(-14,14));

        return newPosition;
    }

    private void OnDestroy() => EventManager.Unsubscribe(EventEnum.RemoveItemFromList, RemoveFoodFormList);
    
}