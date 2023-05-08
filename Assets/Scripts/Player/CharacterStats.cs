using System.Collections.Generic;
using static UnityEditor.Progress;
using UnityEngine;

public static class CharacterStats 
{
    public static int HitDamage { get; } = 200;
    //public int Speed { get; } = ;
    public static float AttackTime { get; } = 1.22f;
    public static float AttackSpeedMultiplyer { get; } = 1.8f;
    public static int Health { get; set; } = 50;
    public static int HealthMax { get; set; } = 100;
    public static int Energy { get; set; } = 200;
    public static int EnergyMax { get; set; } = 400;
    public static int XP { get; set; } = 200;
    public static int XPMax { get; set; } = 400;
    public static ItemData[] Items { get; set; } = new ItemData[20];

    // <ID,amount>
    public static Dictionary<int, int> Stash { get; set; } = new Dictionary<int, int>();

    public static void AddItemsToStash()
    {
        // Copy Items to Stash
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null) continue;

            if(!Stash.ContainsKey(Items[i].ID)) Stash.Add(Items[i].ID, 0);
            int currentValue = Stash[Items[i].ID];
            Stash[Items[i].ID] = currentValue+1;
            // remove the Items from inventory
            Items[i] = null;
        }
    }
       
    public static int RemoveSingleItemFromStash(int ID)
    {
        //Debug.Log("Trying to remove item that dont exist");
        if (Stash.ContainsKey(ID))
        {
            Stash[ID] = Stash[ID]--;
            if (Stash[ID] == 0)
            {
                Stash.Remove(ID);
            }
            return ID;
        }            
        return -1;
    }


}
