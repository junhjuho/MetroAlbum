using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLog : MonoBehaviour
{
    public Text debugText;
    public int maxLines = 15;

    private Queue<string> logs = new Queue<string>();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logs.Enqueue(logString);

        if (logs.Count > maxLines)
            logs.Dequeue();

        debugText.text = string.Join("\n", logs);
    }
}
