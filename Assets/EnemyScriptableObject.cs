using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    public string prefabName;
    public Sprite prefabSprite;
    public RuntimeAnimatorController prefabAnimation;
    
    public float firstSpawnTime;
    
    public int healthPoint;
    public int attackPoint;
    public int movePoint;

    public Target attackTarget;
    public AttackLogic attackLogic;
    public MoveLogic moveLogic;
}