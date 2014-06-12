using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
    public class LocationMap : ClassMap<Location>
    {
        public LocationMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name);
            Map(x => x.Enabled);
            Table("Locations");
        }
    }
}