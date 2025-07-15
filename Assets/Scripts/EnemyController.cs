using System.Collections.Generic;
using UnityEngine;

public class EnemyController : HeroControllerBase, IHeroAttack
{
    private void Start()
    {
        _animationInputs = new Dictionary<int, InputFlags>
        {
            { 1,  new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 2,  new InputFlags { MoveForward = true , MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 3,  new InputFlags { MoveForward = false, MoveBack = true , Jump = false, SeatDown = false, Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 4,  new InputFlags { MoveForward = false, MoveBack = false, Jump = true , SeatDown = false, Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 5,  new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = true , Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 6,  new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = true , Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 7,  new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
            { 8,  new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
            { 9,  new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
    //        { 10, new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
            { 11, new InputFlags { MoveForward = false, MoveBack = false, Jump = true , SeatDown = false, Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
            { 12, new InputFlags { MoveForward = false, MoveBack = false, Jump = true , SeatDown = false, Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
        //    { 13, new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 14, new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = true , Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
            { 15, new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = true , Block = true , Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 16, new InputFlags { MoveForward = true , MoveBack = false, Jump = false, SeatDown = false, Block = true , Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 17, new InputFlags { MoveForward = false, MoveBack = true , Jump = false, SeatDown = false, Block = true , Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 18, new InputFlags { MoveForward = true , MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
            { 19, new InputFlags { MoveForward = true , MoveBack = false, Jump = true , SeatDown = false, Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 20, new InputFlags { MoveForward = false, MoveBack = true , Jump = true , SeatDown = false, Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = false } },
            { 21, new InputFlags { MoveForward = true , MoveBack = false, Jump = true , SeatDown = false, Block = false, Attack = true , SuperAttack1 = false, SuperAttack2 = false } },
            { 22, new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = false, SuperAttack1 = false, SuperAttack2 = true  } },
            { 23, new InputFlags { MoveForward = false, MoveBack = false, Jump = false, SeatDown = false, Block = false, Attack = false, SuperAttack1 = true , SuperAttack2 = false } },
        };
    }

    public float GetAttackDamageValue()
    {
        if (IsDead || _damageTakedOnCurrentStep)
            return 0;

        _damageTakedOnCurrentStep = true;

        if (!_gameController.HeroNotDead)
        {
            if (_animationInputs[_currentAnimationId].SuperAttack1)
                return _superAttack1Damage * (0.6f - _health.Value / 2 + _enemyHealth.Value);
            if (_animationInputs[_currentAnimationId].SuperAttack2)
                return _superAttack2Damage * (0.6f - _health.Value / 2 + _enemyHealth.Value);
            if (_animationInputs[_currentAnimationId].Attack)
                return _attackDamage * (0.6f - _health.Value / 2 + _enemyHealth.Value);
        }
        else
        {
            float damage = 0;
            if (_animationInputs[_currentAnimationId].SuperAttack1)
                damage = _superAttack1Damage * (0.4f - _health.Value / 2 + _enemyHealth.Value);
            if (_animationInputs[_currentAnimationId].SuperAttack2)
                damage = _superAttack2Damage * (0.4f - _health.Value / 2 + _enemyHealth.Value);
            if (_animationInputs[_currentAnimationId].Attack)
                damage = _attackDamage * (0.4f - _health.Value / 2 + _enemyHealth.Value);
            damage = Mathf.Max(0, Mathf.Min(damage, _enemyHealth.Value-0.02f));
            return damage;
        }

        return 0;
    }
}

