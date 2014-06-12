using System;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Atom.Areas.Fusion.Domain
{
	public class WebsiteInfo
	{
		public virtual int Id { get; set; }
		public virtual string Website { get; set; }
		public virtual string IISInternalName { get; set; }

		public WebsiteInfo()
		{
			
		}
	}
}