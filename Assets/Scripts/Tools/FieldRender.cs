
using UnityEngine;

namespace Simulation.Tool
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class FieldRender : MonoBehaviour
    {
        [SerializeField] protected bool RGBToNormal = false;
        protected MeshRenderer MeshRenderer => this.meshRenderer ??= this.GetComponentInChildren<MeshRenderer>();
        protected MeshRenderer meshRenderer;
        protected ITextureField TextureField => this.textureField ??= this.GetComponentInParent<ITextureField>();
        protected ITextureField textureField;
        [SerializeField] protected Shader shader;
        [SerializeField] protected Material mat;
        protected void OnEnable()
        {
            this.mat = new Material(this.shader);
            this.MeshRenderer.sharedMaterial = this.mat;
        }
        protected void OnDisable()
        {
            GameObject.Destroy(this.mat);
        }
        protected void Update()
        {
            this.mat.mainTexture = this.TextureField.Texture;
            this.mat.SetInt("_RGBToNormal", this.RGBToNormal?1:0);
        }
    }
}