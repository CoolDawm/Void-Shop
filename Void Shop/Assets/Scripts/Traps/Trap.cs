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
    [SerializeField]
    private Animator _animator;
    private bool _isTriggerd=false;
    private void Start(){
        _animator=GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggerd) return;
        if (other.CompareTag(aim.ToString()))
        {
            ApplyEffect(other.gameObject);
            _animator.SetTrigger("IsTriggered");
            _isTriggerd = true;
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