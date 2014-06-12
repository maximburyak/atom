using System;

namespace Atom.Areas.Suppliers.Domain.Models
{
    public class EtlFilesProcessLogEntry
    {
        public virtual int Id { get; set; }
        public virtual int SupplierId { get; set; }
        public virtual HandlerTypeEnum FileType { get; set; }
        public virtual string LastFileProcessed { get; set; }
        public virtual DateTime? LastAttemptedRun { get; set; }
        public virtual DateTime? LastProcessedRun { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? AlteredDate { get; set; }
        public virtual string AlteredBy { get; set; }
    }
}
