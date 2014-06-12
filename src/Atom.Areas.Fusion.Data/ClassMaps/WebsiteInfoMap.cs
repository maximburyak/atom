using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public class WebsiteInfoMap : ClassMap<WebsiteInfo>, IClassMap
	{
		public WebsiteInfoMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Website).Column("Host");
			Map(x => x.IISInternalName);
			Table("IIS.dbo.Websites");
			ReadOnly();
		}

	}
}
