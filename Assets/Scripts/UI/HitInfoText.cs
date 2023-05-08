using UnityEngine;

public class HitInfoText : MonoBehaviour
{

    [SerializeField] private HitInfo hitInfoPrefab;

    public void CreateHitInfo(Vector3 pos, int val)
    {
        Debug.Log("Creating hit Info: "+val);
        // Get screen position
        pos = Camera.main.WorldToScreenPoint(pos);

        HitInfo hitInfo = Instantiate(hitInfoPrefab,transform);
        hitInfo.transform.position = pos;
        hitInfo.SetInfoText(val.ToString());
    }

}
