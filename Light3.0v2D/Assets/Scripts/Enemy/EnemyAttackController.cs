using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rigid;
    [SerializeField] float atkSpeed;
    [SerializeField] float lifeTime;
    public float damage;
    [SerializeField] float defaultDamage;

    // public bool homingBullet;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        target = PlayerController.instance.transform;
    }
    private void OnEnable()
    {
        damage = defaultDamage;
        Invoke(nameof(DestroyObject), lifeTime);
        // if (homingBullet) return;
        Vector2 moveDir = (target.position - transform.position).normalized;
        Vector2 moveAtkPoint = new Vector2(moveDir.x, moveDir.y) * atkSpeed;
        rigid.velocity = moveAtkPoint;
    }
    // private void Update()
    // {
    //     if (homingBullet)
    //         transform.position = Vector2.MoveTowards(transform.position, target.position, atkSpeed * Time.deltaTime);
    // }
    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
    private void DestroyObject() => gameObject.SetActive(false);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Wall"))
            DestroyObject();
        if (other.CompareTag("AssistantWeapon")) damage /= 2;
    }
}
