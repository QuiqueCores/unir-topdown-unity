public class PlayerPersistent : PersistentSingleton<PlayerPersistent>
{
    public PlayerCharacter Character { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Character = GetComponentInChildren<PlayerCharacter>();
    }
}