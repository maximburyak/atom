using Atom.Areas.Suppliers.Domain;
using Atom.Areas.Suppliers.Domain.Models;
using FluentNHibernate.Mapping;

namespace Atom.Areas.Suppliers.Data.ClassMaps
{
    public class EtlFilesProcessLogMap : ClassMap<EtlFilesProcessLogEntry>
    {
        public EtlFilesProcessLogMap()
        {
            Id().GeneratedBy.Assigned();
            Map(x => x.Id);
            Map(x => x.SupplierId);
            Map(x => x.FileType).CustomType(typeof (HandlerTypeEnum));
            Map(x => x.LastAttemptedRun).Nullable();
            Map(x => x.LastFileProcessed).Nullable();
            Map(x => x.LastProcessedRun).Nullable();
            Map(x => x.Enabled);
            Map(x => x.CreateDate);
            Map(x => x.CreatedBy);
            Map(x => x.AlteredDate).Nullable();
            Map(x => x.AlteredBy).Nullable();
            Table("Suppliers.dbo.ETLFiles_ProcessLog");
        }
    }
}
