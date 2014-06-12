using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Stats.Models.ViewModels
{
	public class ListArchivedWebFilesViewModel: ListWebLogViewModelBase
	{
		public IList<ArchivedFile> ArchivedFiles;

	}
}