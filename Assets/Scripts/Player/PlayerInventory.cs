using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    // Players Stash
    public Dictionary<int, int> Stash { get; set; } = new Dictionary<int, int>();

    // Players Inventory
    public int[] Items { get; set; }= new int[20];
    public float[] TownPos { get; set; }
    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 1000;
    public int Energy { get; set; } = 100;
    public int MaxEnergy { get; set; } = 1000;
    public int XP { get; set; } = 0;
    public int MaxXP { get; set; } = 10000;
    public float AttackSpeedMultiplyer { get; set; } = 1.8f;
    public int HitDamage { get; set; } = 200;
    public float AttackTime { get; set; } = 1.22f;

}
