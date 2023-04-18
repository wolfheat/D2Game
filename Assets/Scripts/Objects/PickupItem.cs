using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private ItemData itemData;
	private SpriteRenderer spriteRenderer;


	public void Init(ItemData data)
    {
		itemData = data;
		spriteRenderer.sprite = itemData.Sprite;
    }

	private void OnTriggerEnter(Collider other)
	{

		if (other.GetComponent<PlayerController>() != null)
		{
			Debug.Log("PICK ME UP");
		}
	}

}
