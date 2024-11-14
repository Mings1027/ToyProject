// using UnityEngine;

// public class MeleeAttackController : MonoBehaviour
// {
//     public float atkSpeed;
//     public float lifeTime;
//     public Rigidbody2D rigid;
//     public Transform target;
//     public float damage;

//     private void Awake()
//     {
//         target = Player.instance.transform;
//     }
//     private void OnEnable()
//     {
//         Invoke(nameof(DestroyObject), lifeTime);
//         Vector2 moveDir = (target.position - transform.position).normalized;
//         Vector2 moveAtkPoint = new Vector2(moveDir.x, moveDir.y);
//         rigid.velocity = moveAtkPoint * atkSpeed;
//     }
//     private void OnDisable()
//     {
//         rigid.velocity = Vector2.zero;
//         ObjectPooler.ReturnToPool(gameObject);
//         CancelInvoke();
//     }
//     private void DestroyObject() => gameObject.SetActive(false);
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player")) DestroyObject();
//     }
// }
