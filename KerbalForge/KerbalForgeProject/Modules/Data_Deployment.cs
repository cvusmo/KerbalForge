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

        // Event handlers to notify state changes
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
        public bool IsExtended
        {
            get => CurrentState == Data_Deployable.DeployState.Extended;
        }
        public bool IsRetracted
        {
            get => CurrentState == Data_Deployable.DeployState.Retracted;
        }
        public bool IsRetracting
        {
            get => CurrentState == Data_Deployable.DeployState.Retracting;
        }
        public bool IsExtending
        {
            get => CurrentState == Data_Deployable.DeployState.Extending;
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
        public void RaiseToggleExtendChanged(bool isExtended)
        {
            OnToggleExtendChanged?.Invoke(isExtended);
        }
        public void RaiseCurrentDeployStateChanged(Data_Deployable.DeployState state)
        {
            OnCurrentDeployStateChanged?.Invoke(state);
        }
    }
}