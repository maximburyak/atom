using System;

namespace Atom.Areas.Suppliers.Domain.Models
{
	public class SupplierCategoryMappingV2Powerplay
	{
		public virtual int Id { get; set; }
		public virtual string Category { get; set; }
		public virtual string Format { get; set; }
		public virtual string FormatType { get; set; }
		public virtual string Class { get; set; }
		public virtual DateTime CreateDate { get; set; }
		public virtual string CreatedBy { get; set; }

		public virtual void SetDefaults(string user)
		{
			if (string.IsNullOrWhiteSpace(Category))
				throw new ArgumentNullException();

			CreatedBy = user;
			CreateDate = DateTime.Now;
			Format = Format ?? string.Empty;
			FormatType = FormatType ?? string.Empty;
			Class = Class ?? string.Empty;
		}
	}
}