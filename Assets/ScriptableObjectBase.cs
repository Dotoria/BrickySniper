using UnityEngine;

public abstract class ScriptableObjectBase : ScriptableObject
{
    public abstract void ApplyTo(Component component);
}

public enum Target
{
    Player,
    Enemy,
    Ball
}

public enum AttackLogic
{
    Default,
    HitPlayer
}

public enum MoveLogic
{
    Default,
    ToPlayer,
    Spawn,
}

public enum SupplyLogic
{
    Default,
    Split
}