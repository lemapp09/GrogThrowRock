using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugDisplay : MonoBehaviour
{
    private Dictionary<string, string> debuglogs = new Dictionary<string, string>();

    public TextMeshProUGUI _display;

    private void Update()
    {
        Debug.Log("Time:" + Time.time);
        Debug.Log(gameObject.name);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLogs;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLogs;
        
    }

    private void HandleLogs(string logstring, string stacktrace, LogType type)
    {
        if (type == LogType.Log)
        {
            string[] splitString = logstring.Split(char.Parse(":"));
            string debugKey = splitString[0];
            string debugValue = splitString.Length > 1 ? splitString[1] : "";

            if (debuglogs.ContainsKey(debugKey))
            {
                debuglogs[debugKey] = debugValue;
            } else {
                debuglogs.Add(debugKey, debugValue);
            }
        }

        string displayText = "";
        foreach (KeyValuePair<string, string> log in debuglogs)
        {
            if (log.Value == "")
                displayText += log.Key + "\n";
            else
            {
                displayText += log.Key + ": " + log.Value + "\n";
            }

            _display.text = displayText;
        }

    }
}


