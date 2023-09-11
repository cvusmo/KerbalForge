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

        public bool IsDeployed
        {
            get => _dataDeployment.IsDeployed;
            set
            {
                if (value != IsDeployed)
                {
                    _dataDeployment.IsDeployed = value;
                    OnDeployedStateChanged(value);
                    Debug.Log($"IsDeployed has been set to: {value}");
                }
            }
        }
        string ToStringDelegate(object obj) => IsDeployed ? "Deployed" : "Retracted";
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
                KerbalForgePlugin.Instance._logger.LogError("Module_Deployable Animator is null");
                return;
            }

            SetDeploymentActiveState(_dataDeployment.IsDeployed);
            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized Components");
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
            IsDeployed = newState;
            UpdateDeployment();
        }
        private void ToggleDeployedState()
        {
            SetDeploymentActiveState(!IsDeployed);
        }
        public override string GetModuleDisplayName()
        {
            return LocalizationManager.GetTranslation("PartModules/Deployable/Name");            
        }
        private void OnDeployedStateChanged(bool isdeployed)
        {
            if (this.animator != null)
            {
                if (isdeployed)
                {
                    // If it's currently retracted, start extending
                    if (this.animator.GetBool("isRetracted"))
                    {
                        this.animator.SetTrigger("isExtending");
                        this.animator.SetBool("isRetracted", false);
                    }
                }
                else
                {
                    // If it's fully extended, start retracting
                    if (this.animator.GetBool("isExtended"))
                    {
                        this.animator.SetTrigger("isRetracting");
                        this.animator.SetBool("isExtended", false);
                    }
                }
            }
            else
            {
                KerbalForgePlugin.Instance._logger.LogError("Animator is null in OnDeployedStateChanged");
            }
        }
        private void UpdateDeployment()
        {
            if (this.animator != null)
            {
                if (this.animator.GetBool("isExtended") != IsDeployed)
                {
                    if (IsDeployed)
                    {
                        this.animator.SetBool("isExtended", true);
                        this.animator.SetBool("isRetracted", false);
                    }
                    else
                    {
                        this.animator.SetBool("isExtended", false);
                        this.animator.SetBool("isRetracted", true);
                    }
                    KerbalForgePlugin.Instance._logger.LogDebug($"Animator 'isExtended' or 'isRetracted' state mismatch detected. Setting 'isExtended' to: {IsDeployed}");
                }
            }
            else
            {
                KerbalForgePlugin.Instance._logger.LogError("Animator is null in UpdateDeployment");
            }
        }
        public void ResetExtendingTrigger()
        {
            if (this.animator != null)
            {
                this.animator.ResetTrigger("isExtending");
            }
        }
        public void ResetRetractingTrigger()
        {
            if (this.animator != null)
            {
                this.animator.ResetTrigger("isRetracting");
            }
        }
        public override string ToString()
        {
            return ToStringDelegate(this);
        }
    }
}