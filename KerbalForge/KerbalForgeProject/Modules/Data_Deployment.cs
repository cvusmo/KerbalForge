using KSP.Modules;
using KSP.Sim.Definitions;
using UnityEngine;

namespace KerbalForge.Modules
{
    [Serializable]
    public class Data_Deployment : ModuleData
    {
        public override Type ModuleType
        {
            get
            {
                return typeof(Module_Deployable);
            }
        }

        [LocalizedField("Deploy Heat Shield")]
        [Tooltip("Current Heat Shield State")]
        public ModuleProperty<bool> isDeployed = new ModuleProperty<bool>(false, false);
    }
}