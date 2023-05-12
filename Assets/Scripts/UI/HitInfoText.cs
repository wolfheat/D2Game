using UnityEngine;

public enum InfoTextType{Damage,Health,XP}
public class HitInfoText : MonoBehaviour
{

    [SerializeField] private HitInfo hitInfoPrefab;

    public static HitInfoText Instance { get; set; }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
        public void CreateHitInfo(Vector3 pos, int val, HitInfoType type)
    {
        Debug.Log("Creating hit Info: "+val);
        // Get screen position
        pos = Camera.main.WorldToScreenPoint(pos);

        HitInfo hitInfo = Instantiate(hitInfoPrefab,transform);
        hitInfo.transform.position = pos;
        hitInfo.SetInfoText(val.ToString(),type);
    }

}
