using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public class CompoundWebLogMap :ClassMap<CompoundWebLog>
	{
		public CompoundWebLogMap()
		{
			Id(x => x.Id).GeneratedBy.Native();

			// from IIS.dbo.WebFilesLog
			Map(x => x.FileId);	
			Map(x => x.Website);
			Map(x => x.Name);
			Map(x => x.Extension);
			Map(x => x.FullName);
			Map(x => x.DirectoryName);
			Map(x => x.RelativePath);
			Map(x => x.Size);
			
			//  from IIS.dbo.InternetLog
			Map(x => x.LogTime);
			Map(x => x.ClientIp).Column("c_ip");
			Map(x => x.UsernameRequest).Column("cs_username");
			Map(x => x.ServerSiteName).Column("s_sitename");
			Map(x => x.ServerComputerName).Column("s_computername");
			Map(x => x.ServerIp).Column("s_ip");
			Map(x => x.ServerPort).Column("s_port");
			Map(x => x.MethodRequest).Column("cs_method");
			Map(x => x.UriQueryRequest).Column("cs_uri_query");
			Map(x => x.StatusResponse).Column("sc_status");
			Map(x => x.SubStatusResponse).Column("sc_substatus");
			Map(x => x.Win32StatusResponse).Column("sc_win32_status");
			Map(x => x.BytesRequest).Column("cs_bytes");
			Map(x => x.BytesResponse).Column("sc_bytes");
			Map(x => x.TimeTaken).Column("time_taken");
			Map(x => x.VersionRequest).Column("cs_version");
			Map(x => x.UserAgentRequest).Column("cs_user_agent");
			Map(x => x.CookieRequest).Column("cs_cookie");
			Map(x => x.RefererRequest).Column("cs_referer");
			Table("IIS.dbo.CompoundLog");
			ReadOnly();
		}
			
	}
}
