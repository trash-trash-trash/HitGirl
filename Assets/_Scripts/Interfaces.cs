public interface IInteractable
{
    public void Interact(CharacterActions action);
}

public interface IHear
{
    public void HeardSound(SoundData sound);

    public void FlipCanHear(bool input);
}

public interface IMemorable
{
    
}

public interface ISee
{
    public bool ReturnCanSee();

    public bool ReturnCanSeePlayer();

    public void ChangeCanSeePlayer(CharacterBase player, bool input);

    public void ChangeCanSee(bool input);
}

public interface ICharacter
{
    public Character ReturnCharacter();

    public CharacterBase ReturnCharacterBase();
}

public interface IWeapon
{
    public void Equip(WeaponSO weaponSO);

    public void Aim();

    public void Holster();
    
    public void AggroAction();

    public IWeapon ReturnSelf();
}