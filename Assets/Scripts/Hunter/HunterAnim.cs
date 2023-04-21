using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAnim : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        EventManager.Subscribe(EventEnum.HuntingAnims, SetRunAnim);
        EventManager.Subscribe(EventEnum.PreyDeath, SetShootAnim);
        EventManager.Subscribe(EventEnum.HunterRest, SetRestAnim);
    }

    void Update()
    {
        
    }

    private void SetRunAnim(params object[] isRunning) => _animator.SetBool("Run", (bool)isRunning[0]);
    private void SetRestAnim(params object[] isRunning) => _animator.SetBool("Resting", (bool)isRunning[0]);
    private void SetShootAnim(params object[] isRunning) => _animator.SetTrigger("Shoot");
    
    
    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventEnum.HuntingAnims, SetRunAnim);
        EventManager.Unsubscribe(EventEnum.PreyDeath, SetShootAnim);
        EventManager.Unsubscribe(EventEnum.HunterRest, SetRestAnim);
    }
}
