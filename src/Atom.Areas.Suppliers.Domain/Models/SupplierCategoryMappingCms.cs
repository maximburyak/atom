using System;

namespace Atom.Areas.Suppliers.Domain.Models
{
	public class SupplierCategoryMappingCms
	{
		public SupplierCategoryMappingCms()
		{
			CreateDate = DateTime.Now;
		}

		public virtual int SupplierID { get; set; }
		public virtual string Level1 { get; set; }
		public virtual string Level2 { get; set; }
		public virtual string Level3 { get; set; }
		public virtual string Level4 { get; set; }
		public virtual string Format { get; set; }
		public virtual string FormatType { get; set; }
		public virtual string Class { get; set; }
		public virtual bool Disabled { get; set; }
		public virtual bool Ignored { get; set; }
		public virtual DateTime? CreateDate { get; set; }
		public virtual string CreatedBy { get; set; }
		
		public override bool Equals(object obj)
		{
			return obj.GetHashCode().Equals(GetHashCode());
		}
		public override int GetHashCode()
		{
			return HashCode(this);
		}

		private int HashCode(SupplierCategoryMappingCms obj)
		{
			var supplierIdHash = obj.SupplierID.GetHashCode();
			var level1Hash = obj.Level1.GetHashCode();
			var level2Hash = obj.Level2.GetHashCode();
			var level3Hash = obj.Level3.GetHashCode();
			var level4Hash = obj.Level4.GetHashCode();
			var formatHashCode = obj.Format.GetHashCode();
			var formatTypeHashCode = obj.FormatType.GetHashCode();
			return supplierIdHash ^ level1Hash ^ level2Hash ^ level3Hash ^ level4Hash ^ formatHashCode ^ formatTypeHashCode;
		}

		public virtual void SetDefaults(string user)
		{
			CreatedBy = user;
			Level1 = Level1 ?? string.Empty;
			Level2 = Level2 ?? string.Empty;
			Level3 = Level3 ?? string.Empty;
			Level4 = Level4 ?? string.Empty;
			Class = Class ?? string.Empty; // Not-Mandatory
		}
	}
}
