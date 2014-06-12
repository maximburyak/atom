using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Atom.Areas.Fusion.Data.Conventions
{
	//public class DefaultLazyOverride : IHibernateMappingConvention
	//{
	//    public void Apply(IHibernateMappingInstance instance)
	//    {
	//        if (instance.EntityType == typeof(SupportProfile))
	//            instance.Not.DefaultLazy();
	//    }
	//}

	//public class SupportProfileMappingOverride : IAutoMappingOverride<SupportProfile>
	//{
	//    public void Override(AutoMapping<SupportProfile> mapping)
	//    {
	//        //mapping.HasMany(x => x.Avatars).Not.LazyLoad();
	//    }
	//}

	//public class SignOffMappingOverride : IAutoMappingOverride<WorkItem>
	//{
	//    public void Override(AutoMapping<WorkItem> mapping)
	//    {
	//        mapping.HasMany(x => x.SignOffs).Not.LazyLoad();
	//    }
	//}

	public class DynamicUpdateSubClassConvention : IJoinedSubclassConvention
	{
		public void Apply(IJoinedSubclassInstance instance)
		{
			instance.DynamicUpdate();
		}
	}
}