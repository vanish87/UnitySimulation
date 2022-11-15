using System.Runtime.InteropServices;
using UnityEngine;

namespace Simulation
{
	public class BitonicSort<T> : MonoBehaviour
	{
		protected const int BITONIC_BLOCK_SIZE = 1024;
		protected const int TRANSPOSE_BLOCK_SIZE = 32;
		[SerializeField] protected ComputeShader bitonicCS;
		protected ComputeBuffer tempBuffer;
		public void Sort(ref ComputeBuffer source)
		{
			// LogTool.LogAssertIsTrue(Mathf.IsPowerOfTwo(source.Size), "num of source should be power of 2");
			// LogTool.LogAssertIsTrue(source.Size >= BITONIC_BLOCK_SIZE * TRANSPOSE_BLOCK_SIZE, "source size must bigger than " + (BITONIC_BLOCK_SIZE * TRANSPOSE_BLOCK_SIZE));

			if (this.tempBuffer == null || this.tempBuffer.count != source.count)
			{
				this.tempBuffer?.Release();
				this.tempBuffer = new ComputeBuffer(source.count, Marshal.SizeOf<T>());
			}
			ComputeShader sortCS = this.bitonicCS;

			int KERNEL_ID_BITONIC_SORT = sortCS.FindKernel("BitonicSort");
			int KERNEL_ID_TRANSPOSE = sortCS.FindKernel("MatrixTranspose");

			uint NUM_ELEMENTS = (uint)source.count;
			uint MATRIX_WIDTH = BITONIC_BLOCK_SIZE;
			uint MATRIX_HEIGHT = (uint)NUM_ELEMENTS / BITONIC_BLOCK_SIZE;

			for (uint level = 2; level <= BITONIC_BLOCK_SIZE; level <<= 1)
			{
				SetGPUSortConstants(sortCS, level, level, MATRIX_HEIGHT, MATRIX_WIDTH);

				// Sort the row data
				sortCS.SetBuffer(KERNEL_ID_BITONIC_SORT, "Data", source);
				sortCS.Dispatch(KERNEL_ID_BITONIC_SORT, (int)(NUM_ELEMENTS / BITONIC_BLOCK_SIZE), 1, 1);
			}

			// Then sort the rows and columns for the levels > than the block size
			// Transpose. Sort the Columns. Transpose. Sort the Rows.
			for (uint level = (BITONIC_BLOCK_SIZE << 1); level <= NUM_ELEMENTS; level <<= 1)
			{
				// Transpose the data from buffer 1 into buffer 2
				SetGPUSortConstants(sortCS, level / BITONIC_BLOCK_SIZE, (level & ~NUM_ELEMENTS) / BITONIC_BLOCK_SIZE, MATRIX_WIDTH, MATRIX_HEIGHT);
				sortCS.SetBuffer(KERNEL_ID_TRANSPOSE, "Input", source);
				sortCS.SetBuffer(KERNEL_ID_TRANSPOSE, "Data", tempBuffer);
				sortCS.Dispatch(KERNEL_ID_TRANSPOSE, (int)(MATRIX_WIDTH / TRANSPOSE_BLOCK_SIZE), (int)(MATRIX_HEIGHT / TRANSPOSE_BLOCK_SIZE), 1);

				// Sort the transposed column data
				sortCS.SetBuffer(KERNEL_ID_BITONIC_SORT, "Data", tempBuffer);
				sortCS.Dispatch(KERNEL_ID_BITONIC_SORT, (int)(NUM_ELEMENTS / BITONIC_BLOCK_SIZE), 1, 1);

				// Transpose the data from buffer 2 back into buffer 1
				SetGPUSortConstants(sortCS, BITONIC_BLOCK_SIZE, level, MATRIX_HEIGHT, MATRIX_WIDTH);
				sortCS.SetBuffer(KERNEL_ID_TRANSPOSE, "Input", tempBuffer);
				sortCS.SetBuffer(KERNEL_ID_TRANSPOSE, "Data", source);
				sortCS.Dispatch(KERNEL_ID_TRANSPOSE, (int)(MATRIX_HEIGHT / TRANSPOSE_BLOCK_SIZE), (int)(MATRIX_WIDTH / TRANSPOSE_BLOCK_SIZE), 1);

				// Sort the row data
				sortCS.SetBuffer(KERNEL_ID_BITONIC_SORT, "Data", source);
				sortCS.Dispatch(KERNEL_ID_BITONIC_SORT, (int)(NUM_ELEMENTS / BITONIC_BLOCK_SIZE), 1, 1);
			}

		}
		protected void SetGPUSortConstants(ComputeShader cs, uint level, uint levelMask, uint width, uint height)
		{
			cs.SetInt("_Level", (int)level);
			cs.SetInt("_LevelMask", (int)levelMask);
			cs.SetInt("_Width", (int)width);
			cs.SetInt("_Height", (int)height);
		}

		protected virtual void OnDestroy()
		{
			this.tempBuffer?.Release();
		}
	}
}
