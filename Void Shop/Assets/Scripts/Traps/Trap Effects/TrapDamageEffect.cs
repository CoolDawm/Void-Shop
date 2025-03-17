using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageEffect", menuName = "TrapEffects/DamageEffect")]
public class TrapDamageEffect : TrapEffect
{
    [SerializeField] private int damageAmount = 10;

    public override void ApplyEffect(GameObject target)
    {
        PlayerHealthUI health = target.GetComponent<PlayerHealthUI>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}
