using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

/// <summary>
///     Custom-made generic logger class.
///     Use this instead of Debug.Log() whenever possible.
///     Configure this through Assets/Resources/DefaultLoggerController.
///     Create DefaultLoggerController if it doesn't exist through
///     Assets -> Create -> Logger -> DefaultLoggerController.
/// </summary>
public static class Logger
{
    private static LoggerController _config;

    private static LoggerController Config =>
        _config ?? throw new InvalidOperationException("Logger has not been initialized yet.");

    // This attribute tells Unity to run this method automatically 
    // before the first scene even loads.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        // Load the config from the Resources folder by its filename
        var config = Resources.Load<LoggerController>("DefaultLoggerController");

        if (config != null)
        {
            SetConfigOnce(config);
        }
        else
        {
            Debug.LogError("Logger: Could not find 'DefaultLogConfig' in Resources!\n" +
                           "Please create a LoggerConfiguration asset and" +
                           " place it in the Resources folder with the name 'DefaultLoggerController'.");
        }

    }

    /// <summary>
    ///     Logs an informational message through the project logger.
    /// </summary>
    /// <param name="message">Message text to write.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void Log(string message, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.Log(FormatMessage(message, channel));
    }

    /// <summary>
    ///     Logs a formatted informational message through the project logger.
    /// </summary>
    /// <param name="format">Composite format string.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    /// <param name="args">Values used to format <paramref name="format" />.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, LogChannel channel = LogChannel.GENERAL, params object[] args)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogFormat(FormatFormat(format, channel), args);
    }

    /// <summary>
    ///     Logs a formatted informational message with a Unity context object.
    /// </summary>
    /// <param name="context">Unity object linked to the log entry.</param>
    /// <param name="format">Composite format string.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    /// <param name="args">Values used to format <paramref name="format" />.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogFormat(Object context, string format, LogChannel channel = LogChannel.GENERAL,
        params object[] args)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogFormat(context, FormatFormat(format, channel), args);
    }

    /// <summary>
    ///     Logs a warning message through the project logger.
    /// </summary>
    /// <param name="message">Warning text to write.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(string message, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogWarning(FormatMessage(message, channel));
    }

    /// <summary>
    ///     Logs a warning message with a Unity context object.
    /// </summary>
    /// <param name="message">Warning text to write.</param>
    /// <param name="context">Unity object linked to the log entry.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(string message, Object context, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogWarning(FormatMessage(message, channel), context);
    }

    /// <summary>
    ///     Logs a formatted warning message through the project logger.
    /// </summary>
    /// <param name="format">Composite format string.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    /// <param name="args">Values used to format <paramref name="format" />.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(string format, LogChannel channel = LogChannel.GENERAL, params object[] args)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogWarningFormat(FormatFormat(format, channel), args);
    }

    /// <summary>
    ///     Logs a formatted warning message with a Unity context object.
    /// </summary>
    /// <param name="context">Unity object linked to the log entry.</param>
    /// <param name="format">Composite format string.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    /// <param name="args">Values used to format <paramref name="format" />.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(Object context, string format, LogChannel channel = LogChannel.GENERAL,
        params object[] args)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogWarningFormat(context, FormatFormat(format, channel), args);
    }

    /// <summary>
    ///     Logs an error message through the project logger.
    /// </summary>
    /// <param name="message">Error text to write.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogError(string message, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogError(FormatMessage(message, channel));
    }

    /// <summary>
    ///     Logs an error message with a Unity context object.
    /// </summary>
    /// <param name="message">Error text to write.</param>
    /// <param name="context">Unity object linked to the log entry.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogError(string message, Object context, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogError(FormatMessage(message, channel), context);
    }

    /// <summary>
    ///     Logs a formatted error message through the project logger.
    /// </summary>
    /// <param name="format">Composite format string.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    /// <param name="args">Values used to format <paramref name="format" />.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogErrorFormat(string format, LogChannel channel = LogChannel.GENERAL, params object[] args)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogErrorFormat(FormatFormat(format, channel), args);
    }

    /// <summary>
    ///     Logs a formatted error message with a Unity context object.
    /// </summary>
    /// <param name="context">Unity object linked to the log entry.</param>
    /// <param name="format">Composite format string.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    /// <param name="args">Values used to format <paramref name="format" />.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogErrorFormat(Object context, string format, LogChannel channel = LogChannel.GENERAL,
        params object[] args)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogErrorFormat(context, FormatFormat(format, channel), args);
    }

    /// <summary>
    ///     Logs an assertion message through the project logger.
    /// </summary>
    /// <param name="message">Assertion text to write.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogAssertion(string message, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogAssertion(FormatMessage(message, channel));
    }

    /// <summary>
    ///     Logs an assertion message with a Unity context object.
    /// </summary>
    /// <param name="message">Assertion text to write.</param>
    /// <param name="context">Unity object linked to the log entry.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogAssertion(string message, Object context, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogAssertion(FormatMessage(message, channel), context);
    }

    /// <summary>
    ///     Logs a formatted assertion message through the project logger.
    /// </summary>
    /// <param name="format">Composite format string.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    /// <param name="args">Values used to format <paramref name="format" />.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogAssertionFormat(string format, LogChannel channel = LogChannel.GENERAL, params object[] args)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogAssertionFormat(FormatFormat(format, channel), args);
    }

    /// <summary>
    ///     Logs a formatted assertion message with a Unity context object.
    /// </summary>
    /// <param name="context">Unity object linked to the log entry.</param>
    /// <param name="format">Composite format string.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    /// <param name="args">Values used to format <paramref name="format" />.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogAssertionFormat(Object context, string format, LogChannel channel = LogChannel.GENERAL,
        params object[] args)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogAssertionFormat(context, FormatFormat(format, channel), args);
    }

    /// <summary>
    ///     Logs an exception through the project logger.
    /// </summary>
    /// <param name="exception">Exception Instance to write.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogException(Exception exception, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogException(PrepareException(exception, channel));
    }

    /// <summary>
    ///     Logs an exception with a Unity context object.
    /// </summary>
    /// <param name="exception">Exception Instance to write.</param>
    /// <param name="context">Unity object linked to the log entry.</param>
    /// <param name="channel">Channel used for filtering and optional channel-prefix display.</param>
    [Conditional("ENABLE_GAME_LOGS")]
    [Conditional("UNITY_EDITOR")]
    public static void LogException(Exception exception, Object context, LogChannel channel = LogChannel.GENERAL)
    {
        if (!IsChannelEnabled(channel)) return;
        Debug.LogException(PrepareException(exception, channel), context);
    }

    private static string FormatMessage(string message, LogChannel channel)
    {
        return Config.Config.ShowChannelNameInLog
            ? $"[{channel}] {message}"
            : message;
    }

    private static string FormatFormat(string format, LogChannel channel)
    {
        return Config.Config.ShowChannelNameInLog
            ? $"[{channel}] {format}"
            : format;
    }

    private static Exception PrepareException(Exception exception, LogChannel channel)
    {
        if (!Config.Config.ShowChannelNameInLog || exception == null) return exception;

        return new Exception($"[{channel}] {exception.Message}", exception);
    }

    private static void SetConfigOnce(LoggerController config)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        if (_config != null)
            throw new InvalidOperationException("LoggerConfiguration was already set and cannot be changed.");

        _config = config;
        Log("Logger initialized with config: " + config.name);
    }

    private static bool IsChannelEnabled(LogChannel channel)
    {
        var isEnabled = true; // if channel not in list, consider it enabled.
        if (_config.Config.EnabledChannels.ContainsKey(channel)) isEnabled = _config.Config.EnabledChannels[channel];
#if UNITY_EDITOR
        return isEnabled && _config.Config.EnableInEditor;
#elif DEVELOPMENT_BUILD
        return isEnabled && _config.Config.EnableInDevelopmentBuild;
#else
        return isEnabled && _config.Config.EnableInBuild;
#endif
    }
}