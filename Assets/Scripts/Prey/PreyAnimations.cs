using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    
    public void SetEscapeAnim(params object[] isEscaping) => _animator.SetBool("Escape", (bool)isEscaping[0]);
    public void SetDeathAnim(params object[] death) => _animator.SetTrigger("Death");
    
}