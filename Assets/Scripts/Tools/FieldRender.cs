
using UnityEngine;

namespace Simulation.Tool
{
    public class FieldRender : MonoBehaviour
    {
        protected MeshRenderer MeshRenderer => this.meshRenderer ??= this.GetComponentInChildren<MeshRenderer>();
        protected MeshRenderer meshRenderer;
        protected ITextureField TextureField => this.textureField ??= this.GetComponentInParent<ITextureField>();
        protected ITextureField textureField;
        protected void Update()
        {
            this.MeshRenderer.material.mainTexture = this.TextureField.Texture;
        }
    }
}