using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{

	public class WebLogAnalysisMap : ClassMap<WebLogAnalysis>, IClassMap
	{
		public WebLogAnalysisMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Website);
			Map(x => x.Path);
			Map(x => x.FileName);
			Map(x => x.Extension);
			Map(x => x.AccessCount);
			Map(x => x.FirstAccessed);
			Map(x => x.LastAccessed);
			Map(x => x.ProcessingTime);
			//Map(x => x.AvgProcessingTime);
			Table("IIS.dbo.WebTrafficAnalysis");
			ReadOnly();
		}
	}

}
