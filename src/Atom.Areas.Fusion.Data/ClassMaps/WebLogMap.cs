using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public class WebLogMap : ClassMap<WebLog>, IClassMap
	{
		public WebLogMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.ClientHost);
			Map(x => x.Username);
			Map(x => x.LogTime);
			Map(x => x.Service);
			Map(x => x.Machine);
			Map(x => x.ServerIp);
			Map(x => x.ProcessingTime);
			Map(x => x.BytesReceived).Column("BytesRecvd");
			Map(x => x.BytesSent);
			Map(x => x.ServiceStatus);
			Map(x => x.Win32Status);
			Map(x => x.Operation);
			Map(x => x.Target);
			Map(x => x.Parameters);
			Table("IIS.dbo.InternetLog");
			ReadOnly();
		}
	}
}
