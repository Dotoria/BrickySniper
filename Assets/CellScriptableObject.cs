using UnityEngine;

[CreateAssetMenu(fileName = "Cell", menuName = "ScriptableObjects/Cell", order = 0)]
public class CellScriptableObject : ScriptableObject
{
    public string prefabName;
    public Sprite prefabSprite;
    public Sprite paddleSprite;
    public Sprite arrowSprite;
    public Animator prefabAnimation;

    public float respawnTime;

    public int healthPoint;
    public int attackPoint;
    public int movePoint;

    public AttackLogic attackLogic;
    public MoveLogic moveLogic;
}