
namespace Atom.Areas.Fusion.Domain.Models
{
	public class Avatar : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string FileExtension { get; set; }
	}
}