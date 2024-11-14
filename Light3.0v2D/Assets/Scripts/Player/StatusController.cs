using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusController : MonoBehaviour
{
    [SerializeField] Image HealthBar;
    [SerializeField] Image StaminaBar;

    [SerializeField] float maxHealth;
    [SerializeField] float maxStamina;
    [SerializeField] public float curHealth;
    [SerializeField] public float curStamina;

    private void Start()
    {
        curHealth = maxHealth;
        curStamina = maxStamina;
    }
    private void Update()
    {
        IncreaseStamina();

        if (HealthBar.fillAmount <= 1) HealthBar.fillAmount = curHealth / maxHealth;
        if (StaminaBar.fillAmount <= 1) StaminaBar.fillAmount = curStamina / maxStamina;

    }
    public void DecreaseHealth(float amount)
    {
        curHealth -= amount;
    }
    public void IncreaseHealth(float amount)
    {
        if (curHealth < maxHealth)
            curHealth += amount;
    }

    public void DecreaseStamina(float amount)
    {
        curStamina -= amount;
    }
    private void IncreaseStamina()
    {
        if (curStamina < maxStamina) curStamina += 20 * Time.deltaTime;
    }


}
