using System;

namespace Atom.Areas.Suppliers.Domain.Models
{
	public class SupplierCategoryMappingV2
	{
		public virtual int SupplierID { get; set; }
		public virtual string Category { get; set; }
		public virtual string Level1 { get; set; }
		public virtual string Level2 { get; set; }
		public virtual string Level3 { get; set; }

		public virtual bool Disabled { get; set; }
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

		private int HashCode(SupplierCategoryMappingV2 obj)
		{
			var supplierIdHash = obj.SupplierID.GetHashCode();
			var categoryHash = obj.Category.GetHashCode();
			var level1Hash = obj.Level1.GetHashCode();
			var level2Hash = obj.Level2.GetHashCode();
			var level3Hash = obj.Level3.GetHashCode();

			return supplierIdHash ^ categoryHash ^ level1Hash ^ level2Hash ^ level3Hash;
		}

		public virtual void SetDefaults(string user)
		{
			CreatedBy = user;
			CreateDate = DateTime.Now;
			Level1 = Level1 ?? string.Empty;
			Level2 = Level2 ?? string.Empty;
			Level3 = Level3 ?? string.Empty;
		}
	}
}