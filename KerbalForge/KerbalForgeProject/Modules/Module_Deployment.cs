using KerbalForge.Modules;
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

        enum DeploymentState
        {
            Extended,
            Retracted,
            Extending,
            Retracting
        }

        #region Properties      
        public bool IsRetracted
        {
            get => _dataDeployment.IsRetracted;
            set => SetDeploymentState(value, DeploymentState.Retracted);
        }
        public bool IsExtending
        {
            get => _dataDeployment.IsExtending;
            set
            {
                if (value)
                {
                    SetDeploymentState(true, DeploymentState.Extending);
                }
            }
        }
        public bool IsExtended
        {
            get => _dataDeployment.IsExtended;
            set => SetDeploymentState(value, DeploymentState.Extended);
        }
        public bool IsRetracting
        {
            get => _dataDeployment.IsRetracting;
            set
            {
                if (value)
                {
                    SetDeploymentState(true, DeploymentState.Retracting);
                }
            }
        }

        #endregion
        public override void OnInitialize()
        {
            KerbalForgePlugin.Instance._logger.LogInfo("Initializing Module_Deployment...");
            InitializeComponents();
            base.OnInitialize();
            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized!");
        }
        private void InitializeComponents()
        {
            animator = GetComponentInChildren<Animator>(true);
            LogAnimatorStatus();
            UpdateDeploymentStatesFromData();
            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized Components");
        }
        public override void OnToggleExtendChanged(bool newToggleExtendValue)
        {
            base.OnToggleExtendChanged(newToggleExtendValue);
            if (newToggleExtendValue)
                Extend();
            else
                Retract();
        }
        private void UpdateDeploymentStatesFromData()
        {
            IsRetracted = _dataDeployment.IsRetracted;
            IsExtended = _dataDeployment.IsExtended;
            IsRetracting = _dataDeployment.IsRetracting;
            IsExtending = _dataDeployment.IsExtending;
        }
        private void SetDeploymentState(bool value, DeploymentState state)
        {
            switch (state)
            {
                case DeploymentState.Extended:
                    if (value)
                    {
                        animator?.SetTrigger("isExtending");
                        animator?.SetBool("isExtended", true);
                        animator?.SetBool("isRetracted", false);
                    }
                    else
                    {
                        animator?.SetTrigger("isRetracting");
                        animator?.SetBool("isRetracted", true);
                        animator?.SetBool("isExtended", false);
                    }
                    break;
                case DeploymentState.Retracting:
                    animator?.SetTrigger("isRetracting");
                    animator?.SetBool("isRetracted", true);
                    animator?.SetBool("isExtended", false);
                    break;
                case DeploymentState.Extending:
                    animator?.SetTrigger("isExtending");
                    animator?.SetBool("isExtended", true);
                    animator?.SetBool("isRetracted", false);
                    break;
            }

            UpdateDeploymentStatesFromData();
            LogAnimatorStatus();

            if (IsExtended)
                Extend();
            else
                Retract();

            OnToggleExtendChanged(IsExtended);

            KerbalForgePlugin.Instance._logger.LogInfo($"{state} has been set to: {value}");
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
            return IsExtended ? "isExtended" : "isRetracted";
        }
        private void OnDestroy()
        {
            _dataDeployment.OnToggleExtendChanged -= OnToggleExtendChanged;
            _dataDeployment.OnCurrentDeployStateChanged -= OnCurrentDeployStateChanged;
        }
    }
}