using UnityEngine;

namespace Simulation.Fluid.SPH
{
    public class SPHConfigure : MonoBehaviour, IConfigure
    {
        [System.Serializable]
        public class Data
        {
            public float smoothLength = 0.3f;
        }
        public bool Inited => true;
        public Data D => this.data;
        [SerializeField] protected Data data = new Data();
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
    }
}