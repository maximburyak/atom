using System;
using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class AddCrfViewModel : BaseWorkItemViewModel
	{
		public AddCrfViewModel(WorkItem workItem)
		{
			WorkItem = workItem;
		}
		public Crf Crf
		{
			get
			{
				return (Crf)WorkItem;
			}
		}
		public IDictionary<int, string> SignOffs { get; set; }
		public DateTime ChangeBoardMeetingDate { get; set; }
		public IDictionary<int, string> Locations()
		{
			return new LocationEnum().ToDictionary();
		}
		public new IDictionary<int, string> Severity()
		{
			return new SeverityEnum().ToDictionary();
		}
		public new User User { get; set; }

		public IList<Channel> SelectedChannels { get; set; }
		public IList<InsuranceCompany> SelectedInsuranceCompanies { get; set; }
		public IList<Supplier> SelectedSuppliers { get; set; }
		public IList<ProductGroup> SelectedProductGroups { get; set; }

	}


}
