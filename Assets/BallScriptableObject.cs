using UnityEngine;

[CreateAssetMenu(fileName = "Ball", menuName = "ScriptableObjects/Ball", order = 0)]
public class BallScriptableObject : ScriptableObjectBase
{
    public string prefabName;
    public Sprite prefabSprite;

    public int healthPoint;
    public int attackPoint;
    public int movePoint;

    public AttackLogic attackLogic;
    public MoveLogic moveLogic;
    
    public override void ApplyTo(Component component)
    {
        Supply supply = component as Supply;
        if (supply != null)
        {
            supply.healthPoint = this.healthPoint;
            supply.movePoint = this.movePoint;
        }
    }
}