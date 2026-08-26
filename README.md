# Unity-Custom-Logger
A custom Logger script for Unity 6.
 <br/>
Allows you to control what is logged via enabling and disabling custom Log Channels. Log 
Channels are defined in LogChannel.cs. They can be enabled/disabled via the "DefaultLoggerController" ScriptableObject in the 
Resources folder. (If you do not have it, create it via Assets -> Create -> Logger -> DefaultLoggerController).
 <br/>
Example usage: <code>Logger.Log("Hello, World!", LogChannel.GENERAL);</code> <br/>
The logs are stripped out of builds automatically. To keep the logs in the build, define a symbol named
 <code>"ENABLE_GAME_LOGS"</code>
 
