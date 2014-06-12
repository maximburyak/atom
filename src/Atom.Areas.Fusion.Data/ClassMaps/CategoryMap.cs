using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Fusion.Data.ClassMaps
{
	public sealed class CategoryMap : ClassMap<Category>, IClassMap
	{
		public CategoryMap()
		{
			Id(x => x.Id).GeneratedBy.Native();
			Map(x => x.Description);
			Map(x => x.AlteredDate).Nullable();
		    Map(x => x.Enabled);
			References(x => x.AlteredBy).Column("AlteredBy");
			References(x => x.CreatedBy).Column("CreatedBy");
			Map(x => x.CreateDate).Nullable();
            HasManyToMany(x => x.AdditionalInfo).Table("AdditionalInfoToCategories")
                .ChildKeyColumn("AdditionalInfoType_Id").ParentKeyColumn("Category_Id").ReadOnly();
		}
	}
}