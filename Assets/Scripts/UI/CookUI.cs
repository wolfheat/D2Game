using System;
using System.Collections.Generic;
using UnityEngine;

public class CookUI : MonoBehaviour, IOpenCloseMenu
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject recipeHolder;
    [SerializeField] Recipe recipePrefab;
    [SerializeField] Recipe activeRecipe;

    [SerializeField] List<RecipeData> recipeDatas = new List<RecipeData>();
    [SerializeField] List<Recipe> recipes = new List<Recipe>();

    private Recipe oldRecipe;
    public bool PanelEnabled => panel.activeSelf;
    public void ShowPanel() { panel.SetActive(true); UpdateCookPanel(); }
    public void ClosePanel() => panel.SetActive(false);

    public void PopulateRecipeList()
    {
        Debug.Log("Populate Recipes");
        foreach (var recipe in recipeDatas)
        {
            Debug.Log("Recipe FOUND: "+recipe.Recipename);
            Recipe newRecipe = Instantiate(recipePrefab, recipeHolder.transform,false);
            newRecipe.Init(recipe);
            recipes.Add(newRecipe);
        }
    }
    
    public void UpdateCookPanel()
    {
        if (recipes.Count == 0) PopulateRecipeList();

        SetRecipeColorsByAvailability();
    }

    private void SetRecipeColorsByAvailability()
    {
        ItemData[] itemDatas = FindObjectOfType<InventoryUI>().GetAllItemData();
        foreach (var recipe in recipes) 
        {
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            bool hasItems = inventoryUI.HaveItems(recipe.data.ingredients);
            recipe.SetColorByAvailability(hasItems);
        }        
    }

    public void ChangeActiveRecipe(Recipe recipe)
    {
        if (activeRecipe.data != null && recipe.data.ID == activeRecipe.data.ID) return; 

        if(oldRecipe != null) oldRecipe.Deselect();
        oldRecipe = recipe;
        // Trying to change to the recipe that is already active

        Debug.Log("Change active recipe to "+recipe.data.Recipename);
        activeRecipe.Init(recipe.data);
        FindObjectOfType<SoundMaster>().PlaySFX(SoundMaster.SFX.MenuStep);
    }
    
    public void CreateActiveRecipe()
    {
        Debug.Log("Create active recipe: "+activeRecipe.data.Recipename);
        // Check if player have the required ingrediences
        // Need to reference the inventory
        ItemData[] itemDatas = FindObjectOfType<InventoryUI>().GetAllItemData();
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();

        bool hasItems = inventoryUI.HaveItems(activeRecipe.data.ingredients);
        if (hasItems)
        {
            bool didTransfer = inventoryUI.MakeTransfer(activeRecipe.data.ingredients,activeRecipe.data.results);
            if (!didTransfer) Debug.LogError("Could not complete transfer");
            else
            {
                FindObjectOfType<SoundMaster>().PlaySFX(SoundMaster.SFX.MenuSelect);
                UpdateCookPanel();
            }
        }
        else
        {
            Debug.Log("You do not have the required items in inventory");
            FindObjectOfType<SoundMaster>().PlaySFX(SoundMaster.SFX.MenuError);
        } 
    }

}
