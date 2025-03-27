using UnityEngine;

[CreateAssetMenu(fileName = "Cell", menuName = "ScriptableObjects/Cell", order = 0)]
public class CellScriptableObject : ScriptableObject, INewGettable
{
    public bool Get { get; set; }
    public bool NewGet { get; set; }
    public int collectionIndex;
    
    public string prefabName;
    public Sprite prefabSprite;
    public Sprite paddleSprite;
    public Sprite arrowSprite;
    public Sprite bookSprite;
    public RuntimeAnimatorController prefabAnimation;

    public float respawnTime;
    public float skillTime;

    public int healthPoint;
    public int attackPoint;
    public int movePoint;

    public AttackLogic attackLogic;
    public MoveLogic moveLogic;
    private INewGettable _newGettableImplementation;
}