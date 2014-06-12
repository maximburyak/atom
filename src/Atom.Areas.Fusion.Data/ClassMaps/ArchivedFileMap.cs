using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public class ArchivedFileMap : ClassMap<ArchivedFile>, IClassMap
	{
		public ArchivedFileMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.FileId);
			Map(x => x.FileName);
			Map(x => x.Website);
			Map(x => x.Webpath);
			Map(x => x.NativePath);
			Map(x => x.CreatedBy);
			Map(x => x.CreatedDate);
			Table("IIS.dbo.ArchivedFiles");
		}
	}
}
