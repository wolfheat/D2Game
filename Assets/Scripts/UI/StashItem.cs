using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StashItem : UIItem, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI textField;
    
    public void SetAmount(int amt)
    {
        textField.text = amt.ToString();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left Click STash Item");
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right Click STash Item");
        }
    }
}
