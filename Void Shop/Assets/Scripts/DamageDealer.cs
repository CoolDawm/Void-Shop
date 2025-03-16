using UnityEngine;
using System.Collections;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10; 
    public float damageInterval = 1f; 

    private bool isPlayerOnSpikes = false; 
    private PlayerHealthUI playerHealth; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealthUI>();

            if (playerHealth != null)
            {
                isPlayerOnSpikes = true;
                StartCoroutine(DealDamage());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnSpikes = false;
        }
    }

    private IEnumerator DealDamage()
    {
        while (isPlayerOnSpikes && playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}