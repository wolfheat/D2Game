using System;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private ItemData itemData;
	private SpriteRenderer spriteRenderer;

	private const float scale = 0.5f;
	public bool Pickable { get; private set; } = false;

	private float speed = 3f;
	private float slowDownSpeed = 5f;
	private Vector3 direction;


    public void Init(ItemData data, Vector3 dir)
    {
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		itemData = data;
		direction = dir;
		ScaleItem();
    }

    private void Update()
    {
        if (!Pickable)
        {
            if (speed > 0)
            {
                speed -= Time.deltaTime * slowDownSpeed;
                transform.position += direction.normalized * speed * Time.deltaTime;
            }
            if (!Pickable && speed <= 0.4f)
            {
                ActivateCollider();
                Pickable = true;

            }
        }
    }

    private void ActivateCollider()
    {
        GetComponent<Collider>().enabled = true;
    }

    private void ScaleItem()
    {
        var oldSize = spriteRenderer.sprite.bounds.size.y;
        spriteRenderer.sprite = itemData.Sprite;

        // Scale to size
        var targetHeight = new Vector3(scale, scale, 1f);
        var newSize = spriteRenderer.sprite.bounds.size.y;
        spriteRenderer.transform.localScale = (oldSize / newSize) * targetHeight;
    }

    private void OnTriggerEnter(Collider other)
	{

		if (other.GetComponent<PlayerController>() != null && Pickable)
		{
			//UIController uIController = FindObjectOfType<UIController>();
			bool didAdd = UIController.Instance.AddItemToInventory(itemData);
			if(didAdd) Destroy(gameObject);
		}
	}

}
