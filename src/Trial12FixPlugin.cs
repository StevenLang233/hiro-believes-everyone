using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Naninovel;
using Naninovel.Commands;

namespace ManosabaTrial12Fix;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class Trial12FixPlugin : BasePlugin
{
    public const string PluginGuid = "org.man.osaba.trial12finalobjectionfix";
    public const string PluginName = "希罗宝宝拯救器 | 让希罗宝宝相信大家";
    public const string PluginVersion = "0.1.0";

    internal static ManualLogSource PluginLog { get; private set; }

    private static IScriptPlayer _scriptPlayer;
    private static ICustomVariableManager _variableManager;
    private static Il2CppSystem.Action _engineInitHandler;
    private static Il2CppSystem.Action<Script> _onPlayHandler;
    private static Il2CppSystem.Action<Command> _onCommandStartHandler;
    private static bool _eventsRegistered;

    public override void Load()
    {
        PluginLog = Log;
        _engineInitHandler = (Il2CppSystem.Action)OnEngineInitialized;

        if (Engine.Initialized)
            OnEngineInitialized();
        else
            Engine.OnInitializationFinished += _engineInitHandler;

        Log.LogInfo($"Loaded {PluginName} v{PluginVersion}; using Naninovel script-player events, no Harmony method detours.");
    }

    private static void OnEngineInitialized()
    {
        if (_eventsRegistered)
            return;

        try
        {
            _scriptPlayer = Engine.GetServiceOrErr<IScriptPlayer>();
            _variableManager = Engine.GetService<ICustomVariableManager>();
            _onPlayHandler = (Il2CppSystem.Action<Script>)OnScriptPlay;
            _onCommandStartHandler = (Il2CppSystem.Action<Command>)OnCommandExecutionStart;
            _scriptPlayer.add_OnPlay(_onPlayHandler);
            _scriptPlayer.add_OnCommandExecutionStart(_onCommandStartHandler);
            _eventsRegistered = true;
            PluginLog?.LogInfo("Registered Naninovel OnPlay and OnCommandExecutionStart handlers.");
        }
        catch (Exception ex)
        {
            PluginLog?.LogError("Could not register Naninovel script-player events: " + ex);
        }
    }

    private static void OnScriptPlay(Script script)
    {
        try
        {
            if (!IsTargetScript(script?.Path))
                return;

            // Use the underlying IL2CPP array rather than the managed IReadOnlyList wrapper.
            var lines = script.lines;
            var found = false;
            for (var i = 0; i < lines.Length; i++)
            {
                var commandLine = lines[i].TryCast<CommandScriptLine>();
                var beginIf = commandLine?.Command.TryCast<BeginIf>();
                if (beginIf is null)
                    continue;

                var expression = (string)beginIf.Expression;
                if (!IsTargetGate(expression))
                    continue;

                found = true;
                PluginLog?.LogInfo($"Found the original Trial 6-12 final gate in the loaded Script object (line slot {i}); expression: {expression}");
            }

            if (!found)
                PluginLog?.LogWarning("Trial 6-12 started, but the expected final gate was not found in Script.Lines.");
        }
        catch (Exception ex)
        {
            PluginLog?.LogError("Could not inspect the loaded Trial 6-12 Script object: " + ex);
        }
    }

    private static void OnCommandExecutionStart(Command command)
    {
        try
        {
            var script = _scriptPlayer?.PlayedScript;
            if (!IsTargetScript(script?.Path))
                return;

            var beginIf = command.TryCast<BeginIf>();
            if (beginIf is null)
                return;

            var expression = (string)beginIf.Expression;
            if (!IsTargetGate(expression))
                return;

            if (_variableManager is null)
                _variableManager = Engine.GetService<ICustomVariableManager>();

            if (_variableManager is null)
            {
                PluginLog?.LogError("Reached the exact Trial 6-12 final gate, but the custom-variable service is unavailable.");
                return;
            }

            var repaired = new List<string>();
            RepairFlag("Skip6_12_3flag1", repaired);
            RepairFlag("Skip6_12_4flag1", repaired);
            RepairFlag("Skip6_12_5flag1", repaired);
            if (repaired.Count > 0)
                PluginLog?.LogWarning("Restored the missing Trial 6-12 gate state at the original command: " + string.Join(", ", repaired));
            else
                PluginLog?.LogInfo("The three Trial 6-12 gate flags were already true; the original condition will run unchanged.");
        }
        catch (Exception ex)
        {
            PluginLog?.LogError("Trial 6-12 final-gate event handler failed safely: " + ex);
        }
    }

    private static void RepairFlag(string name, ICollection<string> repaired)
    {
        var wasTrue = _variableManager!.VariableExists(name) && _variableManager.GetVariableValue(name).Boolean;
        if (wasTrue)
            return;

        _variableManager.SetVariableValue(name, new CustomVariableValue(true));
        if (!_variableManager.VariableExists(name) || !_variableManager.GetVariableValue(name).Boolean)
            throw new InvalidOperationException($"Naninovel did not retain the repaired boolean flag '{name}'.");
        repaired.Add(name);
    }

    private static bool IsTargetScript(string path) =>
        string.Equals(path, "Act02_Chapter01/Act02_Chapter01_Trial12", StringComparison.OrdinalIgnoreCase);

    private static bool IsTargetGate(string expression) =>
        string.Equals(Compact(expression), Compact(ExpectedGate), StringComparison.OrdinalIgnoreCase);

    private static string Compact(string value) =>
        new string((value ?? string.Empty).Where(c => !char.IsWhiteSpace(c)).ToArray());

    private const string ExpectedGate =
        "Skip6_12_3flag1=True&&Skip6_12_4flag1=True&&Skip6_12_5flag1=True";
}
