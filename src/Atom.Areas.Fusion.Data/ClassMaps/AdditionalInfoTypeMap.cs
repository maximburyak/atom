using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public sealed class AdditionalInfoTypeMap : ClassMap<AdditionalInfoType>, IClassMap
	{
		public AdditionalInfoTypeMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Description);
			Map(x => x.AlteredDate).Nullable();
			References(x => x.AlteredBy).Column("AlteredBy");
			References(x => x.CreatedBy).Column("CreatedBy");
			Map(x => x.CreateDate).Nullable();
			HasManyToMany(x => x.Categories)
                .Table("AdditionalInfoToCategories")
                .ParentKeyColumn("Category_Id")
                .ChildKeyColumn("AdditionalInfoType_Id")
                .ReadOnly();
		}

	}
}