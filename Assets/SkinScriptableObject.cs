using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "ScriptableObjects/Skin", order = 3)]
public class SkinScriptableObject : ScriptableObject
{
    public string prefabName;
    public Sprite prefabSprite;
    public AnimationClip prefabClip;
}