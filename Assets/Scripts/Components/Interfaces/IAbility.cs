public interface IAbility
{
    void Execute();
    float Duration { get; set; }
    AbilityType AbilityType {get ; set;}
}

public enum AbilityType
{
    CharacterAbility = 0,
    OneTimeTrigger = 1,
    ConstantEffector = 2,
    Shooter = 3,
}