using UnityEngine;

[CreateAssetMenu(fileName = "Ball", menuName = "ScriptableObjects/Ball", order = 0)]
public class BallScriptableObject : ScriptableObject
{
    public string prefabName;
    public Vector3 spawnPoint;
}