using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;
using NHibernate.SqlCommand;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListSingleIncident : IQuery
	{
		public int id { get; set; }
		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof(SupportIncident));
			IList<AdditionalInfo> additionalInfo = null;
			AdditionalInfoType additionalInfoType = null;
			if (id > 0)
				criteria.Add<SupportIncident>(x => x.Id == id);

			criteria.CreateAlias<SupportIncident>(c => c.AdditionalInfo, () => additionalInfo, JoinType.LeftOuterJoin)
				.CreateAlias<SupportIncident>(p => p.AdditionalInfo[0].InfoType, () => additionalInfoType, JoinType.LeftOuterJoin);


			return criteria;
		}
	}
}
