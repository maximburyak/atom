using System.Collections.Generic;

namespace Atom.Areas.Suppliers.Domain.Models
{
	public class Format
	{
		public Format()
		{
			FormatTypes = new List<Format_FormatType>();
		}
		public virtual string FormatCode { get; set; }
		public virtual string Description { get; set; }
		public virtual string SearchType { get; set; }
		public virtual int ListEnable { get; set; }
		public virtual int NotSearchable { get; set; }
		public virtual int Seq { get; set; }
		public virtual string Col { get; set; }
		public virtual string SuperFmt { get; set; }
		public virtual int Itemised { get; set; }
		public virtual int AllowanceIgnore { get; set; }
		public virtual string Colour { get; set; }
		public virtual IList<Format_FormatType> FormatTypes { get; set; }
		public virtual string FormatDescription
		{
			get
			{
				return string.Format("{0} - [{1}]", Description, FormatCode);
			}
		}
	}
}