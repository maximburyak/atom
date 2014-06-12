using Atom.Areas.Fusion.Data.Conventions;
using Atom.Areas.Fusion.Domain.Models;
using FluentNHibernate.Automapping;

namespace Atom.Main.Setup
{
    public class ApConfig
    {
        public static AutoPersistenceModel Init()
        {
            var assemblyArray = new[] { typeof(SupportIncident).Assembly };
            var autoconfig = AutoMap.Assemblies(assemblyArray)
                .Where(n => (n.Namespace == "Atom.Areas.Fusion.Domain.Models" && n.Name != "User"))
                .UseOverridesFromAssemblyOf<ClassConventions>()
                .Conventions.AddFromAssemblyOf<ClassConventions>();

            //autoconfig.WriteMappingsTo("d:");
            return autoconfig;
        }
    }
}