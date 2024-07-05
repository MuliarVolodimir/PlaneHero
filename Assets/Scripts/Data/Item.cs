using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = ("ScriptableObjects/Item"))]
public class Item : ScriptableObject
{
    public int Name;
    public Sprite Sprite;
}
