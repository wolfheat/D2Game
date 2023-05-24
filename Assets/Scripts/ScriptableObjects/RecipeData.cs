using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Recipe")]
public class RecipeData : ScriptableObject
{
    public int ID;
    public string Recipename;
    public List<StackableItem> ingredients;
    public List<StackableItem> results;
}
