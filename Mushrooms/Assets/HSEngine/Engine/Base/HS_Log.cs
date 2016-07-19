using UnityEngine;
using System.Collections;

public class D
{
    static private bool LastEnableLog = false;
    static private bool _EnableLog = false;

    static public bool EnableLog
    {
        get
        {
            return _EnableLog;
        }
        set
        {
            LastEnableLog = _EnableLog;
            _EnableLog = value;
        }
    }

    static private void RestEnableLog()
    {
        EnableLog = LastEnableLog;
    }


    static public void LogForce(object message)
    {
        EnableLog = true;
        Log("LogForce:\t" + message);
        RestEnableLog();
    }
    static public void LogErrorForce(object message)
    {
        EnableLog = true;
        LogError("LogErrorForce:\t" + message);
        RestEnableLog();
    }

    static public void Log(object message)
    {
        Log(message, null);
    }
    static public void Log(object message, Object context)
    {
        if (EnableLog)
        {
            Debug.Log(message, context);
        }
    }
    static public void LogError(object message)
    {
        LogError(message, null);
    }
    static public void LogError(object message, Object context)
    {
        if (EnableLog)
        {
            Debug.LogError(message, context);
        }
    }
    static public void LogWarning(object message)
    {
        LogWarning(message, null);
    }
    static public void LogWarning(object message, Object context)
    {
        if (EnableLog)
        {
            Debug.LogWarning(message, context);
        }
    }

    static public void LogException(System.Exception message)
    {
        LogException(message, null);
    }
    static public void LogException(System.Exception message, Object context)
    {
        if (EnableLog)
        {
            Debug.LogException(message, context);
        }
    }
}
