using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private void Awake()
    {
        EventManager.Subscribe(EventEnum.HuntingAnims, SetEscapeAnim);
        EventManager.Subscribe(EventEnum.PreyDeath, SetDeathAnim);
    }
    
    private void SetEscapeAnim(params object[] isEscaping) => _animator.SetBool("Escape", (bool)isEscaping[0]);
    private void SetDeathAnim(params object[] Death) => _animator.SetTrigger("Death");
    
    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventEnum.HuntingAnims, SetEscapeAnim);
        EventManager.Unsubscribe(EventEnum.PreyDeath, SetDeathAnim);
    }
}
