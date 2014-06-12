using System.Linq;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class WorkItemHelper
	{
		private static string WorkItemSupplierTitle(HtmlHelper helper, WorkItem workItem)
		{
			var title = string.Join("<br/>", (from c in workItem.Suppliers select c.Supplier.Name).ToArray());
			return "<a href=\"#\" onclick=\"return false;\" class=\"toolInfo\" title=\"" + title + "\">{--}</a>";
		}

		public static string WorkItemSuppliers(this HtmlHelper helper, WorkItem workitem)
		{
			var count = workitem.Suppliers.Count;
			return count == 0 ? "None chosen" : "{0}{1} {2}".With(workitem.Suppliers[0].Supplier.Name, count > 1 ? ", " + workitem.Suppliers[1].Supplier.Name : "", count > 1 ? WorkItemSupplierTitle(helper, workitem) : "");
		}

		private static string WorkItemProductsTitle(this HtmlHelper helper, WorkItem workItem)
		{
			var title = string.Join("<br/>", (from c in workItem.ProductGroups select c.ProductGroup.Name).ToArray());
			return "<a href=\"#\" onclick=\"return false;\" class=\"toolInfo\" title=\"" + title + "\">{--}</a>";
		}

		private static string WorkItemInscosTitle(this HtmlHelper helper, WorkItem workItem)
		{
			var title = string.Join("<br/>", (from c in workItem.InsuranceCompanies select c.InsuranceCompany.Name).ToArray());
			return "<a href=\"#\" onclick=\"return false;\" class=\"toolInfo\" title=\"" + title + "\">{--}</a>";
		}

		public static string WorkItemProductGroups(this HtmlHelper helper, WorkItem workitem)
		{
			var count = workitem.ProductGroups.Count;
			return count == 0 ? "None chosen" : "{0}{1} {2}".With(workitem.ProductGroups[0].ProductGroup.Name, count > 1 ? ", " + workitem.ProductGroups[1].ProductGroup.Name : "", count > 1 ? WorkItemProductsTitle(helper, workitem) : "");
		}

		public static string WorkItemInscos(this HtmlHelper helper, WorkItem workitem)
		{
			var count = workitem.InsuranceCompanies.Count;
			return count == 0 ? "None chosen" : "{0}{1} {2}".With(workitem.InsuranceCompanies[0].InsuranceCompany.Name, count > 1 ? ", " + workitem.InsuranceCompanies[1].InsuranceCompany.Name : "", count > 1 ? WorkItemInscosTitle(helper, workitem) : "");
		}
	}
}
