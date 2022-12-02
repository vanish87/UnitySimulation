
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.Tool
{
	public class TextureTool
	{
		public enum TextureLayout
		{
			Auto,
			Horizontal,
			Vertical,
		}
		public static int2 SpaceToPixelSize(ISpace space, int basePixelWidth = 1024)
		{
            return new int2(basePixelWidth, Mathf.CeilToInt(basePixelWidth * (space.Scale.y / space.Scale.x)));
		}
		public static void CalculateTextureSizeAndOffset(List<Texture> input, out int2 totalSize, out List<float4> st, TextureLayout layout = TextureLayout.Auto, int maxLineCount = -1)
		{
			if (input == null || input.Count == 0)
			{
				totalSize = new int2(2, 2);
				st = new List<float4>() { new float4(0, 0, 1, 1) };
				return;
			}

			totalSize = new int2(0, 0);
			st = new List<float4>();

			var maxW = input.Max(c => c.width);
			var maxH = input.Max(c => c.height);
			//use max width or height as total width/height
			//another size of texture is sum up of width/height size
			var horizontal = layout == TextureLayout.Auto ?
							 maxH <= maxW :
							 layout == TextureLayout.Horizontal;

			var compacked = layout != TextureLayout.Auto;

			// var newSize = new Vector2Int(0, 0);
			var currentSize = new Vector2Int(0, 0);

			maxLineCount = maxLineCount != -1 ? maxLineCount : Mathf.CeilToInt(Mathf.Sqrt(input.Count));
			var maxLineSize = (horizontal ? maxW : maxH) * (compacked ? input.Count : maxLineCount);

			foreach (var t in input)
			{
				var offSetX = compacked ? t.width : maxW;
				var offSetY = compacked ? t.height : maxH;
				if (horizontal)
				{
					currentSize.x += offSetX;
					if (currentSize.x > maxLineSize)
					{
						currentSize.x = offSetX;
						currentSize.y += offSetY;
					}
				}
				else
				{
					currentSize.y += offSetY;
					if (currentSize.y > maxLineSize)
					{
						currentSize.y = offSetY;
						currentSize.x += offSetX;
					}
				}
			}

			if (horizontal)
			{
				if (compacked)
				{
					totalSize.x = currentSize.x;
				}
				else
				{
					bool newLine = currentSize.y >= maxH;
					totalSize.x = newLine ? maxLineSize : currentSize.x;
				}
				totalSize.y = currentSize.y + maxH;
			}
			else
			{
				if (compacked)
				{
					totalSize.y = currentSize.y;
				}
				else
				{
					bool newLine = currentSize.x >= maxW;
					totalSize.y = newLine ? maxLineSize : currentSize.y;
				}
				totalSize.x = currentSize.x + maxW;
			}
			//xy is start uv coordinates
			//zw is texture size in uv space
			var currentST = new Vector4(0, 0, 1, 1);
			foreach (var t in input)
			{
				var sizeX = t.width * 1f / totalSize.x;
				var sizeY = t.height * 1f / totalSize.y;

				currentST.z = sizeX;
				currentST.w = sizeY;

				// t.st = currentST;
				st.Add(currentST);

				var offSetX = compacked ? sizeX : (maxW * 1.0f / totalSize.x);
				var offSetY = compacked ? sizeY : (maxH * 1.0f / totalSize.y);

				if (horizontal)
				{
					currentST.x += offSetX;

					if (currentST.x >= maxLineSize * 1.0f / totalSize.x)
					{
						currentST.x = 0;
						currentST.y += offSetY;
					}
				}
				else
				{
					currentST.y += offSetY;
					if (currentST.y >= maxLineSize * 1.0f / totalSize.y)
					{
						currentST.y = 0;
						currentST.x += offSetX;
					}
				}
			}
		}
	}
}