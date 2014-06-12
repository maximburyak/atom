using Atom.Areas.Fusion.Domain;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
    public sealed class ProfileMap : ClassMap<SupportProfile>, IClassMap
    {
        public ProfileMap()
        {
            Id(x => x.UserId).Column("UserID").GeneratedBy.Assigned();
            Map(x => x.CurrentAvatar);
            Map(x => x.CurrentSignature);
            Map(x => x.ShowFilters);
            Map(x => x.RefreshSearch);
            Map(x => x.AlteredDate).Nullable();
            References(x => x.AlteredBy).Column("AlteredBy");
            References(x => x.CreatedBy).Column("CreatedBy");
            Map(x => x.CreateDate).Nullable();
            HasMany(x => x.Avatars);
            HasMany(x => x.Signatures);
            HasMany(x => x.Filters);
            Map(x => x.IsAssignedToAuto);
            Table("Fusion.dbo.Profile");
        }
    }
}