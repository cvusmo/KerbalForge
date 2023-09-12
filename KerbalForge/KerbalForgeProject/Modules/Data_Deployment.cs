using KSP.Modules;
using KSP.Sim.Definitions;
using System;

namespace KerbalForge.Modules
{
    [Serializable]
    public class Data_Deployment : ModuleData
    {
        private Data_Deployable deployableData = new Data_Deployable();

        public override Type ModuleType => typeof(Module_Deployable);

        public event Action<bool> OnToggleExtendChanged;
        public event Action<Data_Deployable.DeployState> OnCurrentDeployStateChanged;

        public Data_Deployable.DeployState CurrentState
        {
            get => deployableData.CurrentDeployState.GetValue();
            set
            {
                if (deployableData.CurrentDeployState.GetValue() != value)
                {
                    deployableData.CurrentDeployState.SetValue(value);
                    OnCurrentDeployStateChanged?.Invoke(value);
                }
            }
        }

        public bool ToggleExtend
        {
            get => deployableData.toggleExtend.GetValue();
            set
            {
                if (deployableData.toggleExtend.GetValue() != value)
                {
                    deployableData.toggleExtend.SetValue(value);
                    OnToggleExtendChanged?.Invoke(value);
                }
            }
        }

        public string AnimDeployStateKey => deployableData.AnimDeployStateKey;

        public string AnimReverseStateTransitionTriggerKey => deployableData.AnimReverseStateTransitionTriggerKey;

        public string AnimSpeedMultiplierKey => deployableData.AnimSpeedMultiplierKey;

        public float DefaultExtendedAnimSpeedValue => deployableData.DefaultExtendedAnimSpeedValue;

        public float DefaultRetractedAnimSpeedValue => deployableData.DefaultRetractedAnimSpeedValue;

        public bool IsExtended => CurrentState == Data_Deployable.DeployState.Extended;

        public bool IsRetracted => CurrentState == Data_Deployable.DeployState.Retracted;

        public bool IsRetracting => CurrentState == Data_Deployable.DeployState.Retracting;

        public bool IsExtending => CurrentState == Data_Deployable.DeployState.Extending;
    }
}