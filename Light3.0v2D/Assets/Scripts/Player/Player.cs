// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Player : MonoBehaviour
// {
//     public static Player instance;

//     [SerializeField] AttackType attackType;
//     //Component
//     Animator anim;
//     Rigidbody2D rigid;
//     SpriteRenderer spriteRenderer;
//     //Flash
//     private Material normalMaterial;
//     [SerializeField] Material flashMaterial;
//     [SerializeField] float flashDuration;
//     private Coroutine flashRoutine;

//     [SerializeField] Transform aimTransform;
//     [SerializeField] GameObject bulletHole;

//     [SerializeField] StatusController statusController;
//     [SerializeField] AudioSource pistolAudio;

//     public Vector2 mousePos;
//     Vector2 move, aimVec;
//     float rotZ;
//     [SerializeField] bool isDodge, isAttack, isDamage, isMove;
//     public bool isDead, gameStart;
//     //----------------------------Range------------------------------
//     //Move
//     [SerializeField][Range(0, 10)] int moveSpeed, dodgeSpeed;
//     [SerializeField] float dodgeAmount;
//     [SerializeField][Range(0, 0.5f)] float i_frame;  //dodge 
//     [SerializeField][Range(0, 1f)] float hitCoolTime;
//     private float noHitTime;
//     //Camera Shake
//     [SerializeField][Range(0, 1f)] float shakeLength, shakePower;
//     //Melee
//     [SerializeField][Range(0, 1f)] float meleeCoolTime;
//     //Pistol
//     [SerializeField][Range(0, 1f)] float pistolCoolTime;
//     //ShotGun
//     // [SerializeField] float shotGunCoolTime;
//     // [SerializeField] float shotGunSpreadRange;
//     // [SerializeField] int shotGunBulletAmount;

//     [SerializeField][Range(0, 30)] int bulletSpeed;
//     private float atkDelay;

//     //Animation
//     private readonly int IsMove = Animator.StringToHash("isMove");
//     private readonly int IsDodge = Animator.StringToHash("isDodge");
//     private readonly int IsAttack = Animator.StringToHash("isAttack");
//     private readonly int IsDead = Animator.StringToHash("isDead");

//     private void Awake()
//     {
//         instance = this;
//         //health = maxHealth;
//         anim = GetComponentInChildren<Animator>();
//         rigid = GetComponent<Rigidbody2D>();
//         spriteRenderer = GetComponentInChildren<SpriteRenderer>();
//         normalMaterial = spriteRenderer.material;
//     }
//     private void Update()
//     {
//         if (isDead || !gameStart) return;

//         mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         aimVec = mousePos - (Vector2)transform.position;
//         rotZ = Mathf.Atan2(aimVec.y, aimVec.x) * Mathf.Rad2Deg;
//         aimTransform.rotation = Quaternion.Euler(0, 0, rotZ);

//         Move();
//         Dodge();
//         Attack();
//         OnDamage();
//     }
//     private void Animate()
//     {
//         anim.SetFloat("MoveX", move.x);
//         anim.SetFloat("MoveY", move.y);
//     }
//     private void Move()
//     {
//         if (isDodge || isAttack) return;

//         float h = Input.GetAxis("Horizontal");
//         float v = Input.GetAxis("Vertical");

//         move = new Vector2(h, v).normalized;
//         //if (move.sqrMagnitude != 0) isMove = true;
//         // else isMove = false;
//         // anim.SetBool(IsMove, isMove);

//         rigid.velocity = move * moveSpeed;

//         if (h < 0) spriteRenderer.flipX = true;
//         else if (h > 0) spriteRenderer.flipX = false;

//         if (mousePos.x < transform.position.x) aimTransform.transform.localScale = new Vector2(0.5f, -0.5f); //left
//         else if (mousePos.x > transform.position.x) aimTransform.transform.localScale = new Vector2(0.5f, 0.5f); //right
//     }
//     private void Dodge()
//     {
//         if (Input.GetKeyDown(KeyCode.Space) && move.sqrMagnitude != 0 && !isDodge && statusController.curStamina > dodgeAmount)
//         {
//             isDodge = true;
//             statusController.DecreaseStamina(dodgeAmount);
//             rigid.velocity = move * dodgeSpeed;
//             this.Wait(i_frame, () => isDodge = false);
//         }
//     }
//     private void Attack()
//     {
//         if (isDodge) return;

