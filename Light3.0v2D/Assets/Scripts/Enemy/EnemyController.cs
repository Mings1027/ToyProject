using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Melee, Range, Homing
};
public class EnemyController : MonoBehaviour
{
    private Transform target;
    //Component
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D rigid;

    [SerializeField] EnemyType enemyType;
    //Patrol
    [SerializeField] LayerMask checkLayer;
    [SerializeField][Range(1, 15)] int chasingRange;
    [SerializeField][Range(0, 10f)] float attackRange;
    //ChooseDirecion
    [SerializeField][Range(0.1f, 3f)] float circleCastSize, eyeSightRange;
    private List<Vector2> ranMoveList = new List<Vector2>();
    private Vector3 dir;
    //Control Inspector
    [SerializeField][Range(0, 10)] int moveSpeed;
    [SerializeField][Range(0, 2f)] float attackCool;
    private float attackDelay;
    [SerializeField][Range(0, 200)] int maxHealth;
    public float health;
    //Bool Controll
    [SerializeField] bool targetVisiable, isMove, isAttack, isDead;
    public bool isDamage;
    //Animation
    private readonly int IsMove = Animator.StringToHash("isMove");
    private readonly int IsAttack = Animator.StringToHash("isAttack");
    private readonly int IsDamage = Animator.StringToHash("isDamage");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ranMoveList.Add(Vector2.right);
        ranMoveList.Add(Vector2.left);
        ranMoveList.Add(Vector2.up);
        ranMoveList.Add(Vector2.down);
        ranMoveList.Add(new Vector2(1, 1));
        ranMoveList.Add(new Vector2(-1, 1));
        ranMoveList.Add(new Vector2(1, -1));
        ranMoveList.Add(new Vector2(-1, -1));
        dir = ranMoveList[Random.Range(0, 8)];
        health = maxHealth;
    }
    private void Start() => target = PlayerController.instance.transform;
    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
        health = maxHealth;
        gameObject.tag = "Enemy";
        gameObject.layer = 8;
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
    private void Update()
    {
        Animation();
        Patrol();
    }

    private void Animation()
    {
        if (isMove) anim.SetBool(IsMove, true);
        else anim.SetBool(IsMove, false);
    }

    private void Patrol()
    {
        Vector2 disPlayer = target.transform.position - transform.position;
        var hit = Physics2D.Raycast(transform.position, disPlayer, chasingRange, checkLayer);
        if (Vector2.Distance(transform.position, target.transform.position) < chasingRange && hit && hit.collider.CompareTag("Player"))
        {
            if (Vector2.Distance(transform.position, target.transform.position) < attackRange)
            {
                isAttack = true;
                isMove = false;
                Attack();
            }
            else
            {
                isAttack = false;
                isMove = true;
                targetVisiable = true;
                ChasePlayer();
            }
        }
        else
        {
            targetVisiable = false;
            isAttack = false;
            isMove = true;
            RandomMove();
        }
    }
    private void RandomMove()
    {
        //if (targetVisiable) return;
        var hit = Physics2D.CircleCast(transform.position, circleCastSize, dir, eyeSightRange);

        if (hit.collider != null && (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Enemy")))
        {
            dir = ranMoveList[Random.Range(0, 8)];
        }

        transform.Translate((dir.normalized * moveSpeed) * Time.deltaTime);
        if (dir.x > 0) spriteRenderer.flipX = true;
        if (dir.x < 0) spriteRenderer.flipX = false;
    }
    private void ChasePlayer()
    {
        //if (!targetVisiable || isAttack) return;
        Vector2 move = target.transform.position - transform.position;
        transform.Translate(move.normalized * moveSpeed * Time.deltaTime);
        if (target.transform.position.x > transform.position.x)
            spriteRenderer.flipX = true;
        if (target.transform.position.x < transform.position.x)
            spriteRenderer.flipX = false;
    }
    private void Attack()
    {
        //if (!isAttack) return;
        //rigid.velocity = Vector2.zero;
        attackDelay -= Time.deltaTime;
        if (isDamage) return;
        if (attackDelay <= 0) switch (enemyType)
            {
                case (EnemyType.Melee):
                    ObjectPooler.SpawnFromPool("EnemyMeleeAttack", transform.position);
                    anim.SetTrigger(IsAttack);
                    attackDelay = attackCool;
                    break;
                case (EnemyType.Range):
                    ObjectPooler.SpawnFromPool("EnemyRangeBullet", transform.position);
                    anim.SetTrigger(IsAttack);
                    attackDelay = attackCool;
                    break;
                case (EnemyType.Homing):
                    ObjectPooler.SpawnFromPool("EnemyHomingBullet", transform.position);
                    anim.SetTrigger(IsAttack);
                    attackDelay = attackCool;
                    break;
            }
    }

    public void Die()
    {
        isDead = true;
        gameObject.tag = "EnemyDead";
        gameObject.layer = 10;
        //Dead Animation Play
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WorldEnd")) gameObject.SetActive(false);

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack") || other.CompareTag("PlayerAreaAttack"))
        {
            isDamage = true;
            if (!isDead)
                this.Wait(0.1f, () => isDamage = false);
            anim.SetTrigger(IsDamage);
            if (health <= 0) Die();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chasingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + (dir) * eyeSightRange, circleCastSize);
        Gizmos.DrawWireSphere(transform.position, eyeSightRange);
    }
}
