using System.Collections.Generic;

namespace Atom.Main.Areas.Suppliers.Models
{
	public abstract class CommonViewModel
	{
		public int CurrentPageIndex { get; set; }
		public int DefaultPageSize { get; set; }
		public int SupplierId { get; set; }
		public string Level1S { get; set; }
		public string Level2S { get; set; }

		public IList<object> DisabledValues
		{
			get
			{
				return new List<object>
				       	{
				       		new {value=true},
				       		new {value=false}
				       	};
			}
		}
		public IList<object> IgnoredValues
		{
			get
			{
				return new List<object>
				       	{
				       		new {value=true},
				       		new {value=false}
				       	};
			}
		}
	}
}