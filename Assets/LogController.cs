using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LogController : MonoBehaviour
{

    private TMPro.TextMeshProUGUI logText;

    Queue<string> log = new Queue<string>(6);

    private void Start()
    {
        this.logText = GetComponent<TMPro.TextMeshProUGUI>();
        this.logText.text = "";
    }

    public void Log(string text)
    {
        Debug.Log("adding text to log: " + text);
        this.log.Enqueue(text);
        if (this.log.Count > 6)
            this.log.Dequeue();

        this.logText.text = "";
        foreach (var logItem in this.log.Reverse())
        {
            this.logText.text += logItem + "\n";
        }
    }
}
