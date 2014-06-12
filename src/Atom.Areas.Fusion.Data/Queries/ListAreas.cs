using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;
using NHibernate.SqlCommand;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListCategoriesForArea:IQuery
	{
		public int Id { get; set; }

		public ICriteria GetQuery(ISession session)
		{
		    Category categoriesAlias=null;
			var criteria = session.CreateCriteria(typeof(Area))
                .CreateAlias<Area>(x=>x.Categories,()=> categoriesAlias)
				.Add<Area>(x => x.Id == Id)
				.Add<Area>(x => x.Enabled)
                .Add(()=>categoriesAlias.Enabled)
                //.AddOrder(()=>categoriesAlias.Description,Order.Asc)
				.SetFetchMode<Area>(x => x.Categories, FetchMode.Eager);
			return criteria;
		}
	}
}