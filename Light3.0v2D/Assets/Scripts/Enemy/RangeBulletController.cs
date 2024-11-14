// using UnityEngine;

// public class RangeBulletController : MonoBehaviour
// {
//     public float bulletSpeed;
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
//         Invoke(nameof(DestroyBullet), lifeTime);
//         Vector2 moveDir = (target.position - transform.position).normalized * bulletSpeed;
//         Vector2 moveBullet = new Vector2(moveDir.x, moveDir.y);
//         rigid.velocity = moveBullet;
//     }
//     private void OnDisable()
//     {
//         rigid.velocity = Vector2.zero;
//         ObjectPooler.ReturnToPool(gameObject);
//         CancelInvoke();
//     }
//     private void DestroyBullet() => gameObject.SetActive(false);
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") || other.CompareTag("Wall"))
//             DestroyBullet();
//     }
// }
