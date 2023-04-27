using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterCamera : MonoBehaviour
{
    public Transform player;

    [SerializeField]
    private Vector3 maxDistance;
    
    void Update() => transform.position = player.position + maxDistance;
    
}