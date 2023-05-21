using System.   Collections;
using TMPro;
using UnityEngine;


//public enum HitInfoType{Damage,Healing,XP}

public class HitInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;

    private const float FadeRate = 0.02f;
    private bool isInfo = false;
    public void SetInfoText(string infoString, InfoTextType info = InfoTextType.Damage)
    {
        textField.text = infoString;
        SetColor(info);
    }

    private void SetColor(InfoTextType info)
    {
        switch (info)
        {
            case InfoTextType.Damage:
                textField.color = Color.red;
                break;
            case InfoTextType.Health:
                textField.color = Color.green;
                break;
            case InfoTextType.XP:
                textField.color = Color.yellow;
                break;
           case InfoTextType.Info:
                isInfo = true;
                textField.color = Color.white;
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        //textField.color.a = 1f;
        textField.alpha = 1.0f;
        while (textField.alpha > 0f) 
        {
            textField.alpha -= isInfo? FadeRate/3: FadeRate;
            transform.position += Vector3.up*0.1f; 
            yield return null;
        }
        Destroy(gameObject);
    }

}
