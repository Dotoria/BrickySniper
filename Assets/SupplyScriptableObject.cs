using UnityEngine;

[CreateAssetMenu(fileName = "Supply", menuName = "ScriptableObjects/Supply", order = 2)]
public class SupplyScriptableObject : ScriptableObject
{
    public string prefabName;
    public Sprite prefabSprite;

    public int healthPoint;
    public int movePoint;

    public SupplyLogic supplyLogic;
    public Target supplyTarget;
    public MoveLogic moveLogic;
}

public enum SupplyLogic
{
    Default,
    Split
}