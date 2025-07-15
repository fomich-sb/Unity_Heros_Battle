using System.Collections;
using UnityEngine;

public class HeroSuperAttack2 : SuperAttackBase
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _animationDuration = 0.5f;

    public override bool Activate()
    {
        if(!base.Activate()) 
            return false;

        _animator.SetBool("active", true);
        StartCoroutine(DisableAnimation());
        return true;
    }

    private IEnumerator DisableAnimation()
    {
        yield return new WaitForSeconds(_animationDuration);
        _animator.SetBool("active", false);
    }
}
