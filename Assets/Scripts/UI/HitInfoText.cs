using UnityEngine;

public enum InfoTextType{Damage,Health,XP,Info}
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
    public void CreateHitInfo(Vector3 pos, int val, InfoTextType type)
    {
        CreateHitInfo(pos, val.ToString(), type);
    }
    public void CreateHitInfo(Vector3 pos, string text, InfoTextType type)
    {
        Debug.Log("Creating hit Info: "+ text);
        // Get screen position
        pos = Camera.main.WorldToScreenPoint(pos);

        HitInfo hitInfo = Instantiate(hitInfoPrefab,transform);
        hitInfo.transform.position = pos;
        hitInfo.SetInfoText(text,type);
    }

}
