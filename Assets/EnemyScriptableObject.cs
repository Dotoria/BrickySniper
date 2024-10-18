using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyScriptableObject : ScriptableObjectBase
{
    public string prefabName;
    public Sprite prefabSprite;
    
    public int healthPoint;
    public int attackPoint;
    public int movePoint;

    public Target attackTarget;
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