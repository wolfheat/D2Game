using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private ItemData itemData;
    private int scale = 20;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Image image;

    public ItemData DefineItem(ItemData newItemData)
    {

        ItemData current = itemData;
        itemData = newItemData;

        UpdateGraphics();

        return current;
    }
    private void UpdateGraphics()
    {
        image = GetComponent<Image>();
        image.sprite = itemData.Sprite;
    }

}
