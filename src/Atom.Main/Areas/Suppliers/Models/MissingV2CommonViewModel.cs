using System.Collections.Generic;
using System.Linq;
using Atom.Areas.Suppliers.Domain.Models;

namespace Atom.Main.Areas.Suppliers.Models
{
	public abstract class MissingV2CommonViewModel : CommonViewModel
	{
		protected MissingV2CommonViewModel()
		{
			Categories = new List<Validator2Category>();
		}
		public IEnumerable<Validator2Category> Categories { get; set; }

		public IEnumerable<SupplierCms> Suppliers { get; set; }
		public IEnumerable<SupplierCms> PpSuppliers
		{
			get { return Suppliers.Where(x => x.NonPpSupplier == false).OrderBy(x => x.Name); }
		}

		public IEnumerable<SupplierCms> NonPpSuppliers
		{
			get { return Suppliers.Where(x => x.NonPpSupplier).OrderBy(x => x.Name); }
		}
	}
}