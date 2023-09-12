using KSP.Modules;
using KSP.Sim.Definitions;
using UnityEngine;

namespace KerbalForge.Modules
{
    [Serializable]
    public class Data_Deployment : ModuleData
    {
        private Data_Deployable deployableData = new Data_Deployable();

        [LocalizedField("Deploy Heat Shield")]
        [Tooltip("Current Heat Shield State")]
        public ModuleProperty<bool> isDeployedField = new ModuleProperty<bool>(false);

        public override Type ModuleType => typeof(Module_Deployable);

        public bool IsDeployed
        {
            get => isDeployedField.GetValue();
            set => isDeployedField.SetValue(value);
        }
        public bool IsRetracted
        {
            get => isDeployedField.GetValue();
            set => isDeployedField.SetValue(value);
        }
        public bool IsRetracting
        {
            get => isDeployedField.GetValue();
            set => isDeployedField.SetValue(value);
        }
        public bool IsExtended
        {
            get => isDeployedField.GetValue();
            set => isDeployedField.SetValue(value);
        }
        public bool IsExtending
        {
            get => isDeployedField.GetValue();
            set => isDeployedField.SetValue(value);
        }
        public bool toggleExtend
        {
            get => isDeployedField.GetValue();
            set => isDeployedField.SetValue(value);
        }

    }
}