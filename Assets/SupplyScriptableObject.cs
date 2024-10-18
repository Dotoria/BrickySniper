using UnityEngine;

[CreateAssetMenu(fileName = "Supply", menuName = "ScriptableObjects/Supply", order = 2)]
public class SupplyScriptableObject : ScriptableObjectBase
{
    public string prefabName;
    public Sprite prefabSprite;

    public int healthPoint;
    public int movePoint;

    public SupplyLogic supplyLogic;
    public Target supplyTarget;
    public MoveLogic moveLogic;
    
    public override void ApplyTo(Component component)
    {
        Supply supply = component as Supply;
        if (supply != null)
        {
            supply.healthPoint = healthPoint;
            supply.movePoint = movePoint;
            supply.supplyLogic = supplyLogic;
            supply.supplyTarget = supplyTarget;
            supply.moveLogic = moveLogic;
        }
    }
}