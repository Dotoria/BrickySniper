using Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "ScriptableObjects/Skin", order = 3)]
public class SkinScriptableObject : ScriptableObject, INewGettable
{
    public bool Get { get; set; }
    public bool NewGet { get; set; }
    
    public string prefabName;
    public Sprite prefabSprite;
    public AnimationClip prefabClip;
}