using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static CellManager Instance { get; private set; }
    
    public List<CellScriptableObject> cellSO;
    public List<float> firstSpawnTime;
}