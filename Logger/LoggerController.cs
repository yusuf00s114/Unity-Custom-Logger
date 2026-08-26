using UnityEngine;

[CreateAssetMenu(fileName = "LoggerController", menuName = "Logger/LoggerController")]
public class LoggerController : ScriptableObject
{
    [SerializeField] private LoggerConfiguration loggerConfiguration;

    public LoggerConfiguration Config => loggerConfiguration;

    private void Reset()
    {
        if (loggerConfiguration == null)
            Debug.LogError("LoggerConfiguration is not assigned in LoggerController! " +
                           "Please create a LoggerConfiguration asset and" +
                           " place it in the Resources folder with the name 'DefaultLoggerController'.",
                this);
    }
}