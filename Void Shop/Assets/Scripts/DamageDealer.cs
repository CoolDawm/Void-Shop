using UnityEngine;
using System.Collections;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float damageInterval = 1f;



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
            yield return new WaitForSeconds(damageInterval);
            playerHealth.TakeDamage(damageAmount);
        }
    }
}