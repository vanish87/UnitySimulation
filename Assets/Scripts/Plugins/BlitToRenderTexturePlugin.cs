
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulation
{
    public enum ForceStep
    {
        BeforeAddExternalForce,
        AddExternalForce,
        AfterAddExternalForce,
    }
    public class AddExternalForcePlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>()
        {
            (int)SimulationStep.AfterSimulation + (int)ForceStep.AddExternalForce,
        };

        public bool Enabled => throw new System.NotImplementedException();

        public bool Inited => throw new System.NotImplementedException();

        public string Identifier => throw new System.NotImplementedException();

        public void Deinit(params object[] parameter)
        {
            throw new System.NotImplementedException();
        }

        public void Init(params object[] parameter)
        {
            throw new System.NotImplementedException();
        }

        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            throw new System.NotImplementedException();
        }
    }
    public class BlitToRenderTexturePlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>()
        {
            (int)SimulationStep.AfterSimulation + (int)ForceStep.BeforeAddExternalForce,
            (int)SimulationStep.AfterSimulation + (int)ForceStep.AfterAddExternalForce,
        };
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var s = this.Steps.ToList();
            if (stepIndex == s[0])
            {
                //Do BeforeAddExternalForce;
            }
            if (stepIndex == s[1])
            {
                //Do AfterAddExternalForce;
            }
        }

        public bool Enabled => this.isActiveAndEnabled;

        public bool Inited => throw new System.NotImplementedException();

        public string Identifier => throw new System.NotImplementedException();

        public void Deinit(params object[] parameter)
        {
            throw new System.NotImplementedException();
        }

        public void Init(params object[] parameter)
        {
            throw new System.NotImplementedException();
        }

    }
}