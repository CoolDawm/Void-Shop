using UnityEngine;
public enum TargetType
{
    Player,
    Monster
}
public  class Trap : MonoBehaviour
{
    [SerializeField] protected TrapEffect effect;
    [SerializeField]
    private TargetType aim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(aim.ToString()))
        {
            ApplyEffect(other.gameObject);
        }
    }

    private void ApplyEffect(GameObject target)
    {
        if (effect != null)
        {
            effect.ApplyEffect(target);
        }
    }
}