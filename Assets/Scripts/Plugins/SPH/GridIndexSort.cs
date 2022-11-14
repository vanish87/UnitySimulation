using Unity.Mathematics;

namespace Simulation
{
	public class GridIndexSort : BitonicSort<uint2>
	{
		// protected void Start()
		// {
		// 	var buffer = new UnityEngine.ComputeBuffer(512*1024, System.Runtime.InteropServices.Marshal.SizeOf<uint2>());
		// 	var data = new uint2[buffer.count];
		// 	foreach (var i in System.Linq.Enumerable.Range(0, buffer.count))
		// 	{
		// 		data[i].x = data[i].y = (uint)UnityEngine.Random.Range(0, buffer.count * 4);
		// 		// this.source.CPUData[i] = (uint)(this.source.Size - i);
		// 	}

		// 	buffer.SetData(data);
		// 	this.Sort(ref buffer);
		// 	buffer.GetData(data);
			

		// 	foreach (var i in System.Linq.Enumerable.Range(0, buffer.count-1))
		// 	{
		// 		if(data[i].x > data[i+1].x) UnityEngine.Debug.Log("Sort Error");
        //         UnityEngine.Debug.Log(data[i]);
		// 	}

		// 	buffer.Release();
		// }
	}
}