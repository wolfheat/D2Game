using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{
    public ItemData Data { get; private set; }
    [SerializeField] private Image image;
    private RectTransform rect;
    private Vector3 speed = Vector3.zero;
    private Vector3 placement = Vector3.zero;
    private ItemDragParent dragParent;
    public Slot ParentSlot { get; private set; }

    public void SetParent(Slot p)
    {
        Debug.Log("Setting item "+Data.Itemname+" to parent "+p.gameObject.GetInstanceID());
        ParentSlot = p;
        ResetPlacement();
    }

    public ItemData DefineItem(ItemData newItemData, Slot p)
    {
        ParentSlot = p;
        rect = GetComponent<RectTransform>();
        dragParent = FindObjectOfType<ItemDragParent>();

        placement = rect.localPosition;

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
        rect.position = Input.mousePosition;
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
            if(results[0].gameObject.TryGetComponent(out Slot hitSlot))
            {
                hitSlot.SwapItemWith(this);
            }
        }
        else
        {
            ResetPlacement();
        }
    }

    public void ResetPlacement()
    {
        transform.SetParent(ParentSlot.transform);
        rect.localPosition = placement;
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
            FindObjectOfType<ItemSpawner>().GenerateItem(Data);
            ParentSlot.ClearSlot();
            Destroy(gameObject);
        }
    }
}
