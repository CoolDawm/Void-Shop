using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int regenerationAmount = 10;
    [SerializeField] private float regenerationInterval = 1f;
    [SerializeField] private float regenerationInterruptTime = 3f;

    public Image healthBar;
    public Image backgroundBar;
    public Image damageScreen;

    private int currentHP;
    private Coroutine regenerationCoroutine;
    private float lastDamageTime;

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
        StartRegeneration();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        UpdateHealthUI();
        lastDamageTime = Time.time;
        InterruptRegeneration();

        if (IsDead())
        {
            Die();
        }
    }
    private void DisplayDamageScreen()
    {
        float transparency = 1f - (currentHP / maxHP);
        Color damageColor = Color.white;
        damageColor.a = transparency;
        damageScreen.color = damageColor;
    }

    private void DisplayDamageScreen()
    {
        float transparency = 1f - (currentHP / maxHP);
        Color damageColor = Color.white;
        damageColor.a = transparency;
        damageScreen.color = damageColor;
    }

    private void Regeneration(int amount)
    {
        if (amount <= 0) return;

        currentHP += amount;
        currentHP = Mathf.Min(maxHP, currentHP);

        UpdateHealthUI();
    }

    private bool IsDead()
    {
        return currentHP <= 0;
    }

    private void Die()
    {
        FindAnyObjectByType<GameUIManager>().ShowLoseMenu();
        GameManager.Instance.EndGame();
        Debug.Log("Player died!");
        // логика для смерти игрока
    }

    private void StartRegeneration()
    {
        if (regenerationCoroutine == null && currentHP < maxHP)
        {
            regenerationCoroutine = StartCoroutine(RegenerationCoroutine());
        }
    }

    private void StopRegeneration()
    {
        if (regenerationCoroutine != null)
        {
            StopCoroutine(regenerationCoroutine);
            regenerationCoroutine = null;
        }
    }

    private void InterruptRegeneration()
    {
        StopRegeneration();
        StartCoroutine(DelayedRegeneration());
    }

    private IEnumerator DelayedRegeneration()
    {
        yield return new WaitForSeconds(regenerationInterruptTime);
        StartRegeneration();
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = (float)currentHP / maxHP;
        DisplayDamageScreen();
    }

    private IEnumerator RegenerationCoroutine()
    {
        while (!IsDead())
        {
            yield return new WaitForSeconds(regenerationInterval);
            if (currentHP < maxHP && Time.time >= lastDamageTime + regenerationInterruptTime)
            {
                Regeneration(regenerationAmount);
            }
        }
    }
}