//         if (atkDelay <= 0 && Input.GetMouseButton(0) && !isAttack)
//         {
//             isAttack = true;

//             if (mousePos.x < transform.position.x) spriteRenderer.flipX = true;
//             if (mousePos.x > transform.position.x) spriteRenderer.flipX = false;

//             switch (attackType)
//             {
//                 case AttackType.Melee:
//                     MeleeAttack();
//                     atkDelay = meleeCoolTime;
//                     break;
//                 case AttackType.Pistol:
//                     PistolAttack();
//                     pistolAudio.Play();
//                     atkDelay = pistolCoolTime;
//                     break;
//                 case AttackType.Big:
//                     ShotGunAttack();
//                     //atkDelay = shotGunCoolTime;
//                     break;
//             }
//         }
//         else
//         {
//             atkDelay -= Time.deltaTime;
//             isAttack = false;
//         }
//     }
//     private void MeleeAttack()
//     {
//         CameraController.instance.StartShake(shakeLength, shakePower);
//         anim.SetTrigger(IsAttack);
//     }
//     private void PistolAttack()
//     {
//         CameraController.instance.StartShake(shakeLength, shakePower);
//         ObjectPooler.SpawnFromPool<PlayerBulletController>("PlayerBullet", bulletHole.transform.position, Quaternion.Euler(0, 0, rotZ))
//         .rigid.velocity = aimVec.normalized * bulletSpeed;
//     }
//     private void ShotGunAttack()
//     {
//         CameraController.instance.StartShake(shakeLength, shakePower);
//         ObjectPooler.SpawnFromPool<PlayerBulletController>("PlayerBigBullet", bulletHole.transform.position, Quaternion.Euler(0, 0, rotZ))
//         .rigid.velocity = aimVec.normalized * bulletSpeed;

//         // Debug.DrawRay(transform.position, (gun.transform.right * bulletRange + gun.transform.up), Color.green);
//         // Debug.DrawRay(transform.position, gun.transform.right * bulletRange, Color.green);
//         // Debug.DrawRay(transform.position, (gun.transform.right * bulletRange - gun.transform.up), Color.green);
//     }
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("EnemyBullet") && !isDamage && !isDodge)
//         {
//             isDamage = true;
//             CameraController.instance.StartShake(shakeLength, shakePower);
//             var enemyAtk = other.GetComponent<EnemyAttackController>();
//             statusController.curHealth -= enemyAtk.damage;
//             OnDamage();
//             Flash();
//             if (statusController.curHealth <= 0) Die();
//         }
//     }
//     private void OnDamage()
//     {
//         noHitTime -= Time.deltaTime;
//         if (noHitTime <= 0)
//         {
//             noHitTime = hitCoolTime;
//             isDamage = false;
//         }
//     }
//     private void Flash()
//     {
//         if (flashRoutine != null) StopCoroutine(flashRoutine);
//         flashRoutine = StartCoroutine(FlashRoutine());
//     }
//     private IEnumerator FlashRoutine()
//     {
//         var wait = new WaitForSeconds(flashDuration);
//         spriteRenderer.material = flashMaterial;
//         yield return wait;
//         spriteRenderer.material = normalMaterial;
//         yield return wait;
//         spriteRenderer.material = flashMaterial;
//         yield return wait;
//         spriteRenderer.material = normalMaterial;
//         flashRoutine = null;
//     }
//     private void Die()
//     {
//         isDead = true;
//         anim.SetTrigger(IsDead);
//         rigid.velocity = Vector2.zero;
//         gameObject.layer = 12;
//     }

//     private void OnDrawGizmos()
//     {
//         // Gizmos.DrawWireSphere(transform.position, bulletRange);
//     }
// }
