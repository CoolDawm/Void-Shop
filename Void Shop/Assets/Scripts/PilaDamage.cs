using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilaDamage : MonoBehaviour
{
    [SerializeField] DamageDelayTimer _damageDelayTimer;
    [SerializeField] private int _damageAmount = 10;
    private PlayerHealthUI _playerHealth;
    private bool isPlayerOnSpikes = false;
    private bool _damageActivated;

    private void Start()
    {
        _damageActivated = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerHealth = other.GetComponent<PlayerHealthUI>();

            if (_playerHealth != null && _damageActivated)
            {
                //isPlayerOnSpikes = true;
                ApplyDamage();
            }
        }
    }
      private void ApplyDamage()
    {
        //Debug.Log("DamageApplied");
        _playerHealth.TakeDamage(_damageAmount);
        _damageActivated = false; //поставили задержку на урон
        _damageDelayTimer.Restart();
    }

    public void ActivateDamage()
    {
        _damageActivated = true;
    }
}
