using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;
namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListAdditionalInfoForCategory:IQuery
	{
		public int Id { get; set; }

		public ICriteria GetQuery(ISession session)
		{
			var criteria = session.CreateCriteria(typeof (Category))
				.Add<Category>(x => x.Id == Id)
				.SetFetchMode<Category>(x => x.AdditionalInfo, FetchMode.Eager)
				.SetMaxResults(20);
			return criteria;
		}
	}
}
