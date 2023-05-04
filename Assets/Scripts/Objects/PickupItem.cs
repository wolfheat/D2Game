using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private ItemData itemData;
	private SpriteRenderer spriteRenderer;

	private const float scale = 0.5f;

	public void Init(ItemData data)
    {
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		itemData = data;
        var oldSize = spriteRenderer.sprite.bounds.size.y;
		spriteRenderer.sprite = itemData.Sprite;

        // Scale to size
        var targetHeight = new Vector3(scale, scale, 1f); 
        var newSize = spriteRenderer.sprite.bounds.size.y;
        spriteRenderer.transform.localScale = (oldSize / newSize) * targetHeight;
    }

	private void OnTriggerEnter(Collider other)
	{

		if (other.GetComponent<PlayerController>() != null)
		{
			//UIController uIController = FindObjectOfType<UIController>();
			bool didAdd = UIController.Instance.AddItemToInventory(itemData);
			if(didAdd) Destroy(gameObject);
		}
	}

}
