using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    // Players Stash
    public Dictionary<int, int> Stash { get; set; } = new Dictionary<int, int>();

    // Players Inventory
    public int[] Items { get; set; }= new int[20];

}
