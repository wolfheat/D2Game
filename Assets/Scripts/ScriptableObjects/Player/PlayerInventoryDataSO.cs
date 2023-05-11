using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable][CreateAssetMenu(fileName = "PLayeInventoryData", menuName = "PlayerInventoryData")]
public class PlayerInventoryDataSO: ScriptableObject
{
    // Players Stash
    public Dictionary<int, int> Stash { get; set; } = new Dictionary<int, int>();

    // Players Inventory
    public ItemData[] Items { get; set; } = new ItemData[20];

}
