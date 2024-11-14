using UnityEngine;
using UnityEngine.Events;

public class Status : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> onHit, onDeath;
    public float currentHealthValue, currentStaminaValue;

    [SerializeField] private float recoverySpeed;
    [SerializeField] private float maxHealth;
    private float _curHealth;

    [SerializeField] private float maxStamina;
    private float _curStamina;
    [SerializeField] private float rollStamina;
    [SerializeField] private float attackStamina;
    private bool _canAttack, _canRoll, _isDead;

    [SerializeField] private float disappearTime;

    private void OnEnable()
    {
        _curHealth = maxHealth;
        _curStamina = maxStamina;
    }

    private void Update()
    {
        currentHealthValue = _curHealth / maxHealth;
        currentStaminaValue = _curStamina / maxStamina;
        if (_curStamina < maxStamina)
            _curStamina += recoverySpeed * Time.deltaTime;
        _canRoll = _curStamina > rollStamina;
        _canAttack = _curStamina > attackStamina;
    }

    public void GetHit(float amount, GameObject sender)
    {
        if (_isDead) return;
        _curHealth -= amount;
        if (_curHealth > 0) onHit?.Invoke(sender);
        else
        {
            onDeath?.Invoke(sender);
            _isDead = true;
            this.Wait(disappearTime, () => gameObject.SetActive(false));
        }
    }

    public bool RollStamina()
    {
        if (!_canRoll) return false;
        _curStamina -= rollStamina;
        return true;
    }

    public bool AttackStamina()
    {
        if (!_canAttack) return false;
        _curStamina -= attackStamina;
        return true;
    }
}