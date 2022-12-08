using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace GPUGemsFastFluid.Simple
{
    public class GPUParticleForDebug : MonoBehaviour
    {
        public struct ParticleData
        {
            public Vector3 Velocity;
            public Vector3 Position;
            public float   Age;
        };

        public int NUM_PARTICLES = 32768;

        const int NUM_THREADS_X = 256;
        const int NUM_THREADS_Y = 1;
        const int NUM_THREADS_Z = 1;

        public float Throttle = 1.0f;

        public Color ParticleColor = Color.white;
        public Texture2D ParticleTexture = null;
        public float ParticleSize = 0.025f;

        public float LifeTimeMin = 1.0f;
        public float LifeTimeMax = 5.0f;

        public float AreaXMin = -5.0f;
        public float AreaXMax = 5.0f;
        public float AreaYMin = -5.0f;
        public float AreaYMax = 5.0f;

        public float AccelerationWeight = 200.0f;

        public Camera RenderCam = null;

        [SerializeField]
        ComputeShader _kernelCS;
        [SerializeField]
        Shader _renderShader;

        ComputeBuffer _particleDataBuffer;
        Material _renderMat;

        public Simulation.DoubleVelocityTexture2D velocity = null;

        #region MonoBehaviour Functions
        void Start()
        {
            InitResources();
        }

        void Update()
        {
            UpdateResources();
        }

        void OnDestroy()
        {
            if (_particleDataBuffer != null)
            {
                _particleDataBuffer.Release();
                _particleDataBuffer = null;
            }

            if (_renderMat != null)
            {
                if (Application.isEditor)
                {
                    Material.DestroyImmediate(_renderMat);
                }
                else
                {
                    Material.Destroy(_renderMat);
                }
            }
        }

        void OnRenderObject()
        {
            if (_renderMat == null)
            {
                _renderMat = new Material(_renderShader);
                _renderMat.hideFlags = HideFlags.DontSave;
            }

            if (_renderMat == null)
                return;

            if (RenderCam != null)
            {
                // 逆ビュー行列を計算
                var inverseViewMatrix = RenderCam.worldToCameraMatrix.inverse;
                _renderMat.SetMatrix("_InvViewMatrix", inverseViewMatrix);
            }

            _renderMat.SetPass(0);
            _renderMat.SetColor("_Color", ParticleColor);
            _renderMat.SetTexture("_MainTex", ParticleTexture);
            _renderMat.SetFloat("_ParticleSize", ParticleSize);
            _renderMat.SetBuffer("_ParticleDataBuffer", _particleDataBuffer);
            Graphics.DrawProceduralNow(MeshTopology.Points, NUM_PARTICLES);
        }
        #endregion

        #region Public Functions
        #endregion

        #region Private Functions
        void InitResources()
        {
            _particleDataBuffer = new ComputeBuffer(NUM_PARTICLES, Marshal.SizeOf(typeof(ParticleData)));
            var pData = new ParticleData[NUM_PARTICLES];
            for (var i = 0; i < pData.Length; i++)
            {
                pData[i].Velocity = Vector3.zero;
                pData[i].Position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0);
                pData[i].Age = Random.Range(LifeTimeMin, LifeTimeMax);
            }
            _particleDataBuffer.SetData(pData);
        }

        void UpdateResources()
        {
            int threadGroupsX = Mathf.CeilToInt((float)NUM_PARTICLES / NUM_THREADS_X);
            _kernelCS.SetBuffer(0, "_ParticleDataBuffer", _particleDataBuffer);

            _kernelCS.SetFloat("_DeltaTime", Time.deltaTime);
            _kernelCS.SetFloat("_Time", Time.time);
            _kernelCS.SetFloat("_AccelerationWeight", AccelerationWeight);

            var min = this.transform.localPosition - 0.5f * this.transform.localScale;
            var max = this.transform.localPosition + 0.5f * this.transform.localScale;
            _kernelCS.SetVector("_SimMin", new Vector4(min.x, min.y, min.z));
            _kernelCS.SetVector("_SimMax", new Vector4(max.x, max.y, max.z));

            _kernelCS.SetVector("_LifeTimeParams", new Vector2(1.0f / LifeTimeMin, 1.0f / LifeTimeMax));

            if (velocity != null)
            {
                _kernelCS.SetTexture(0, "_FluidVelocityMap", velocity.Read.Data);
                _kernelCS.SetVector("_FluidVelocityMapResolution", new Vector2(velocity.Read.Data.width, velocity.Write.Data.height));
            }

            _kernelCS.Dispatch(0, threadGroupsX, 1, 1);
        }
        #endregion
    }
}