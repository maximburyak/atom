using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class ArchivedFile
	{
		public virtual int Id { get; set; }
		public virtual int FileId { get; set; }
		public virtual string FileName { get; set; }
		public virtual string Website { get; set; }
		public virtual string Webpath { get; set; }
		public virtual string NativePath { get; set; }
		public virtual string CreatedBy { get; set; }
		public virtual DateTime CreatedDate { get; set; }
	}
}
