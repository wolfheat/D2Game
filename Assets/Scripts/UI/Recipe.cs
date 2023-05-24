using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    [SerializeField] GameObject itemsHolder;
    [SerializeField] StashItem itemPrefab;
    [SerializeField] GameObject resultArrowPrefab;
    [SerializeField] Image image;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color availableColor;
    [SerializeField] private Color unavailableColor;
    private Color normalColor;

    private List<StackableItem> ingredients = new List<StackableItem>();
    private List<StackableItem> results = new List<StackableItem>();

    public RecipeData data;

    public void RequestSetToActiveRecipe()
    {
        //Debug.Log("Clicked on Recipe set to Active");
        FindObjectOfType<CookUI>().ChangeActiveRecipe(this);
        image.color = selectedColor;
    }
    
    public void Deselect()
    {
        image.color = normalColor;
    }

    public void SetColorByAvailability(bool available)
    {
        if (available) normalColor = availableColor;
        else normalColor = unavailableColor;
        Deselect();
    }

    public void Init(RecipeData recipeData)
    {
        data = recipeData;
        Clear();
        FillRecipe();
        SetColorByAvailability(false);
    }

    private void Clear()
    {
        foreach (Transform child in itemsHolder.transform) 
        {
            Destroy(child.gameObject);
        }
    }

    private void FillRecipe()
    {
        ingredients = data.ingredients;
        results = data.results;
        foreach (StackableItem item in ingredients) 
        {
            StashItem newItem = Instantiate(itemPrefab,itemsHolder.transform,false);
            newItem.DefineItem(item.itemData);
            newItem.SetAmount(item.amount);
            newItem.RemoveRayCastTarget();
        }
        
        // Create Arrow
        GameObject arrow = Instantiate(resultArrowPrefab,itemsHolder.transform,false);

        foreach (StackableItem item in results) 
        {
            StashItem newItem = Instantiate(itemPrefab,itemsHolder.transform,false);
            newItem.DefineItem(item.itemData);
            newItem.RemoveRayCastTarget();
        }
    }
}
