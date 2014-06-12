using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Services.Domain;
using Atom.Main.Areas.Stats.Models;
using Atom.Main.Areas.Stats.Models.ViewModels;
using BeValued.Data.NHibernate;
using NHibernate;

namespace Atom.Main.Areas.Stats.Services.Domain
{
	public class WebLogOperationService
	{
		private readonly string _archiveRoot;
		private readonly ISession _session;
		private readonly NHibernateRepository<ArchivedFile> _archivedFilesRepo;
		private readonly string _pathSeparator = Path.DirectorySeparatorChar.ToString();

		public WebLogOperationService(string archiveRoot, ISession session)
		{
			_archivedFilesRepo = new NHibernateRepository<ArchivedFile>(_session);
			_archiveRoot = archiveRoot;
			_session = session;
			if (!_archiveRoot.EndsWith(_pathSeparator))
				_archiveRoot += _pathSeparator;
		}

		public ListArchivedWebFilesViewModel PopulateListArchivedWebFilesViewModel(ListArchivedWebFilesViewModel viewModel)
		{

			viewModel.ArchivedFiles = new ListArchivedFiles()
										{
											SortColumn = viewModel.SortColumn,
											IsSortDesc = viewModel.IsSortDesc,
											Website = viewModel.SelectedWebsite,
											WebPath = viewModel.PathFilter
										}
										.GetQuery(_session)
										.List<ArchivedFile>();

			PopulateAvailableSites(viewModel);

			return viewModel;
		}

		public ListWebLogViewModel PopulateListWebLogViewModel(ListWebLogViewModel viewModel)
		{
			viewModel.SortColumn = viewModel.SortColumn ?? GetPropertyName(() => new WebLogAnalysis().LastAccessed);

			// set the inputs
			var query = new ListWebLogAnalysis()
							{
								FromDate = viewModel.FromDate,
								ToDate = viewModel.ToDate,
								SortColumn = viewModel.SortColumn,
								IsSortDesc = viewModel.IsSortDesc,
								MaxResults = viewModel.ResultsBatchSize,
								PathFilter = viewModel.PathFilter,
								Website = viewModel.SelectedWebsite,
								AccessCountFrom = viewModel.AccessCountFrom,
								AccessCountTo = viewModel.AccessCountTo
							};

			PopulateAvailableSites(viewModel);

			viewModel.LogEntries = query
				.GetQuery(_session)
				.List<WebLogAnalysis>();

			return viewModel;
		}

		private void PopulateAvailableSites(ListWebLogViewModelBase viewModel)
		{
			// get the outputs
			viewModel.AvailableSites = new ListWebLogWebsites()
										.GetQuery(_session)
										.List<WebsiteInfo>()
										.Select(x => new WebsiteName()
													{
														SiteName = x.Website,
														IISInternalName = x.IISInternalName
													})
										.ToList();
		}

		public static string GetPropertyName<T>(Expression<Func<T>> expression)
		{
			MemberExpression body = (MemberExpression)expression.Body;
			return body.Member.Name;
		}

		public bool ArchiveFile(ArchivedFile file) //string site, string relativePath, string absolutePath)
		{
			var siteRoot = _archiveRoot + file.Website;
			if (!Directory.Exists(siteRoot))
				Directory.CreateDirectory(siteRoot);

			var pathStr = file.Webpath;

			var splitOptions = new[] { "/" };
			var nodes = pathStr.Split(splitOptions, StringSplitOptions.RemoveEmptyEntries);

			var dirPath = CreateDirectoryPath(nodes, siteRoot);

			var newFullname = dirPath + _pathSeparator + nodes.Last();

			SmartMoveFile(file.NativePath, newFullname);

			file.CreatedBy = HttpContext.Current.User.Identity.Name;
			file.CreatedDate = DateTime.Now;

			_archivedFilesRepo.Save(file);

			// we're not doing the repo.Delete() approach because we don't have the ID for the CompoundLog entry we want to delete
			var query = _session.CreateSQLQuery("delete from IIS.dbo.CompoundLog where FileId=:fid");
			query.SetInt32("fid", file.FileId);
			query.ExecuteUpdate();

			return File.Exists(newFullname);
		}

		private static void SmartMoveFile(string source, string destination)
		{
			var sourceExists = File.Exists(source);
			var destExists = File.Exists(destination);

			if (sourceExists && !destExists)
				File.Move(source, destination);

			else if (sourceExists && destExists)
			{
				File.Delete(destination);
				File.Move(source, destination);
			}

			else if (!sourceExists && !destExists)
				throw new FileNotFoundException(@"The file " + source + @" does not exist in either their source or destination locations.");
		}

		private static string CreateDirectoryPath(ICollection<string> nodes, string siteRoot)
		{
			var pathSeparator = Path.DirectorySeparatorChar.ToString();
			var currentPath = siteRoot;
			foreach (var node in nodes.Where((s, i) => i < nodes.Count - 1))
			{
				currentPath += pathSeparator + node;

				if (!Directory.Exists(currentPath))
					Directory.CreateDirectory((currentPath));
			}
			return currentPath;
		}

		public bool RestoreFile(ArchivedFile file)
		{
			var siteRoot = _archiveRoot + file.Website;
			if (!Directory.Exists(siteRoot))
				throw new DirectoryNotFoundException("Website backup folder does not exists: " + siteRoot);

			var archivePath = siteRoot + file.Webpath.Replace("/", Path.DirectorySeparatorChar.ToString());

			if (!File.Exists(archivePath))
				throw new FileNotFoundException("The archived file, " + archivePath + " does not exist");

			_archivedFilesRepo.Delete(file);

			var query = _session.CreateSQLQuery("exec IIS.dbo.MergeLogs @fileId=:fid");
			query.SetInt32("fid", file.FileId);
			query.ExecuteUpdate();

			SmartMoveFile(archivePath, file.NativePath);
			return File.Exists(file.NativePath);
		}
	}
}