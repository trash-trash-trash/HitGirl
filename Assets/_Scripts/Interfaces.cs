public interface IHear
{
    public void HeardSound(SoundData sound);

    public void FlipCanHear(bool input);
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
    public void AggroAction();
}