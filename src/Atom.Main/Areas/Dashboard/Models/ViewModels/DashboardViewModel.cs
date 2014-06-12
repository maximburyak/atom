using System.Collections.Generic;
using Atom.Main.Domain;
using NHibernate;

namespace Atom.Main.Areas.Dashboard.Models.ViewModels
{
	public class DashboardViewModel
	{
		public IList<DashboardAssignedItem> IT_Items;
		public IList<DashboardAssignedItem> PMO_Items;
		public IList<DashboardAssignedItem> PMO_Team_Items;
		public IList<DashboardItemUnAssigned> Unassigned;
		public IList<DashboardItemCompleted> Completed;
		public IList<Statistic> Stats1;
		public IList<Statistic> Stats2;
		public string Draggable;
		public string Droppable;
		public ISession Session;

		public DashboardViewModel(ISession session)
		{
			Session = session;
			IT_Items = new List<DashboardAssignedItem>();
			PMO_Items = new List<DashboardAssignedItem>();
			PMO_Team_Items = new List<DashboardAssignedItem>();
			Unassigned = new List<DashboardItemUnAssigned>();
			Completed = new List<DashboardItemCompleted>();
			Stats1 = new List<Statistic>();
			Stats2 = new List<Statistic>();
			Draggable = "draggable ";
			Droppable = "droppable ";
		}
	}

	public class JsonViewModel
	{
		public IList<JsonCrfData> CrfData;
		public IList<JsonStatsData> StatsData;
		public IList<JsonCrfData> Unassigned;
		public IList<JsonCrfData> Completed;
		public IList<JsonCrfData> PmoGroup;
		public string Draggable;
		public string Droppable;

		public JsonViewModel()
		{
			CrfData = new List<JsonCrfData>();
			StatsData = new List<JsonStatsData>();
			Unassigned = new List<JsonCrfData>();
			Completed = new List<JsonCrfData>();
			PmoGroup = new List<JsonCrfData>();
			Draggable = "draggable ";
			Droppable = "droppable ";
		}
	}
}