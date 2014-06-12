using System;
using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Stats.Models.ViewModels
{
	public class ListWebLogViewModel: ListWebLogViewModelBase
	{
		public DateTime? FromDate { get; set; }

		public DateTime? ToDate { get; set; }

		public IList<WebLogAnalysis> LogEntries { get; set; }

		public int ResultsBatchSize { get; set; }

		public string AccessCountTo { get; set; }

		public string AccessCountFrom { get; set; }

		public IEnumerable<string> ExcludeColumns { get; set; }
	}
}