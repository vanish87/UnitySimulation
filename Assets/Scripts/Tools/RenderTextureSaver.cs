
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Simulation
{
    public class RenderTextureSaver : AsyncGPUDataReader
    {
        public class RenderTextureFrame : FrameData
        {
            public int frameCount;
            public GraphicsFormat format;
        }

        public ConcurrentQueue<SaveData> saveQueue = new ConcurrentQueue<SaveData>();

        public class SaveData
        {
            public NativeArray<byte> data;
            public GraphicsFormat format;
            public int w;
            public int h;
            public string name;
        }
        protected override void OnSucceeded(FrameData frame)
        {
            var rtframe = frame as RenderTextureFrame;

            var data = rtframe.readback.GetData<byte>();
            var path = Path.Combine(Application.streamingAssetsPath, "../../../Out/SavedScreen" + rtframe.frameCount + ".png");
            this.saveQueue.Enqueue(new SaveData() { data = data, format = rtframe.format, w = rtframe.readback.width, h = rtframe.readback.height, name = path });
        }

        CancellationTokenSource t;

        protected void OnEnable()
        {
            Camera.main.targetTexture = new RenderTexture(new RenderTextureDescriptor(1024, 1024));

            t = new CancellationTokenSource();
            Task.Run(()=>
            {
                while(true)
                {
                    var d = default(SaveData);
                    if(this.saveQueue.TryDequeue(out d))
                    {
                        var bytes = ImageConversion.EncodeNativeArrayToPNG(d.data, d.format, (uint)d.w, (uint)d.h);
                        File.WriteAllBytesAsync(d.name, bytes.ToArray());
                        Thread.Sleep(10);
                    }

                    if (t.Token.IsCancellationRequested) break;
                }
            }, t.Token);
        }
        protected void OnDisable()
        {
            var tex = Camera.main.targetTexture;
            Camera.main.targetTexture = null;
            GameObject.Destroy(tex);

            t.Cancel();
            t.Dispose();
        }

        protected override void Update()
        {
            base.Update();

            var tex = Camera.main.targetTexture;
            this.QueueFrame(new RenderTextureFrame() { frameCount = Time.frameCount, format = tex.graphicsFormat, readback = AsyncGPUReadback.Request(tex) });
        }

    }
}