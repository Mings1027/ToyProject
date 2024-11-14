using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Melee, Pistol, AreaEffect }

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] CameraController cameraController;
    [SerializeField] StatusController statusController;
    //Component
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [SerializeField] Transform crossHair;
    [SerializeField] AttackType attackType;
    [SerializeField] Transform bulletHole;
    [SerializeField] Transform meleePos;
    [SerializeField] Transform aimTransform;

    private Vector2 mousePos;
    private Vector2 move;
    private Vector2 aimVec;
    float rotZ;
    private bool isMove, isAttack, isDamage, isDodge;
    public bool isDead, gameStart;
    //----------------------------Value------------------------------
    [SerializeField] float walkSpeed, dodgeSpeed;
    [SerializeField] float dodgeAmount;
    [SerializeField] float i_frame;
    //OnDamage
    [SerializeField][Range(0, 1f)] float hitCoolTime;
    private float noHitTime;
    //Camera Shake
    [SerializeField] float shakeLength, shakePower;
    //Melee
    [SerializeField][Range(0, 1f)] float meleeCoolTime;
    [SerializeField] int meleeSpeed;
    //Pistol
    [SerializeField][Range(0, 1f)] float pistolCoolTime;
    [SerializeField][Range(0, 30f)] int areaAtkCoolTime;
    [SerializeField] int bulletSpeed;
    private float atkDelay;
    //Flash
    private Material normalMaterial;
    [SerializeField] Material flashMaterial;
    [SerializeField] float flashDuration;
    private Coroutine flashRoutine;
    //---------------------KeyController-----------------------------
    bool spaceKey;
    bool mouseLeftButton;

    private void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        if (isDead || !gameStart) return;
        AimControll();
        Animate();
        Move();
        Dodge();
        Attack();
        OnDamage();
    }
    private void AimControll()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimVec = mousePos - (Vector2)transform.position;
        rotZ = Mathf.Atan2(aimVec.y, aimVec.x) * Mathf.Rad2Deg;
        aimTransform.rotation = Quaternion.Euler(0, 0, rotZ);
        crossHair.transform.position = mousePos;
    }
    private void Animate()
    {
        var mouseX = mousePos.x - transform.position.x;
        var mouseY = mousePos.y - transform.position.y;

        anim.SetBool("isMove", isMove);
        if (isMove)
        {
            anim.SetFloat("MoveX", mouseX);
            anim.SetFloat("MoveY", mouseY);
        }
        else
        {
            anim.SetFloat("IdleX", mouseX);
            anim.SetFloat("IdleY", mouseY);
        }
        if (Dodge())
        {
            anim.SetTrigger("isDodge");
            anim.SetFloat("DodgeX", move.x);
            anim.SetFloat("DodgeY", move.y);
        }
    }
    private void Move()
    {
        if (isDodge) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h == 0 && v == 0)
        {
            rigid.velocity = Vector2.zero;
            isMove = false;
            return;
        }
        else isMove = true;

        move = new Vector2(h, v).normalized;
        rigid.velocity = move * walkSpeed;
    }
    private bool Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isMove && !isDodge && statusController.curStamina > dodgeAmount)
        {
            isMove = false;
            isDodge = true;
            statusController.DecreaseStamina(dodgeAmount);
            rigid.velocity = move * dodgeSpeed;
            this.Wait(i_frame, () => isDodge = false);
            return true;
        }
        else return false;
    }

    private void Attack()
    {
        if (isDodge) return;
        if (atkDelay <= 0 && Input.GetMouseButton(0) && !isAttack)
        {
            isAttack = true;

            switch (attackType)
            {
                case AttackType.Melee:
                    cameraController.StartShake(shakeLength, shakePower);
                    var m = ObjectPooler.SpawnFromPool<PlayerMeleeController>("PlayerMeleeAttack", meleePos.transform.position, Quaternion.Euler(0, 0, rotZ - 90))
                    .rigid.velocity = aimVec.normalized * meleeSpeed;
                    // if (m.transform.position.x < transform.position.x) m.transform.localScale = new Vector2(3, 3);
                    // else m.transform.localScale = new Vector2(-3, 3);
                    atkDelay = meleeCoolTime;
                    break;
                case AttackType.Pistol:
                    cameraController.StartShake(shakeLength, shakePower);
                    ObjectPooler.SpawnFromPool<PlayerBulletController>("PlayerBullet", bulletHole.position)
                    .rigid.velocity = aimVec.normalized * bulletSpeed;
                    atkDelay = pistolCoolTime;
                    break;
                case AttackType.AreaEffect:
                    cameraController.StartShake(shakeLength, shakePower);
                    ObjectPooler.SpawnFromPool<AreaEffectController>("AreaEffect", mousePos);
                    atkDelay = areaAtkCoolTime;
                    break;
            }
        }
        else
        {
            atkDelay -= Time.deltaTime;
            isAttack = false;
        }
    }
    // private void MeleeAttack()
    // {
    // }
    // private void PistolAttack()
    // {
    // }
    // private void AreaEffectAttack()
    // {
    // }

    private void OnDamage()
    {
        if (!isDamage) return;
        noHitTime -= Time.deltaTime;
        if (noHitTime <= 0)
        {
            noHitTime = hitCoolTime;
            isDamage = false;
        }
    }
    public void Die()
    {
        isDead = true;
        rigid.velocity = Vector2.zero;
        gameObject.tag = "PlayerDead";
        gameObject.layer = 7;
    }

    private IEnumerator FlashRoutine()
    {
        var wait = new WaitForSeconds(flashDuration);
        spriteRenderer.material = flashMaterial;
        yield return wait;
        spriteRenderer.material = normalMaterial;
        yield return wait;
        spriteRenderer.material = flashMaterial;
        yield return wait;
        spriteRenderer.material = normalMaterial;
        flashRoutine = null;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet") && !isDamage && !isDodge)
        {
            isDamage = true;
            cameraController.StartShake(shakeLength, shakePower);
            var enemyAtk = other.GetComponent<EnemyAttackController>();
            statusController.DecreaseHealth(enemyAtk.damage);
            if (statusController.curHealth <= 0)    //Player Die
            {
                Die();
            }
            //Flash
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashRoutine());
        }
    }

}