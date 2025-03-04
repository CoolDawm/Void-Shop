using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHP;
    private int currentHP;

    public event Action<int> OnHealthChanged;

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;

    // private GameMenu gameMenu; ��� ����� �������  ���� ���� ��� ����������� ��� ������

    protected virtual void Awake()
    {
        currentHP = maxHP;
        // gameMenu = FindObjectOfType<GameMenu>(); ������ �� GameMenu
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        OnHealthChanged?.Invoke(currentHP);

        if (IsDead())
        {
            Die();
        }
    }

    public virtual void Regeneration(int amount)
    {
        if (amount <= 0) return;

        currentHP += amount;
        currentHP = Mathf.Min(maxHP, currentHP);

        OnHealthChanged?.Invoke(currentHP);
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    protected virtual void Die() // ��������� virtual
    {
        Debug.Log("Player died!");
        /*if (gameMenu != null)
        {
            gameMenu.ShowMenu(); ���� ����� - ���������� ������� ���� ��� ��������
        }*/ 
    }
}