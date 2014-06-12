using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events;
using Atom.Areas.Fusion.Events.WorkItem;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
	public class CommentService
	{
		public virtual void AddComment(WorkItem workItem, Comment comment, User user)
		{
			workItem.Comments.Add(comment);
			DomainEvents.Raise(new WorkItemCommentAdded { Comment = comment, WorkItem = workItem });
		}
	}
}
