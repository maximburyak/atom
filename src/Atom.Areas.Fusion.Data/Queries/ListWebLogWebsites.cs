using System;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Atom.Areas.Fusion.Data.Queries;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
	public class ListWebLogWebsites
	{
		public ICriteria GetQuery(NHibernate.ISession iSession)
		{
			var website = PropertyUtil.GetName<WebsiteInfo>(x => x.Website);

			ICriteria criteria = iSession.CreateCriteria<WebsiteInfo>();
				//.SetProjection(Projections.Distinct(Projections.ProjectionList()
				//                                        .Add(Projections.Alias(Projections.Property(website), website))))
				//.SetResultTransformer(new AliasToBeanResultTransformer(typeof (WebsiteInfo)));
			criteria.AddOrder(Order.Asc(website));

			return criteria;
		}
	}
}