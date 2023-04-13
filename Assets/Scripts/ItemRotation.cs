using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    float rotationAngle;
    float rotationSpeed = 90f;

	private void Update()
    {
        rotationAngle += Time.deltaTime*rotationSpeed;
		rotationAngle = rotationAngle%360;
        this.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
    }
}
