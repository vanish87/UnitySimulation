
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;

namespace Simulation.Fluid
{
    public class SimulationController : MonoBehaviour, ISimulation, ISimulationData
    {
        public virtual ISimulationData SimulationData => this;
        public virtual IEnumerable<IConfigure> Configures => this.configures ??= this.GetComponentsInChildren<IConfigure>();
        public virtual IEnumerable<IData> Data => this.data ??= this.GetComponentsInChildren<IData>();
        public virtual IEnumerable<ISpace> Spaces => this.spaces ??= this.GetComponentsInChildren<ISpace>();
        public virtual IEnumerable<IPlugin> Plugins => this.plugins ??= this.GetComponentsInChildren<IPlugin>();
        public bool Inited => this.inited;
        public virtual IEnumerable<KeyValuePair<int, List<IPlugin>>> SortedPlugins => this.sortedPlugins.OrderBy(k => k.Key);
        [SerializeField] protected AccumulatorTimestep accumulator = new AccumulatorTimestep();
        protected Dictionary<int, List<IPlugin>> sortedPlugins = new Dictionary<int, List<IPlugin>>();
        protected IEnumerable<IPlugin> plugins;
        protected IEnumerable<IConfigure> configures;
        protected IEnumerable<ISpace> spaces;
        protected IEnumerable<IData> data;
        protected bool inited = false;
        public virtual void Init(params object[] parameter)
        {
            if (this.Inited) return;

            foreach (var c in this.Configures)
            {
                if (!c.Inited) c.Init(parameter, this, this.Configures, this.Spaces, this.Data, this.Plugins);
            }
            foreach (var s in this.Spaces)
            {
                if (!s.Inited) s.Init(parameter, this, this.Configures, this.Spaces, this.Data, this.Plugins);
            }
            foreach (var d in this.Data)
            {
                if (!d.Inited) d.Init(parameter, this, this.Configures, this.Spaces, this.Data, this.Plugins);
            }

            this.sortedPlugins.Clear();
            foreach (var p in this.Plugins)
            {
                if (!p.Inited) p.Init(parameter, this, this.Configures, this.Spaces, this.Data, this.Plugins);

                foreach (var s in p.Steps)
                {
                    if (!this.sortedPlugins.ContainsKey(s)) this.sortedPlugins.Add(s, new List<IPlugin>());
                    this.sortedPlugins[s].Add(p);
                }
            }
            this.inited = true;
        }

        public virtual void Deinit(params object[] parameter)
        {
            foreach (var p in this.Plugins)
            {
                p.Deinit(parameter, this, this.Configures, this.Spaces, this.Data, this.Plugins);
            }
            foreach (var d in this.Data)
            {
                d.Deinit(parameter, this, this.Configures, this.Spaces, this.Data, this.Plugins);
            }
            foreach (var s in this.Spaces)
            {
                s.Deinit(parameter, this, this.Configures, this.Spaces, this.Data, this.Plugins);
            }
            foreach (var c in this.Configures)
            {
                c.Deinit(parameter, this, this.Configures, this.Spaces, this.Data, this.Plugins);
            }
            this.inited = false;
        }
        public virtual void SimulationStep()
        {
            foreach (var l in this.SortedPlugins)
            {
                foreach (var p in l.Value)
                {
                    if (p.Enabled) p.OnSimulationStep(l.Key, this, this);
                }
            }
        }

        protected void OnUpdate(float time)
        {

        }
        protected virtual void OnEnable()
        {
            this.Init();
            this.accumulator.OnUpdate(dt => this.OnUpdate(dt));
        }
        protected virtual void OnDisable()
        {
            this.Deinit();
        }
        protected virtual void Update()
        {
            this.SimulationStep();
        }
    }
}