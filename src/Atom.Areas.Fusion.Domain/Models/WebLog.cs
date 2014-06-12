using System;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class WebLog
	{
		public virtual int Id { get; set; }
		public virtual string ClientHost { get; set; }
		public virtual string Username { get; set; }
		public virtual DateTime LogTime { get; set; }
		public virtual string Service { get; set; }
		public virtual string Machine { get; set; }
		public virtual string ServerIp { get; set; }
		public virtual int ProcessingTime { get; set; }
		public virtual int BytesReceived { get; set; }
		public virtual int BytesSent { get; set; }
		public virtual int ServiceStatus { get; set; }
		public virtual int Win32Status { get; set; }
		public virtual string Operation { get; set; }
		public virtual string Target { get; set; }
		public virtual string Parameters { get; set; }

	}
}
