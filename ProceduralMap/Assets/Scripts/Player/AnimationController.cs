using System;
using UnityEngine;

namespace Player
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _anim;
        private static readonly int IsMove = Animator.StringToHash("isMove");
        private static readonly int IsDeath = Animator.StringToHash("isDeath");
        private static readonly int IsHit = Animator.StringToHash("isHit");
        private static readonly int IsRoll = Animator.StringToHash("isRoll");
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        
        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void MoveAnimation(bool isMove)
        {
            _anim.SetBool(IsMove,isMove);
        }

        public void RollAnimation()
        {
            _anim.SetTrigger(IsRoll);
        }

        public void HitAnimation()
        {
            _anim.SetTrigger(IsHit);
        }

        public void DeathAnimation()
        {
            _anim.SetTrigger(IsDeath);
        }

        public void AttackAnimation()
        {
            _anim.SetTrigger(IsAttack);
        }
    }
}