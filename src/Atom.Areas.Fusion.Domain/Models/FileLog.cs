using System;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class FileLog
	{
		public virtual int Id { get; set; }
		public virtual string Website { get; set; }
		public virtual string Name { get; set; }
		public virtual string Extension { get; set; }
		public virtual string FullName { get; set; }
		public virtual string DirectoryName { get; set; }
		public virtual string RelativePath { get; set; }
		public virtual int Size { get; set; }
		public virtual DateTime CreateDate { get; set; }
		public virtual DateTime AccessDate { get; set; }
		

	}
}
