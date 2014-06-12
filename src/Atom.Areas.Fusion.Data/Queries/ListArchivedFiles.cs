using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Utilities;
using NHibernate;
using NHibernate.Criterion;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListArchivedFiles : IQuery
	{
		public bool IsSortDesc { get; set; }
		public string SortColumn { get; set; }
		public string Website { get; set; }
		public string WebPath { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			var website = PropertyUtil.GetName<ArchivedFile>(x => x.Website);
			var webpath = PropertyUtil.GetName<ArchivedFile>(x => x.Webpath);

			var criteria = session
						.CreateCriteria<ArchivedFile>();

			if (IsNotNullOrEmpty(Website)) criteria.Add(Expression.Eq(website, Website));
			if (IsNotNullOrEmpty(WebPath)) criteria.Add(Expression.InsensitiveLike(webpath, WebPath.Replace("*", "%")));
			
			
			criteria.AddOrder((IsSortDesc)
			        ? Order.Desc(SortColumn)
			        : Order.Asc(SortColumn));

			return criteria;
		}

		private static bool IsNotNull(object obj)
		{
			return obj != null;
		}

		private static bool IsNotNullOrEmpty(string str)
		{
			return IsNotNull(str) && str != "";
		}
	}


}
