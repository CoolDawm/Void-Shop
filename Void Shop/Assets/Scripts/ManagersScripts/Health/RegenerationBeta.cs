using System.Collections;
using UnityEngine;

public class RegenerationBeta : MonoBehaviour
{
    [SerializeField] private int regenerationAmount;
    [SerializeField] private float regenerationInterval;
    [SerializeField] private float regenerationInterruptTime;

    private Health health;
    private Coroutine regenerationCoroutine;
    private float lastDamageTime;

    public void Initialize(Health health)
    {
        this.health = health;
        StartRegeneration();
    }

    public void TakeDamage()
    {
        lastDamageTime = Time.time;
        InterruptRegeneration();
    }

    private void StartRegeneration()
    {
        if (regenerationCoroutine == null && health.CurrentHP < health.MaxHP)
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
        Invoke(nameof(StartRegeneration), regenerationInterruptTime);
    }

    private IEnumerator RegenerationCoroutine()
    {
        while (!health.IsDead())
        {
            yield return new WaitForSeconds(regenerationInterval);
            if (health.CurrentHP < health.MaxHP && Time.time >= lastDamageTime + regenerationInterruptTime)
            {
                health.Regeneration(regenerationAmount);
            }
        }
    }
}
