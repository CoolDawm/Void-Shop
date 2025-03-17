using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StasisEffect", menuName = "TrapEffects/StasisEffect")]
public class TrapStasisEffect : TrapEffect
{
    [SerializeField] private float stasisDuration = 5f; 

    public override void ApplyEffect(GameObject target)
    {
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.Stasis(stasisDuration); 
        }
    }
}
