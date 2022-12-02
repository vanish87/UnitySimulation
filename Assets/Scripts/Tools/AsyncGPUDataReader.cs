using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Simulation
{
    public class AsyncGPUDataReader : MonoBehaviour
    {
        [System.Serializable]
        public class FrameData
        {
            public AsyncGPUReadbackRequest readback;
        }

        protected Queue<FrameData> frameQueue = new Queue<FrameData>();

        public void QueueTexture(Texture source)
        {
            this.frameQueue.Enqueue(new FrameData() { readback = AsyncGPUReadback.Request(source) });
        }
        public void QueueBuffer(ComputeBuffer source)
        {
            this.frameQueue.Enqueue(new FrameData() { readback = AsyncGPUReadback.Request(source) });
        }

        public void QueueFrame(FrameData frame)
        {
            this.frameQueue.Enqueue(frame);
        }

        public void NoneSequential(Texture source)
        {
            AsyncGPUReadback.Request(source, 0, this.OnSucceededInternal);
        }
        public void NoneSequential(ComputeBuffer source)
        {
            AsyncGPUReadback.Request(source, this.OnSucceededInternal);
        }

        private void OnSucceededInternal(AsyncGPUReadbackRequest readback)
        {
            this.OnSucceeded(new FrameData() { readback = readback });
        }

        protected virtual void OnSucceeded(FrameData frame)
        {
            //get data like this
            //var data = readback.GetData<byte>();
        }
        protected void ProcessQueue()
        {
            while (this.frameQueue.Count > 0)
            {
                var frame = this.frameQueue.Peek();

                // Edit mode: Wait for readback completion every frame.
                if (!Application.isPlaying) frame.readback.WaitForCompletion();

                // Skip error frames.
                if (frame.readback.hasError)
                {
                    Debug.LogWarning("GPU readback error was detected.");
                    this.frameQueue.Dequeue();
                    continue;
                }

                // Break when found a frame that hasn't been read back yet.
                if (!frame.readback.done) break;

                // Feed the frame data to the sender. It encodes/sends the
                // frame asynchronously.
                this.OnSucceeded(frame);
                // Done. Remove the frame from the queue.
                this.frameQueue.Dequeue();
            }
        }
        protected virtual void Update()
        {
            this.ProcessQueue();
        }
    }
}
