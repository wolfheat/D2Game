using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIItem : MonoBehaviour
{
    public ItemData Data { get; protected set; }
    [SerializeField] protected Image image;
    protected RectTransform rect;
    public void DefineItem(ItemData newItemData)
    {
        Data = newItemData;
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        image = GetComponent<Image>();
        image.sprite = Data.Sprite;
    }
}

public class InventoryItem : UIItem, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{

    private InventoryUI inventoryUI;
    private ItemDragParent dragParent;
    public Slot ParentSlot { get; private set; }

    private void Awake()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }
    public void SetParent(Slot p)
    {
        //Debug.Log("Setting item " + Data.Itemname + " to parent " + p.gameObject.GetInstanceID());
        ParentSlot = p;
        ResetPlacement();
    }

    public ItemData DefineItem(ItemData newItemData, Slot p)
    {
        ParentSlot = p;
        rect = GetComponent<RectTransform>();
        dragParent = FindObjectOfType<ItemDragParent>();

        //placement = rect.localPosition;

        ItemData current = Data;
        Data = newItemData;

        UpdateGraphics();

        return current;
    }

    // On Right click Item


    public void OnBeginDrag(PointerEventData eventData)
    {
        rect.SetAsFirstSibling();
        transform.SetParent(dragParent.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Check if valid position to drop else return

        // Is pointer over Slot?                
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        results = results.Where(r => r.gameObject.layer == LayerMask.NameToLayer("Slot")).ToList();
        if (results.Count() > 0) //6 being my UILayer
        {
            Debug.Log("Item Dropped over a Slot: " + results[0].gameObject.name);
            if (results[0].gameObject.TryGetComponent(out Slot hitSlot))
            {
                hitSlot.SwapItemWith(this);
                SavePlayerInventory();
                
            }
        }
        else
        {
            ResetPlacement();
        }
    }

    private void SavePlayerInventory()
    {
        Debug.Log("Save Player inventory");
        inventoryUI.StoreItemDataArray();
    }

    public void ResetPlacement()
    {
        transform.SetParent(ParentSlot.transform);
        //rect.localPosition = placement;

        // Set anchors to stretch both horizontally and vertically
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        // Reset offset values
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;


    }

    private void UpdateGraphics()
    {
        image = GetComponent<Image>();
        image.sprite = Data.Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (FindObjectOfType<StashUI>().PanelEnabled)
            {
                FindObjectOfType<SavingUtility>().MoveAllItemsOfThisTypeToStash(Data.ID);
                FindObjectOfType<StashUI>().UpdateStashItems();
                FindObjectOfType<StashUI>().UpdateInventoryItems();
            }
            else
            {
                FindObjectOfType<ItemSpawner>().GenerateItem(Data);
                ParentSlot.ClearSlot();
                SavingUtility.Instance.RemoveFromInventory(ParentSlot.Index);
                Destroy(gameObject);
            }
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (FindObjectOfType<StashUI>().PanelEnabled && Inputs.Instance.Controls.Land.Shift.IsPressed())
            {
                FindObjectOfType<SavingUtility>().MoveItemToStash(ParentSlot.Index);
                FindObjectOfType<StashUI>().UpdateStashItems();
                FindObjectOfType<StashUI>().UpdateInventoryItems();
            }            
        }
    }
}
