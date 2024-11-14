// using UnityEngine;

// public class HomingBulletController : MonoBehaviour
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
//     }
//     private void Update()
//     {
//         transform.position = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * Time.deltaTime);
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
