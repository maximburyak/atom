using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
    public sealed class IncidentDepartmentMap : ClassMap<IncidentDepartment>, IClassMap
    {
        public IncidentDepartmentMap()
        {
            CompositeId()
                .KeyReference(x => x.Area)
                .KeyReference(x => x.HandlingDepartment)
                .KeyReference(x => x.Location);
        }
    }
}
