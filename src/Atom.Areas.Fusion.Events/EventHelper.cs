using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Areas.Fusion.Events
{
	public static class EventHelpers
	{
		public static string GetDescriptionOfEnum(this Enum e)
		{
			var da = (DescriptionAttribute[])(e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false));
			return da.Length > 0 ? da[0].Description : e.ToString();
		}

		public static string AdditionalInfoToString(IList<AdditionalInfo> list)
		{
			if (list != null)
			{
				var sb = new StringBuilder();
				foreach (var ai in list)
				{
					sb.Append(ai.InfoType.Description + ": " + ai.Value + Environment.NewLine);
				}
				return sb.ToString();
			}
			return "";
		}
	}
}
