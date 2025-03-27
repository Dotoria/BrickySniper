using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyScriptableObject : ScriptableObject, INewGettable
{
    public bool Get { get; set; }
    public bool NewGet { get; set; }
    
    public string prefabName;
    public Sprite prefabSprite;
    public Sprite bookSprite;
    public RuntimeAnimatorController prefabAnimation;
    
    public float firstSpawnTime;
    
    public int healthPoint;
    public int attackPoint;
    public int movePoint;

    public Target attackTarget;
    public AttackLogic attackLogic;
    public MoveLogic moveLogic;
}