namespace Atom.Areas.Suppliers.Domain.Models
{
	// This is mapped to the Suppliers.dbo.POWERPLAY table
	public class PowerplaySuppliedFeedItem
	{
		public virtual int ProdId { get; set; }
		public virtual int SupplierId { get; set; }
		public virtual Format Format { get; set; }
		public virtual FormatType FormatType { get; set; }
		public virtual ClassCms Class { get; set; }

	}
}