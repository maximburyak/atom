using System;
using System.Drawing;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class ImageHelper
	{
		public static Image GenerateThumbnail(Image img, int height, int width)
		{
			double thumbSize = height;
			int newWidth, newHeight;
			if (img.Height > img.Width)
			{
				newHeight = (int)thumbSize;
				newWidth = (int)(img.Width * thumbSize / img.Height);
			}
			else
			{
				newWidth = (int)thumbSize;
				newHeight = (int)(img.Height * thumbSize / img.Width);
			}
			return img.GetThumbnailImage(newWidth, newHeight, null, (IntPtr)null);
		}
	}
}