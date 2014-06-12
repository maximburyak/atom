using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Atom.Main.Areas.Dashboard.Models.ViewModels;
using Atom.Main.Domain;
using NHibernate;

namespace Atom.Main.Areas.Dashboard.Services
{
	public class DashboardServices
	{
		private readonly ISession _session;
		private string ConnStr { get; set; }
		private const string ApplicationName = "Atom.Main";

		public DashboardServices(ISession session)
		{
			_session = session;
			ConnStr = ConfigurationManager.ConnectionStrings[ApplicationName].ConnectionString;
		}

		public DashboardViewModel PopulateViewModel()
		{
			var viewModel = new DashboardViewModel(_session);
			var conn = new SqlConnection(ConnStr);
			var cmd = new SqlCommand("Fusion.dbo.GrandPix_GetData", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			var ds = new DataSet();
			var adapter = new SqlDataAdapter { SelectCommand = cmd };
			adapter.Fill(ds);

			viewModel.PMO_Items = Summarise(ListAssignedItems(ds.Tables[0]));
			viewModel.PMO_Team_Items = Summarise(ListAssignedItems(ds.Tables[1]));
			viewModel.IT_Items = Summarise(ListAssignedItems(ds.Tables[2]));
			viewModel.Unassigned = ListUnAssignedItems(ds.Tables[3]);
			viewModel.Completed = ListCompletedItems(ds.Tables[4]);
			//viewModel.Stats1 = StatsItems(ds.Tables[5]);
			//viewModel.Stats2 = StatsItems(ds.Tables[6]);

			return viewModel;
		}

		public JsonViewModel PopulateJsonModel()
		{
			var viewModel = new JsonViewModel();
			var conn = new SqlConnection(ConnStr);
			var cmd = new SqlCommand("Fusion.dbo.GrandPix_GetJsonData", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			var ds = new DataSet();
			var adapter = new SqlDataAdapter { SelectCommand = cmd };
			adapter.Fill(ds);

			viewModel.CrfData = SummariseCrfJsonData(ListAssignedItems(ds.Tables[0]));
			viewModel.PmoGroup = SummariseCrfJsonData(ListAssignedItems(ds.Tables[1]));
			viewModel.Unassigned = SummariseUnassignedJsonData(ListUnAssignedItems(ds.Tables[2]));
			viewModel.Completed = SummariseCompletedJsonData(ListCompletedItems(ds.Tables[3]));
			viewModel.StatsData = SummariseStatsJsonData(StatsItems(ds.Tables[4]));

			return viewModel;
		}

		public List<Statistic> StatsItems(DataTable dataTable)
		{
			var items = new List<Statistic>();
			try
			{
				foreach (DataRow row in dataTable.Rows)
				{
					var item = new Statistic
					{
						Title = row["Title"].ToString(),
						QtyCrf = row["CRFs"].ToString(),
						QtyProject = row["Projects"].ToString()
					};
					items.Add(item);
				}
				return items;
			}
			catch (Exception e)
			{
				if (e.GetType() == typeof(SqlException))
					throw e;

				return new List<Statistic>(); // return empty
			}
		}

		public List<DashboardData> ListAssignedItems(DataTable dataTable)
		{
			var items = new List<DashboardData>();
			try
			{
				foreach (DataRow row in dataTable.Rows)
				{
					var item = new DashboardData
					{
						AssignedTo = row["AssignedTo"].ToString(),
						StatusCode = row["StatusCode"].ToString(),
						Status = row["status"].ToString(),
						Crf = row["CRF"].ToString(),
						Title = row["Title"].ToString(),
						Severity = row["Severity"].ToString(),
						LogonId = row["logonid"].ToString(),
						Type = row["type"].ToString(),
						Nickname = row["Nickname"].ToString()
					};
					items.Add(item);
				}
				return items;
			}
			catch (Exception e)
			{
				if (e.GetType() == typeof(SqlException))
					throw e;

				return new List<DashboardData>(); // return empty
			}

		}

		public List<DashboardItemUnAssigned> ListUnAssignedItems(DataTable dataTable)
		{
			var items = new List<DashboardItemUnAssigned>();
			try
			{
				foreach (DataRow row in dataTable.Rows)
				{
					var item = new DashboardItemUnAssigned
					{
						Crf = row["CRF"].ToString(),
						Title = row["Title"].ToString(),
						Severity = row["Severity"].ToString(),
						Type = row["type"].ToString()
					};
					items.Add(item);
				}
				return items;
			}
			catch (Exception e)
			{
				if (e.GetType() == typeof(SqlException))
					throw e;

				return new List<DashboardItemUnAssigned>(); // return empty
			}
		}

		public List<DashboardItemCompleted> ListCompletedItems(DataTable dataTable)
		{
			var items = new List<DashboardItemCompleted>();
			try
			{
				foreach (DataRow row in dataTable.Rows)
				{
					var item = new DashboardItemCompleted
					{
						Crf = row["CRF"].ToString(),
						Title = row["Title"].ToString(),
						Severity = row["Severity"].ToString(),
						Type = row["type"].ToString()
					};
					items.Add(item);
				}
				return items;
			}
			catch (Exception e)
			{
				if (e.GetType() == typeof(SqlException))
					throw e;

				return new List<DashboardItemCompleted>(); // return empty
			}
		}

		public List<JsonCrfData> SummariseCrfJsonData(List<DashboardData> data)
		{
			var items = new List<JsonCrfData>();
			JsonCrfData item = null;

			foreach (var row in data)
			{
				item = new JsonCrfData()
						{
							Crf = row.Crf,
							Id = row.AssignedTo + "_" + row.Status.Replace(" ", "").Replace("&", ""),
							Title = row.Title,
							Type = row.Type,
							Severity = row.Severity
						};
				items.Add(item);
			}
			return items;
		}

		public List<JsonCrfData> SummariseUnassignedJsonData(List<DashboardItemUnAssigned> data)
		{
			var items = new List<JsonCrfData>();
			JsonCrfData item = null;

			foreach (var row in data)
			{
				item = new JsonCrfData()
				{
					Crf = row.Crf,
					Id = "unassigned_crfs",
					Title = row.Title,
					Type = row.Type,
					Severity = row.Severity
				};
				items.Add(item);
			}
			return items;
		}

		public List<JsonCrfData> SummariseCompletedJsonData(List<DashboardItemCompleted> data)
		{
			var items = new List<JsonCrfData>();
			JsonCrfData item = null;

			foreach (var row in data)
			{
				item = new JsonCrfData()
				{
					Crf = row.Crf,
					Id = "completed_crfs",
					Title = row.Title,
					Type = row.Type,
					Severity = row.Severity
				};
				items.Add(item);
			}
			return items;
		}

		public List<JsonStatsData> SummariseStatsJsonData(List<Statistic> data)
		{
			var items = new List<JsonStatsData>();
			JsonStatsData item = null;

			foreach (var row in data)
			{
				item = new JsonStatsData()
				{
					Id = row.Title.Replace(" ", "").Replace("&", ""),
					Total = row.QtyCrf
				};
				items.Add(item);
			}
			return items;
		}

		public List<DashboardAssignedItem> Summarise(List<DashboardData> data)
		{
			var items = new List<DashboardAssignedItem>();
			DashboardAssignedItem item = null;
			Crf crf = null;
			List<SignOff> signOffs = null;
			SignOff signOff = null;
			var currentAssignedTo = "";
			var currentStatus = "";

			foreach (var row in data)
			{
				if (row.AssignedTo != currentAssignedTo)
				{
					if (currentAssignedTo != "")
					{
						item.SignOffs.Add(signOff);
						items.Add(item);
					}

					//new row
					currentAssignedTo = row.AssignedTo;
					item = new DashboardAssignedItem();
					item.AssignedTo = row.AssignedTo;
					item.LogonId = row.LogonId;
					item.Nickname = row.Nickname;

					currentStatus = row.Status;
					item.SignOffs = new List<SignOff>();
					signOff = new SignOff();
					signOff.StatusCode = row.StatusCode;
					signOff.Status = row.Status;

					signOff.Crfs = new List<Crf>();
					crf = new Crf { Number = row.Crf, Title = row.Title, Severity = row.Severity, Type = row.Type };
					signOff.Crfs.Add(crf);
				}
				else if (row.Status != currentStatus)
				{
					//new status
					item.SignOffs.Add(signOff);

					currentStatus = row.Status;
					signOff = new SignOff();
					signOff.StatusCode = row.StatusCode;
					signOff.Status = row.Status;

					signOff.Crfs = new List<Crf>();
					crf = new Crf { Number = row.Crf, Title = row.Title, Severity = row.Severity, Type = row.Type };
					signOff.Crfs.Add(crf);
				}
				else
				{
					crf = new Crf { Number = row.Crf, Title = row.Title, Severity = row.Severity, Type = row.Type };
					signOff.Crfs.Add(crf);
				}
			}

			item.SignOffs.Add(signOff);
			items.Add(item);

			return items;
		}

	}
}