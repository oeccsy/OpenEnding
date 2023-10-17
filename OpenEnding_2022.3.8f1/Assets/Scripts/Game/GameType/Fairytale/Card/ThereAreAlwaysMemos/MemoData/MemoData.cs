using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MemoData", menuName = "Scriptable Object/MemoData")]
public class MemoData : ScriptableObject
{
    [TextArea]
    public string textContent;
    public Sprite sprite;
    public Vector2 textPos;
    public Vector2 imagePos;
}
