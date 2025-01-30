public enum Target
{
    Player,
    Enemy,
    Ball
}

public enum AttackLogic
{
    Default,
    HitPlayer,
    Phagocytosis, // 식세포 작용
    Apoptosis, // 세포 자살 (유도)
}

public enum MoveLogic
{
    Default,
    ToPlayer,
    Spawn,
}