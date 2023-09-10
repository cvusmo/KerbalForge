using KSP.Sim.impl;

namespace KerbalForge.Modules
{
    public class PartComponentModule_Deployment : PartComponentModule_Deployable
    {
        private Data_Deployment _dataDeployment;
        public override Type PartBehaviourModuleType
        {
            get
            {
                return typeof(PartComponentModule_Deployment);
            }
        }

        public override void OnStart(double universalTime)
        {
            if (!this.DataModules.TryGetByType<Data_Deployment>(out this._dataDeployment))
            {
                KerbalForgePlugin.Instance.SWLogger.LogInfo("Unable to find a Data_Deployable in the PartComponentModule for " + base.Part.PartName);
                return;
            }
        }

        public override void OnUpdate(double universalTime, double deltaUniversalTime)
        {
        }
    }
}