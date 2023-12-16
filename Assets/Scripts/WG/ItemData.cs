using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Create New ItemData")]
public class ItemData : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public int index;
    public int addHP;
    public float speed;
}
