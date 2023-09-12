using KSP.Modules;
using UnityEngine;
using static KSP.Modules.Data_Deployable;

namespace KerbalForge.Modules
{
    [DisallowMultipleComponent]
    public class Module_Deployment : Module_Deployable
    {
        [SerializeField]
        private Data_Deployment _dataDeployment = new Data_Deployment();
        private Animator animator;
        public bool UseAnimation { get; set; } = true;

        #region Properties
        public bool IsDeployed
        {
            get => _dataDeployment.IsDeployed;
            set => SetDeploymentState(value, nameof(IsDeployed));
        }
        public bool IsRetracted
        {
            get => _dataDeployment.IsRetracted;
            set => SetDeploymentState(value, nameof(IsRetracted));
        }
        public bool IsExtended
        {
            get => _dataDeployment.IsDeployed;
            set => SetDeploymentState(value, nameof(IsExtended));
        }
        public bool IsRetracting
        {
            get => _dataDeployment.IsRetracting;
            set
            {
                if (value)
                {
                    SetDeploymentState(true, nameof(IsRetracting));
                }
            }
        }
        public bool IsExtending
        {
            get => _dataDeployment.IsExtending;
            set
            {
                if (value)
                {
                    SetDeploymentState(true, nameof(IsExtending));
                }
            }
        }

        #endregion
        public override void OnInitialize()
        {
            KerbalForgePlugin.Instance._logger.LogInfo("Initializing Module_Deployment...");
            InitializeComponents();
            base.OnInitialize();
            _dataDeployment.OnToggleExtendChanged += OnToggleExtendChanged;
            _dataDeployment.OnCurrentDeployStateChanged += OnCurrentDeployStateChanged;
            RegisterActions();
            UpdateDeployment();
            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized!");
        }
        private void InitializeComponents()
        {
            animator = GetComponentInChildren<Animator>(true);
            LogAnimatorStatus();

            UpdateDeploymentStatesFromData();

            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized Components");
        }
        private void RegisterActions()
        {
            KerbalForgePlugin.Instance._logger.LogInfo("Registering actions for Heat Shield...");
            AddActionGroupAction(() => IsDeployed = true, KSP.Sim.KSPActionGroup.None, "Deploy Heat Shield");
            AddActionGroupAction(() => IsDeployed = false, KSP.Sim.KSPActionGroup.None, "Retract Heat Shield");
        }
        private void UpdateDeployment()
        {
            if (animator != null && UseAnimation)
            {
                animator.SetBool("isExtended", IsDeployed);
                animator.SetBool("isRetracted", !IsDeployed);
                KerbalForgePlugin.Instance._logger.LogDebug($"Animator state mismatch detected. Setting 'isExtended' to: {IsDeployed}");
            }
            else if (animator == null)
            {
                KerbalForgePlugin.Instance._logger.LogError("Animator is null in UpdateDeployment");
            }
        }
        public void ToggleExtension()
        {
            if (IsDeployed)
            {
                SetDeploymentState(false, nameof(IsDeployed));
            }
            else
            {
                SetDeploymentState(true, nameof(IsDeployed));
            }
        }
        private void SetDeploymentState(bool value, string logMsg)
        {
            Data_Deployable.DeployState newState;

            switch (logMsg)
            {
                case nameof(IsDeployed):
                    if (value)
                    {
                        animator?.SetTrigger("isExtending");
                        animator?.SetBool("isExtended", true);
                        animator?.SetBool("isRetracted", false);
                        newState = Data_Deployable.DeployState.Extended;
                    }
                    else
                    {
                        animator?.SetTrigger("isRetracting");
                        animator?.SetBool("isRetracted", true);
                        animator?.SetBool("isExtended", false);
                        newState = Data_Deployable.DeployState.Retracted;
                    }
                    break;
                case nameof(IsRetracting):
                    animator?.SetTrigger("isRetracting");
                    animator?.SetBool("isRetracted", true);
                    animator?.SetBool("isExtended", false);
                    newState = Data_Deployable.DeployState.Retracting;
                    break;
                case nameof(IsExtending):
                    animator?.SetTrigger("isExtending");
                    animator?.SetBool("isExtended", true);
                    animator?.SetBool("isRetracted", false);
                    newState = Data_Deployable.DeployState.Extending;
                    break;
                default:
                    KerbalForgePlugin.Instance._logger.LogError($"Unexpected logMsg received: {logMsg}");
                    newState = Data_Deployable.DeployState.Broken; 
                    break;
            }

            UpdateDeployment();
            UpdateDeployState(newState);

            KerbalForgePlugin.Instance._logger.LogInfo($"{logMsg} has been set to: {value}");
        }
        public override void OnToggleExtendChanged(bool newToggleExtendValue)
        {
            base.OnToggleExtendChanged(newToggleExtendValue);
            IsDeployed = newToggleExtendValue;
            UpdateDeployment();
        }
        void UpdateDeployState(Data_Deployable.DeployState newstate)
        {
            _dataDeployment.CurrentState = newstate;
            switch (newstate)
            {
                case Data_Deployable.DeployState.Retracting:
                    OnCurrentDeployStateChanged(Data_Deployable.DeployState.Retracting);
                    break;
                case Data_Deployable.DeployState.Retracted:
                    OnCurrentDeployStateChanged(Data_Deployable.DeployState.Retracted);
                    break;
                case Data_Deployable.DeployState.Extending:
                    OnCurrentDeployStateChanged(Data_Deployable.DeployState.Extending);
                    break;
                case Data_Deployable.DeployState.Extended:
                    OnCurrentDeployStateChanged(Data_Deployable.DeployState.Extended);
                    break;
                case Data_Deployable.DeployState.Broken:
                    OnCurrentDeployStateChanged(Data_Deployable.DeployState.Broken);
                    break;
                default:
                    break;
            }
        }
        private void UpdateDeploymentStatesFromData()
        {
            IsRetracted = _dataDeployment.IsRetracted;
            IsExtended = _dataDeployment.IsDeployed;
            IsRetracting = _dataDeployment.IsRetracting;
            IsExtending = _dataDeployment.IsExtending;
        }
        private void LogAnimatorStatus()
        {
            if (animator == null)
            {
                KerbalForgePlugin.Instance._logger.LogError("Animator is null during Initialization.");
            }
            else
            {
                KerbalForgePlugin.Instance._logger.LogInfo($"Animator successfully fetched. It is attached to GameObject: {animator.gameObject.name}.");
            }
        }
        public override string ToString()
        {
            return IsDeployed ? "isExtended" : "isRetracted";
        }
        private void OnDestroy()
        {
            _dataDeployment.OnToggleExtendChanged -= OnToggleExtendChanged;
            _dataDeployment.OnCurrentDeployStateChanged -= OnCurrentDeployStateChanged;
        }
    }
}