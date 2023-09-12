using I2.Loc;
using KSP.Modules;
using UnityEngine;
using KerbalForge;
using KSP.Utilities;

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
                    dataDeployable.toggleExtend.SetValueInternal(value); 
                    OnDeployedStateChanged(value);
                    Debug.Log($"IsDeployed has been set to: {value}");
                }
            }
        }
        public bool IsRetracted
        {
            get => _dataDeployment.IsRetracted;
            set
            {
                if (value != IsRetracted)
                {
                    _dataDeployment.IsRetracted = value;
                    dataDeployable.toggleExtend.SetValueInternal(value);
                    OnDeployedStateChanged(value);
                    Debug.Log($"IsRetracted has been set to: {value}");
                }
            }
        }
        public bool IsExtended
        {
            get => _dataDeployment.IsExtended;
            set
            {
                if (value != IsExtended)
                {
                    _dataDeployment.IsExtended = value;
                    dataDeployable.toggleExtend.SetValueInternal(value);
                    OnDeployedStateChanged(value);
                    Debug.Log($"IsExtended has been set to: {value}");
                }
            }
        }
        public bool IsRetracting
        {
            get => _dataDeployment.IsRetracting;
            set
            {
                if (value != IsRetracting)
                {
                    _dataDeployment.IsRetracting = value;
                    dataDeployable.toggleExtend.SetValueInternal(value);
                    OnDeployedStateChanged(value);
                    Debug.Log($"IsRetracting has been set to: {value}");
                }
            }
        }
        public bool IsExtending
        {
            get => _dataDeployment.IsExtending;
            set
            {
                if (value != IsExtending)
                {
                    _dataDeployment.IsExtending = value;
                    dataDeployable.toggleExtend.SetValueInternal(value);
                    OnDeployedStateChanged(value);
                    Debug.Log($"IsExtending has been set to: {value}");
                }
            }
        }
        public override void OnInitialize()
        {
            KerbalForgePlugin.Instance._logger.LogInfo("Initializing Module_Deployment...");

            InitializeComponents();

            // Set animator to the initial state
            if (animator != null)
            {
                animator.SetBool("isRetracted", true);
                animator.SetBool("isExtended", false);
            }
            else
            {
                KerbalForgePlugin.Instance._logger.LogError("Animator is null during OnInitialize");
            }

            base.OnInitialize();
            RegisterActions();
            UpdateDeployment();

            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized!");
        }
        private void InitializeComponents()
        {
            this.animator = GetComponentInChildren<Animator>(true);
            if (this.animator == null)
            {
                KerbalForgePlugin.Instance._logger.LogError($"Module_Deployable Animator is null. Name: {this.animator?.gameObject.name ?? "Animator is null"}");
            }
            else
            {
                KerbalForgePlugin.Instance._logger.LogInfo($"Animator successfully fetched. It is attached to GameObject: {this.animator.gameObject.name}.");

            }

            IsRetracted = _dataDeployment.IsRetracted;
            IsExtended = _dataDeployment.IsExtended;
            IsRetracting = _dataDeployment.IsRetracting;
            IsExtending = _dataDeployment.IsExtending;
            IsDeployed = _dataDeployment.IsDeployed;
            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized Components");
        }
        private void RegisterActions()
        {
            KerbalForgePlugin.Instance._logger.LogInfo("Registering actions for Heat Shield...");

            AddActionGroupAction(() => IsDeployed = true, KSP.Sim.KSPActionGroup.None, "Deploy Heat Shield"); 
            KerbalForgePlugin.Instance._logger.LogInfo("Registered Deploy Heat Shield action.");

            AddActionGroupAction(() => IsDeployed = false, KSP.Sim.KSPActionGroup.None, "Retract Heat Shield"); 
            KerbalForgePlugin.Instance._logger.LogInfo("Registered Retract Heat Shield action.");
        }
        public override void OnToggleExtendChanged(bool extended)
        {
            IsDeployed = extended;
        }
        private void OnDeployedStateChanged(bool isDeployed)
        {
            if (this.animator != null)
            {
                if (IsRetracted)
                {
                    this.animator.SetBool("isExtended", false);
                    this.animator.SetBool("isRetracted", true);

                    bool isExtended = this.animator.GetBool("isExtended");
                    bool isRetracted = this.animator.GetBool("isRetracted");

                    KerbalForgePlugin.Instance._logger.LogInfo($"isExtended: " + isExtended);
                    KerbalForgePlugin.Instance._logger.LogInfo($"isRetracted: " + isRetracted);
                }
                else if (IsExtending)
                {
                    this.animator.SetTrigger("isExtending");
                    KerbalForgePlugin.Instance._logger.LogInfo("Set trigger: isExtending");
                }
                else if (IsExtended)
                {
                    this.animator.SetBool("isExtended", true);
                    bool isExtended = this.animator.GetBool("isExtended");
                    KerbalForgePlugin.Instance._logger.LogInfo($"isExtended set to: " + isExtended);
                }
                else if (IsRetracting)
                {
                    this.animator.SetTrigger("isRetracting");
                    KerbalForgePlugin.Instance._logger.LogInfo("Set trigger: isRetracting");
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
                bool isExtended = this.animator.GetBool("isExtended");
                if (isExtended != IsDeployed)
                {
                    this.animator.SetBool("isExtended", IsDeployed);
                    this.animator.SetBool("isRetracted", !IsDeployed);
                    KerbalForgePlugin.Instance._logger.LogDebug($"Animator 'isExtended' or 'isRetracted' state mismatch detected. Setting 'isExtended' to: {IsDeployed}");
                }
            }
            else
            {
                KerbalForgePlugin.Instance._logger.LogError("Animator is null in UpdateDeployment");
            }
        }
        public override string ToString()
        {
            return IsDeployed ? "Deployed" : "Retracted";
        }
    }
}