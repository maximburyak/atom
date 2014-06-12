using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class CompoundWebLog
	{
		public virtual int Id { get; set; }

		// IIS.dbo.WebFilesLog columns
		public virtual int		FileId { get; set; }
		public virtual string	Website { get; set; }
		public virtual string	Name { get; set; }
		public virtual string	Extension { get; set; }
		public virtual string	FullName { get; set; }
		public virtual string	DirectoryName { get; set; }
		public virtual string	RelativePath { get; set; }
		public virtual int		Size { get; set; }

		// IIS.dbo.InternetLog columns
		public virtual DateTime LogTime { get; set; }
		public virtual string	ClientIp { get; set; }
		public virtual string	UsernameRequest { get; set; }
		public virtual string	ServerSiteName { get; set; }
		public virtual string	ServerComputerName { get; set; }
		public virtual string	ServerIp { get; set; }
		public virtual int		ServerPort { get; set; }
		public virtual string	MethodRequest { get; set; }
		//public virtual string	UriStemRequest { get; set; }
		public virtual string	UriQueryRequest { get; set; }
		public virtual int		StatusResponse { get; set; }
		public virtual int		SubStatusResponse { get; set; }
		public virtual int		Win32StatusResponse { get; set; }
		public virtual int		BytesRequest { get; set; }
		public virtual int		BytesResponse { get; set; }
		public virtual int		TimeTaken { get; set; }
		public virtual string	VersionRequest { get; set; }
		public virtual string	UserAgentRequest { get; set; }
		public virtual string	CookieRequest { get; set; }
		public virtual string	RefererRequest { get; set; }

		public CompoundWebLog()
		{
			
		}


	}
}
