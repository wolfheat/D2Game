using System;
using System.Collections;
using TMPro;
using UnityEngine;


public enum HitInfoType{Damage,Healing,XP}

public class HitInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;

    private const float FadeRate = 0.02f;
    public void SetInfoText(string infoString, HitInfoType info = HitInfoType.Damage)
    {
        textField.text = infoString;
        SetColor(info);
    }

    private void SetColor(HitInfoType info)
    {
        switch (info)
        {
            case HitInfoType.Damage:
                textField.color = Color.red;
                break;
            case HitInfoType.Healing:
                textField.color = Color.green;
                break;
            case HitInfoType.XP:
                textField.color = Color.yellow;
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
            textField.alpha -= FadeRate;
            transform.position += Vector3.up*0.1f; 
            yield return null;
        }
        Destroy(gameObject);
    }

}
