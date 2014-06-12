using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public class FileLogMap :ClassMap<FileLog> , IClassMap
	{
		public FileLogMap()
		{
			Id(x => x.Id).GeneratedBy.Assigned();
			Map(x => x.Website);
			Map(x => x.Name);
			Map(x => x.Extension);
			Map(x => x.FullName);
			Map(x => x.DirectoryName);
			Map(x => x.RelativePath);
			Map(x => x.Size);
			Map(x => x.CreateDate);
			Map(x => x.AccessDate);

			Table("IIS.dbo.WebFilesLog");
			ReadOnly();
		}
	}
}
