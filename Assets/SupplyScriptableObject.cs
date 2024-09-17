using UnityEngine;

[CreateAssetMenu(fileName = "Supply", menuName = "ScriptableObjects/Supply", order = 2)]
public class SupplyScriptableObject : ScriptableObject
{
    public string prefabName;
    public Vector3 spawnPoint;
}