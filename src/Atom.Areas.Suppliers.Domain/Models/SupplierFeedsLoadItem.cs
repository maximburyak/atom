namespace Atom.Areas.Suppliers.Domain.Models
{
	public class SupplierFeedsLoadItem
	{
		public virtual int SupplierId { get; set; }
		public virtual string Level1 { get; set; }
		public virtual string Level2 { get; set; }
		public virtual string Level3 { get; set; }
		public virtual string Level4 { get; set; }

		public override bool Equals(object obj)
		{
			return obj.GetHashCode().Equals(GetHashCode());
		}
		public override int GetHashCode()
		{
			return HashCode(this);
		}

		private int HashCode(SupplierFeedsLoadItem obj)
		{
			return obj.SupplierId.GetHashCode() ^ obj.Level1.GetHashCode() ^ obj.Level2.GetHashCode() ^ obj.Level3.GetHashCode() ^ obj.Level4.GetHashCode();
		}
	}
}
