using System;
using System.Collections;
using UnityEngine;

public class InGameConsol : MonoBehaviour
{
    [SerializeField] GameObject ConsolTextHolder;
    [SerializeField] ConsoleText ConsolTextPrefab;
    [SerializeField] GameObject panel;
    private Queue queue = new Queue();

    public static InGameConsol Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null) Destroy(this);
        else Instance = this;

        LoadAllActiveChildren();
    }

    private void LoadAllActiveChildren()
    {
        foreach (Transform child in ConsolTextHolder.transform)
        {
            queue.Enqueue(child.GetComponent<ConsoleText>());
        }
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void AddInfo(string infotext)
    {
        if(queue.Count >= 10)
        {
            //Debug.Log("Destroy INFO");
            ConsoleText consolText = queue.Dequeue() as ConsoleText;
            Destroy(consolText.gameObject);
        }

        queue.Enqueue(CreateNewConsolText(infotext));
    }

    private ConsoleText CreateNewConsolText(string infotext)
    {
        ConsoleText newConsolText = Instantiate(ConsolTextPrefab, ConsolTextHolder.transform);
        newConsolText.TMPtext.text = infotext;
        return newConsolText;
    }
}
