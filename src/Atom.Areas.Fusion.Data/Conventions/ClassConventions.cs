using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Atom.Areas.Fusion.Data.Conventions
{
	public class ClassConventions : IClassConvention
	{
		public void Apply(IClassInstance instance)
		{
			instance.DynamicUpdate();
			instance.Table("Fusion.dbo." + instance.TableName.Replace("`", ""));
		}
	}

    //public class ReferencesCollection : IReferenceConvention
    //{
    //    public void Apply(IManyToOneInstance instance)
    //    {
    //        instance.Cascade.All(); 
    //    }
    //}
}