using KSP.Modules;
using I2.Loc;
using KSP.Game;
using KSP.Logging;
using KSP.Messages;
using KSP.Sim;
using KSP.Sim.Definitions;
using KSP.Sim.impl;
using System;
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
        public DeployState State
        {
            get => _dataDeployment.CurrentState;
            set
            {
                UpdateDeploymentState(value);
                UpdateAnimatorState(value);
                KerbalForgePlugin.Instance._logger.LogInfo($"DeploymentState has been set to: {value}");
            }
        }
        public override void OnInitialize()
        {
            KerbalForgePlugin.Instance._logger.LogInfo("Initializing Module_Deployment...");
            InitializeComponents();
            base.OnInitialize();
            OnInitializeVisuals();
            OnInitializeDrag();
            _dataDeployment.OnToggleExtendChanged += OnToggleExtendChanged;
            _dataDeployment.OnCurrentDeployStateChanged += OnCurrentDeployStateChanged;
            KerbalForgePlugin.Instance._logger.LogInfo("Module_Deployment Initialized!");
        }
        private void InitializeComponents()
        {
            animator = GetComponentInChildren<Animator>(true);
            LogAnimatorStatus();
        }
        public void ToggleExtension()
        {
            State = (State == DeployState.Extended) ? DeployState.Retracted : DeployState.Extended;
        }
        void UpdateDeploymentState(DeployState newState)
        {
            _dataDeployment.CurrentState = newState;
            OnCurrentDeployStateChanged(newState);
        }
        private void UpdateAnimatorState(DeployState state)
        {
            if (animator == null) return;

            switch (state)
            {
                case DeployState.Extending:
                    animator.SetTrigger(_dataDeployment.AnimReverseStateTransitionTriggerKey);
                    animator.SetFloat(_dataDeployment.AnimSpeedMultiplierKey, _dataDeployment.DefaultExtendedAnimSpeedValue);
                    Extend();
                    break;
                case DeployState.Retracting:
                    animator.SetTrigger(_dataDeployment.AnimReverseStateTransitionTriggerKey);
                    animator.SetFloat(_dataDeployment.AnimSpeedMultiplierKey, _dataDeployment.DefaultRetractedAnimSpeedValue);
                    Retract();
                    break;
                case DeployState.Extended:
                    animator.SetBool(_dataDeployment.AnimDeployStateKey, true);
                    break;
                case DeployState.Retracted:
                    animator.SetBool(_dataDeployment.AnimDeployStateKey, false);
                    break;
            }
        }

        public override void OnToggleExtendChanged(bool newToggleExtendValue)
        {
            base.OnToggleExtendChanged(newToggleExtendValue);
            State = newToggleExtendValue ? DeployState.Extended : DeployState.Retracted;
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
            return State.ToString();
        }
    }
}
