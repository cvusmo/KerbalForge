using I2.Loc;
using KSP.Modules;
using UnityEngine;

namespace KerbalForge.Modules
{
    [DisallowMultipleComponent]
    public class Module_Deployment : Module_Deployable
    {
        [SerializeField]
        private Data_Deployment _dataDeployment = new Data_Deployment();

        private Module_Deployable deployable;
        public Animator deploymentAnimator;
        public bool IsDeployed
        {
            get => _dataDeployment.IsDeployed.GetValue();
            set
            {
                if (value != IsDeployed)
                {
                    _dataDeployment.IsDeployed.SetValue(value);
                    OnDeployedStateChanged(value);
                }
            }
        }
        string ToStringDelegate(object obj) => IsDeployed ? "Deployed" : "Retracted";
        public override Type PartComponentModuleType => typeof(PartComponentModule_Deployment);
        public override void OnInitialize()
        {
            base.OnInitialize();

            InitializeComponents();
            RegisterActions();
            UpdateDeployment();
        }
        private void InitializeComponents()
        {
            deployable = GetComponent<Module_Deployable>();
            if (deployable == null)
            {
                KerbalForgePlugin.Instance.SWLogger.LogInfo("Deployable is null");
                return;
            }

            deploymentAnimator = GetComponentInChildren<Animator>(true);
            _dataDeployment.IsDeployed.OnChangedValue += OnDeployedStateChanged;
            SetDeploymentActiveState(_dataDeployment.IsDeployed.GetValue());
        }
        private void RegisterActions()
        {
            AddActionGroupAction(ToggleDeployedState, KSP.Sim.KSPActionGroup.None, "Toggle Heat Shield");
            AddActionGroupAction(() => SetDeploymentActiveState(true), KSP.Sim.KSPActionGroup.None, "Deploy Heat Shield");
            AddActionGroupAction(() => SetDeploymentActiveState(false), KSP.Sim.KSPActionGroup.None, "Retract Heat Shield");
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
        private void OnDeployedStateChanged(bool deployed)
        {

            if (deploymentAnimator != null)
            {
                deploymentAnimator.SetBool("Deployed", deployed);
            }
        }
        private void UpdateDeployment()
        {
            if (deploymentAnimator != null && deploymentAnimator.GetBool("Deployed") != IsDeployed)
            {
                deploymentAnimator.SetBool("Deployed", IsDeployed);
            }
        }
        public override string ToString()
        {
            return ToStringDelegate(this);
        }
    }
}