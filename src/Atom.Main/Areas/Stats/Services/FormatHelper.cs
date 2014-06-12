using System;

namespace Atom.Main.Areas.Stats.Services
{
	public static class FormatHelper
	{
		public static string TryFormatObject(object obj)
                       {
							var str = obj.ToString();
							if (obj.GetType() == typeof(DateTime))
							{
								var dateTime = ((DateTime) obj);
								str = dateTime == default(DateTime)
												? "Never"
												: dateTime.ToString("dd/MM/yyyy");
							}
                       	return str;
                       }

		public static string IdentifyClass(object value)
		{
			var textValue = value.ToString();
			var dateTime = new DateTime();
			double number = 0;

			return
				DateTime.TryParse(textValue, out dateTime)?	"DateTime": 
				double.TryParse(textValue, out number)?		"Number":
															"";

		}
	}
}