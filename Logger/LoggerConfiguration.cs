using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLoggerConfiguration", menuName = "Logger/LoggerConfiguration")]
public class LoggerConfiguration : ScriptableObject
{
    [Tooltip("If a channel is not in this list, it is considered enabled.")] [SerializeField]
    private SerializableDictionary<LogChannel, bool> enabledChannels = new();

    [SerializeField] private bool enableInEditor = true;
    [SerializeField] private bool enableInDevelopmentBuild = true;
    [SerializeField] private bool enableInBuild;
    [SerializeField] private bool showChannelNameInLog = true;

    [Header("GLOBAL CONTROLS")]
    [Tooltip("Check to enable all channels, uncheck to disable all channels.")]
    [SerializeField] private bool toggleAllChannels = true;

    [Header("RESET ENABLED CHANNELS")]
    [Tooltip("WARNING: This clears enabledChannels and loads all the enums from LoggerChannel.cs into it.\n" +
             "CLICK WITH CAUTION!.")]
    [SerializeField]
    private bool clearAndReInitializeEnabledChannels = true;

    private bool _lastToggleAllChannels;
    private bool _lastInitializeEnabledChannels;

    public SerializableDictionary<LogChannel, bool> EnabledChannels => enabledChannels;
    public bool EnableInEditor => enableInEditor;
    public bool EnableInDevelopmentBuild => enableInDevelopmentBuild;
    public bool EnableInBuild => enableInBuild;
    public bool ShowChannelNameInLog => showChannelNameInLog;

    private void OnEnable()
    {
        _lastInitializeEnabledChannels = clearAndReInitializeEnabledChannels;
        _lastToggleAllChannels = toggleAllChannels;
    }

    private void OnValidate()
    {
        // Handle Reset Trigger
        if (clearAndReInitializeEnabledChannels != _lastInitializeEnabledChannels)
        {
            _lastInitializeEnabledChannels = clearAndReInitializeEnabledChannels;
            OnInitializeEnabledChannelsChanged();
        }

        // Handle Toggle All Trigger
        if (toggleAllChannels != _lastToggleAllChannels)
        {
            _lastToggleAllChannels = toggleAllChannels;
            OnToggleAllChannelsChanged();
        }
    }

    private void OnInitializeEnabledChannelsChanged()
    {
        enabledChannels.Clear();
        foreach (LogChannel channel in Enum.GetValues(typeof(LogChannel))) 
            enabledChannels[channel] = true;

        // Keep the toggle checkbox synced since initialization sets everything to true
        toggleAllChannels = true;
        _lastToggleAllChannels = true;
    }

    private void OnToggleAllChannelsChanged()
    {
        foreach (LogChannel channel in Enum.GetValues(typeof(LogChannel))) 
        {
            enabledChannels[channel] = toggleAllChannels;
        }
    }
}