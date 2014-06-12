using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class Comment : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string CommentText { get; set; }
		public virtual CommentTypeEnum Type { get; set; }
		public virtual int UnitsOfWork { get; set; }

	}
	public enum CommentTypeEnum
	{
		[Description("General")]
		General=0,
		[Description("Severity Change")]
		SeverityChange = 1
	}
}