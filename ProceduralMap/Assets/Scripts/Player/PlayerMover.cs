using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour
    {
        private Rigidbody2D _rigid;
        private WeaponParent _weaponParent;
        private AnimationController _animationController;
        private Status _status;
        private ParticleSystem _dust;

        private Vector2 _moveVec;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rollSpeed, rollCoolTime;
        [SerializeField] private float atkDelay;
        [SerializeField] private bool isMove, isRoll, isAttack;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _weaponParent = GetComponentInChildren<WeaponParent>();
            _animationController = GetComponentInChildren<AnimationController>();
            _status = GetComponent<Status>();
            _dust = GetComponentInChildren<ParticleSystem>();
        }

        public void Movement(Vector2 movement)
        {
            if (isRoll) return;
            isMove = _moveVec.sqrMagnitude > 0;
            _moveVec = movement;
            _animationController.MoveAnimation(isMove);
            _rigid.velocity = _moveVec * moveSpeed;
            CreateDust();
        }

        public void Roll()
        {
            if (isRoll || !isMove || isAttack || !_status.RollStamina()) return;
            isRoll = true;
            _animationController.RollAnimation();
            _rigid.velocity = _moveVec * rollSpeed;
            this.Wait(rollCoolTime, () => isRoll = false);
        }

        public void Attack()
        {
            if (isRoll || isAttack || !_status.AttackStamina()) return;
            isAttack = true;
            _weaponParent.Attack();
            this.Wait(atkDelay, () => isAttack = false);
        }


        public void Flip()
        {
            if (isRoll || isAttack) return;
            var thisTransform = transform;
            Vector2 scale = thisTransform.localScale;
            scale.x = _moveVec.x switch
            {
                < 0 => -1,
                > 0 => 1,
                _ => scale.x
            };
            thisTransform.localScale = scale;
        }

        public void Dead()
        {
            _rigid.velocity = Vector2.zero;
        }

        private void CreateDust()
        {
            if (_moveVec.sqrMagnitude == 0) return;
            _dust.Play();
        }
    }
}