using UnityEngine;

[CreateAssetMenu(fileName = "Ball", menuName = "ScriptableObjects/Ball", order = 0)]
public class BallScriptableObject : ScriptableObject
{
    public string prefabName;
    public Sprite prefabSprite;

    public int healthPoint;
    public int attackPoint;
    public int movePoint;

    public AttackLogic attackLogic;
    public MoveLogic moveLogic;
}