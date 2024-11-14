using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float lifeTime;
    public Rigidbody2D rigid;
    public float damage;
    public float thrust;
    public float knockbackTime;
    private void OnEnable()
    {
        player = PlayerController.instance.transform;
        Invoke(nameof(DestroyObject), lifeTime);
    }
    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
    private void DestroyObject() => gameObject.SetActive(false);
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            ObjectPooler.SpawnFromPool("MeleeEffect", other.transform.position);
            var enemy = other.GetComponent<EnemyController>();
            enemy.KnockbackCheck(player, enemy.rigid, thrust, knockbackTime);
            enemy.health -= damage;
            // DestroyObject();
        }
        if (other.CompareTag("Wall"))
        {
            ObjectPooler.SpawnFromPool("MeleeEffect", transform.position + transform.up);
        }
    }

}
