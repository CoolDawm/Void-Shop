public class PlayerHealth : Health
{
    private Regeneration regeneration;

    protected override void Awake()
    {
        base.Awake();
        regeneration = gameObject.AddComponent<Regeneration>();
        regeneration.Initialize(this);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        regeneration.TakeDamage();
    }

    public override void Regeneration(int amount)
    {
        base.Regeneration(amount);
        if (CurrentHP == MaxHP)
        {
            Destroy(regeneration);
        }
    }
}
