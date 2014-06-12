using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class AddCaseViewModel : BaseWorkItemViewModel
	{
		public AddCaseViewModel(WorkItem workItem)
		{
			WorkItem = workItem;
		}
		public SupportIncident Incident
		{
			get
			{
				return (SupportIncident)WorkItem;
			}
		}
		public IList<Area> SupportAreas { get; set; }
		public Area IncidentArea { get; set; }
		public IList<Category> AreaCategories { get; set; }
		public IList<AdditionalInfoType> CategoryAdditionalInfo { get; set; }
		public string IncidentAdditionalInfo { get; set; }
        public virtual IEnumerable<Location> Locations { get; set; }
		public new virtual IDictionary<int, string> Severity()
		{
			var list = new SeverityEnum().ToDictionary();
			list.Remove(0);
			return list;
		}
		public virtual IDictionary<int, string> FaultTypes()
		{
			return new RequestTypeEnum().ToDictionary();
		}
		
		public User User { get; set; }
	}
}