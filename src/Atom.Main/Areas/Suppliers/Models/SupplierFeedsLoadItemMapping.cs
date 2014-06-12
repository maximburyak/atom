
namespace Atom.Main.Areas.Suppliers.Models
{
	public class SupplierFeedsLoadItemMapping
	{
		public int SupplierId { get; set; }
		public string Level1 { get; set; }
		public string Level2 { get; set; }
		public string Level3 { get; set; }
		public string Level4 { get; set; }
		// Items to be set on Screen
		public string Format { get; set; }
		public string FormatType { get; set; }
		public string Class { get; set; }
	}

}
