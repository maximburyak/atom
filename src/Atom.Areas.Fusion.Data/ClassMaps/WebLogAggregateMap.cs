using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	class WebLogAggregateMap : ClassMap<WebLogAggregate>, IClassMap
	{
		public WebLogAggregateMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Target);
			Map(x => x.AccessCount);
			Map(x => x.FirstAccessed);
			Map(x => x.LastAccessed);
			Map(x => x.ProcessingTime);
			Map(x => x.AvgProcessingTime);
			Table("IIS.dbo.AggregateInternetLog");
			ReadOnly();
		}
	}
}
