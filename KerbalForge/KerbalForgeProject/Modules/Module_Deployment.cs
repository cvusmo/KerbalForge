using I2.Loc;
using KSP.Modules;
using UnityEngine;
using KerbalForge;

namespace KerbalForge.Modules
{
    [DisallowMultipleComponent]
    public class Module_Deployment : Module_Deployable
    {
        [SerializeField]
        private Data_Deployment _dataDeployment = new Data_Deployment();
        public bool isDeployed
        {
            get => _dataDeployment.isDeployed.GetValue();
            set
            {
                if (value != isDeployed)
                {
                    _dataDeployment.isDeployed.SetValue(value);
                    OnDeployedStateChanged(value);
                    Debug.Log($"IsDeployed has been set to: {value}");
                }
            }
        }
        string ToStringDelegate(object obj) => isDeployed ? "Deployed" : "Retracted";
        public override Type PartComponentModuleType => typeof(PartComponentModule_Deployment);
        public override void OnInitialize()
        {
            KerbalForgePlugin.Instance._logger.LogInfo("Initializing Module_Deployment...");
            base.OnInitialize();

            InitializeComponents();
            RegisterActions();
            UpdateDeployment();
            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized!");
        }
        private void InitializeComponents()
        {
            this.animator = GetComponentInChildren<Animator>(true);
            if (this.animator == null)
            {
                KerbalForgePlugin.Instance._logger.LogError("Deployable is null");
                return;
            }

            this.animator = GetComponentInChildren<Animator>(true);
            _dataDeployment.isDeployed.OnChangedValue += OnDeployedStateChanged;
            SetDeploymentActiveState(_dataDeployment.isDeployed.GetValue());
            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized Coomponents");
        }
        private void RegisterActions()
        {
            KerbalForgePlugin.Instance._logger.LogInfo("Registering actions for Heat Shield...");

            AddActionGroupAction(ToggleDeployedState, KSP.Sim.KSPActionGroup.None, "Toggle Heat Shield");
            KerbalForgePlugin.Instance._logger.LogInfo("Registered Toggle Heat Shield action.");

            AddActionGroupAction(() => SetDeploymentActiveState(true), KSP.Sim.KSPActionGroup.None, "Deploy Heat Shield");
            KerbalForgePlugin.Instance._logger.LogInfo("Registered Deploy Heat Shield action.");

            AddActionGroupAction(() => SetDeploymentActiveState(false), KSP.Sim.KSPActionGroup.None, "Retract Heat Shield");
            KerbalForgePlugin.Instance._logger.LogInfo("Registered Retract Heat Shield action.");
        }
        public override void AddDataModules()
        {
            base.AddDataModules();
            this._dataDeployment ??= new Data_Deployment();
            this.DataModules.TryAddUnique<Data_Deployment>(this._dataDeployment, out this._dataDeployment);
        }
        private void SetDeploymentActiveState(bool newState)
        {
            isDeployed = newState;
            UpdateDeployment();
        }
        private void ToggleDeployedState()
        {
            SetDeploymentActiveState(!isDeployed);
        }
        public override string GetModuleDisplayName()
        {
            return LocalizationManager.GetTranslation("PartModules/Deployable/Name");            
        }
        private void OnDeployedStateChanged(bool isdeployed)
        {
            if (this.animator != null)
            {
                this.animator.SetBool("isDeployed", isdeployed);
                KerbalForgePlugin.Instance._logger.LogDebug($"Setting animator 'Deployed' state to: {isdeployed}");
            }
            else
            {
                KerbalForgePlugin.Instance._logger.LogError("Animator is null in OnDeployedStateChanged");
            }
        }
        private void UpdateDeployment()
        {
            if (this.animator != null && this.animator.GetBool("Deployed") != isDeployed)
            {
                this.animator.SetBool("Deployed", isDeployed);
                KerbalForgePlugin.Instance._logger.LogDebug($"Animator 'Deployed' state mismatch detected. Setting to: {isDeployed}");
            }
            else if (this.animator == null)
            {
                KerbalForgePlugin.Instance._logger.LogError("Animator is null in UpdateDeployment");
            }
        }
        public override string ToString()
        {
            return ToStringDelegate(this);
        }
    }
}