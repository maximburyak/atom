using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.Criterion;

namespace Atom.Areas.Fusion.Data.Queries
{
    public class ListSupportLocations : IQuery
    {
        public ICriteria GetQuery(ISession session)
        {
            return session.CreateCriteria(typeof(Location)).Add(Restrictions.Where<Location>(x=>x.Enabled)); 
        }
    }
}