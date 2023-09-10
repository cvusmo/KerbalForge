using BepInEx;
using BepInEx.Logging;
using KerbalForge.Modules;
using SpaceWarp;
using SpaceWarp.API.Mods;

namespace KerbalForge
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
    public class KerbalForgePlugin : BaseSpaceWarpPlugin
    {
        public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
        public const string ModName = MyPluginInfo.PLUGIN_NAME;
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

        internal readonly ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("KerbalForgePlugin");

        private Module_Deployment _moduleDeployment;
        internal static KerbalForgePlugin Instance { get; set; }
        public static string Path { get; private set; }
        public KerbalForgePlugin()
        {
            if (Instance != null)
            {
                throw new Exception("KerbalForgePlugin is a singleton and cannot have multiple instances!");
            }
            Instance = this;
        }
        public override void OnPreInitialized()
        {
            Path = this.PluginFolderPath;
            base.OnPreInitialized();
            _logger.LogInfo("OnPreInitialized KerbalForgePlugin.");
        }
        public override void OnInitialized()
        {
            Logger.LogDebug("Kerbal Forge initializing...");
            base.OnInitialized();

            Instance = this;
            _moduleDeployment = new Module_Deployment();

            LogInit();
        }
        private void LogInit()
        {
            Logger.LogDebug("Kerbal Forge initialized successfully");
            Logger.LogDebug("Module_Deployment initialized successfully.");
        }
    }
}