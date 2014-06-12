using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries
{
    public class ListSingleIncidentDept : IQuery
    {
        public Area Area { get; set; }
        public Location Location { get; set; }

        public ICriteria GetQuery(ISession session)
        {
            var criteria = session.CreateCriteria(typeof(IncidentDepartment));
            if (Area != null)
                criteria.Add<IncidentDepartment>(x => x.Area == Area);

            if (Location != null)
                criteria.Add<IncidentDepartment>(x => x.Location == Location);

            criteria.SetMaxResults(1);
            return criteria;
        }
    }
}
