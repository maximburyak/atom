using System;
using System.Linq;
using System.Web;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Areas.Fusion.Events;
using Atom.Areas.Fusion.Events.Project;
using Atom.Areas.Fusion.Events.WorkItem;
using Atom.Main.Areas.Fusion.Models.ViewModels;
using BeValued.Utilities.Extensions;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
	public class ProjectService : WorkItemService
	{
		private readonly ISession _session;
		private CommentService _commentService;
		public ProjectService(ISession session)
			: base(session)
		{
			_session = session;
			_commentService = new CommentService();
		}

		public AddProjectViewModel NewAddProjectViewModel(string name)
		{
			var model = new AddProjectViewModel(new Project { Status = ProjectStatus.Requested })
			{
				User = GetUser(name)
			};
			return model;
		}

		public AddProjectViewModel NewAddProjectViewModel(Project project, string name)
		{
			var model = new AddProjectViewModel(project)
			{
				User = GetUser(name)
			};
			return model;
		}

		public void AddProject(Project project, string name, HttpFileCollectionBase files)
		{
			var user = GetUser(name);

			AddComments(project, user);
			// Default to 6 weeks if no date provided.
			project.RequestedCompletionDate = project.RequestedCompletionDate ?? DateTime.Now.AddMonths(2);
			project.WorkItemType = WorkItemTypeEnum.Project;
			project.WorkStatus = WorkItemStatus.AwaitingApproval;

			var handlingdepartment = GetHandlingDepartment(HandlingDepartmentTypeEnum.Pmo.GetDescription());
			project.Department = handlingdepartment;

			Validator<Project>.Validate(project);

			CreateListOfSignOffs(project, user);

			_wiRepo.Save(project);

			if (files.Count > 0)
				AddDocument(project.Id, user, files[0]);
		}

		public ProjectDetailsViewModel ProjectDetailsViewModel(int id, string name, bool added)
		{
			var project = GetProject(id);
			var assignedToUser = "";
			if (project.AssignedTo != null)
				assignedToUser = project.AssignedTo.UserID;

			var user = GetUser(name);

			if (user != null)
				base.AddHistory(id);

			var model = new ProjectDetailsViewModel(project)
			{
				User = user,
				CommentAdded = added,
				ResourceUsers = ItUsers().Where(x => x.UserID != assignedToUser),
				LinkedWorkItems = LinkedWorkItems(id),
				SubscribeUsers = SubscribeUsers(project)
			};
			return model;
		}

		private Project GetProject(int id)
		{
			return new ListSingleProject { id = id }
				.GetQuery(_session).List<Project>().First();
		}

		public int SubmitSignOff(int signOffId, string name, SeverityEnum? severity)
		{
			var signOff = _wiSignoffRepo.Get(signOffId);
			var project = GetProject(signOff.Workitem.Id);
			var user = GetUser(name);
			var signOffToUpdate = project.SignOffs.FirstOrDefault(x => x.Id == signOff.Id);
			if (signOffToUpdate.Id <= 0 || signOffToUpdate.SignedOff != null)
				return project.Id;

			var severityChanged = false;
			var oldSeverity = project.Severity;
			if (severity.HasValue)
			{
				severityChanged = severity.Value != project.Severity;
			}
			project.Severity = (severityChanged ? severity.Value : project.Severity);

			//Add comment
			if (signOffToUpdate.SignOffType == SignOffTypeEnum.ChangeBoardAcceptance)
			{
				var text = string.Format("This {0} has been approved by {1} on behalf of the Change Board. {2}",
										 project.WorkItemType.GetDescription().ToUpper(), user.Name,
										 (severityChanged
											? "<br/>Severity changed from " + oldSeverity + " to " + project.Severity
											: ""));
				var comment = new Comment
								{
									CommentText = text,
									Type = CommentTypeEnum.SeverityChange,
									UnitsOfWork = 0
								};
				_commentRepo.Save(comment);
				_commentService.AddComment(project, comment, user);
			}

			project.UpdateSignOff(signOff, user);
			DomainEvents.Raise(new WorkItemSignOffCompleted { WorkItem = project });

			if (project.Status == ProjectStatus.Completed)
				DomainEvents.Raise(new ProjectClosed { Project = project });

			UpdateWorkItem(project, user);
			return project.Id;
		}

		public ProjectCompleteViewModel ProjectCompleteViewModel(int id, string name, bool added)
		{
			var project = GetProject(id);
			var model = new ProjectCompleteViewModel(project)
			{
				User = GetUser(name),
				CommentAdded = added
			};
			return model;
		}

		public void CompleteProject(int id, string name, Project theProject)
		{
			var user = GetUser(name);
			var project = GetProject(id);
			project.CompleteProject(user, theProject);
			DomainEvents.Raise(new ProjectCompletionDetailsUpdated { WorkItem = project });
			UpdateWorkItem(project, user);
		}

		public void RejectProject(int id, WorkItemClosureReason reason, string name)
		{
			var project = GetProject(id);
			var user = GetUser(name);
			if (reason != WorkItemClosureReason.WorkCompleted)
			{
				var text = string.Format("{0} has been rejected by {1} for reason: {2}", project.WorkItemType.GetDescription(), user.Name, reason.GetDescription());
				var comment = new Comment { CommentText = text, Type = CommentTypeEnum.SeverityChange, UnitsOfWork = 0 };
				_commentRepo.Save(comment);
				_commentService.AddComment(project, comment, user);
			}

			project.Reject(user);
			DomainEvents.Raise(new ProjectClosed { Project = project });
		}
	}
}
