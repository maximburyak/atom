using System.Collections.Generic;

namespace Atom.Main.Domain
{
	public class DashboardAssignedItem
	{
		public string AssignedTo { get; set; }
		public string LogonId { get; set; }
		public string Nickname { get; set; }
		public List<SignOff> SignOffs { get; set; }
	}

	public class Crf
	{
		public string Number { get; set; }
		public string Title { get; set; }
		public string Severity { get; set; }
		public string Type { get; set; }
	}

	public class DashboardData
	{
		public string AssignedTo { get; set; }
		public string StatusCode { get; set; }
		public string Status { get; set; }
		public string Crf { get; set; }
		public string Title { get; set; }
		public string Severity { get; set; }
		public string LogonId { get; set; }
		public string Type { get; set; }
		public string Nickname { get; set; }
	}

	public class JsonCrfData
	{
		public string Id { get; set; }
		public string Crf { get; set; }
		public string Title { get; set; }
		public string Severity { get; set; }
		public string Type { get; set; }
	}

	public class JsonStatsData
	{
		public string Id { get; set; }
		public string Total { get; set; }
	}

	public class SignOff
	{
		public string StatusCode { get; set; }
		public string Status { get; set; }
		public List<Crf> Crfs { get; set; }
	}

	public class DashboardItemUnAssigned
	{
		public string Crf { get; set; }
		public string Title { get; set; }
		public string Severity { get; set; }
		public string Type { get; set; }
	}

	public class DashboardItemCompleted
	{
		public string Crf { get; set; }
		public string Title { get; set; }
		public string Severity { get; set; }
		public string Type { get; set; }
	}

	public class Statistic
	{
		public string Title { get; set; }
		public string QtyCrf { get; set; }
		public string QtyProject { get; set; }
	}
}
