using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectController : MonoBehaviour
{
    public float lifeTime;
    public float damage;
    public float atkDelay;
    public float coolTime;

    public float thrust;
    public float knockbackTime;


    private void OnEnable() => Invoke(nameof(DestroyObject), lifeTime);
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
    private void DestroyObject() => gameObject.SetActive(false);

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         var enemy = other.GetComponent<EnemyController>();
    //         if (enemy.health > 0)
    //         {
    //             enemy.KnockbackCheck(enemy.target, enemy.rigid, thrust, knockbackTime);
    //             enemy.health -= damage;
    //         }
    //         else enemy.Die();
    //     }
    // }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (atkDelay <= 0)
            {
                var enemy = other.GetComponent<EnemyController>();
                enemy.rigid.sleepMode = RigidbodySleepMode2D.NeverSleep;
                enemy.KnockbackCheck(transform, enemy.rigid, thrust, knockbackTime);
                enemy.health -= damage;
                atkDelay = coolTime;
            }
            else
            {
                atkDelay -= Time.deltaTime;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().rigid.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }
}
