using System.Collections.Generic;

namespace Atom.Areas.Suppliers.Domain.Models
{
	public class FormatType
	{
		public FormatType()
		{
			Classes = new List<FormatType_Class>();
		}
		public virtual string FormatTypeCode { get; set; }
		public virtual string Description { get; set; }
		public virtual string Col { get; set; }
		public virtual int ListEnable { get; set; }
		public virtual int NotSearchable { get; set; }
		public virtual int Seq { get; set; }
		public virtual string CategoryCode { get; set; }
		public virtual IList<FormatType_Class> Classes { get; set; }
		public virtual string FormatTypeDescription
		{
			get
			{
				return string.Format("{0} - [{1}]", Description, FormatTypeCode);
			}
		}
	}
}