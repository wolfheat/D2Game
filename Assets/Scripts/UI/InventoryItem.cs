using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private ItemData itemData;
    [SerializeField] private Image image;
    private RectTransform rect;
    private Vector3 speed = Vector3.zero;
    private Vector3 placement = Vector3.zero;
    private ItemDragParent dragParent;    
    private Slot parentSlot;

    public ItemData DefineItem(ItemData newItemData, Slot p)
    {
        parentSlot = p;
        rect = GetComponent<RectTransform>();
        dragParent = FindObjectOfType<ItemDragParent>();

        placement = rect.localPosition;

        ItemData current = itemData;
        itemData = newItemData;

        UpdateGraphics();

        return current;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rect.SetAsFirstSibling();
        transform.parent = dragParent.transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Check if valid position to drop else return
        transform.parent = parentSlot.transform;
        rect.localPosition = placement;

    }

    private void UpdateGraphics()
    {
        image = GetComponent<Image>();
        image.sprite = itemData.Sprite;
    }


}
