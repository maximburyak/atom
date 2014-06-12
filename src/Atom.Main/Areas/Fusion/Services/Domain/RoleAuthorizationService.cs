using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Models.ViewModels;
using NHibernate;
using StructureMap;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
    public class RoleAuthorizationService
    {
        
        #region RoleArrays

        //Profile
        private static readonly string[] ProfileViewSignatureExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin" };
        private static readonly string[] ProfileViewSubscriptionExcludedRoles = new[] { "Fusion.User" };
        private static readonly string[] ProfileViewAutoAssignToExcludedRoles = new[] { "Fusion.User" };
        private static readonly string[] ProfileViewChangeBoardMeetingExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfAdmin", "Fusion.CrfUser", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.IT", "Fusion.COM", "Fusion.ResourceUser", "Fusion.SuperUser", "Fusion.SMT" };

        //Incident
        private static readonly string[] IncidentCreateExcludedRoles = new[] { "Fusion.User", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] IncidentAddCommentExcludedRoles = new[] { "Fusion.User", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] IncidentEnterUnitsOfWorkExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.COM", "Fusion.SMT" };
        private static readonly string[] IncidentChangeHoldStatusExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] IncidentDocumentsExcludedRoles = new[] { "Fusion.User", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] IncidentSubscriptionExcludedRoles = new[] { "Fusion.User", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] IncidentCloseExcludedRoles = new[] { "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.COM" };
        private static readonly string[] IncidentCanAssignToExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.CrfAdmin", "Fusion.COM" };
        private static readonly string[] IncidentLinksExcludedRoles = new[] { "Fusion.User", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };

        //Crf
        private static readonly string[] CrfCreateExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] CrfAddCommentExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] CrfEnterUnitsOfWorkExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.COM", "Fusion.SMT" };
        private static readonly string[] CrfChangeHoldStatusExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] CrfDocumentsExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] CrfSubscriptionExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] CrfCanAssignToExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.COM" };
        private static readonly string[] CrfCanSeeCompleteExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] CrfCanCompleteExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.COM" };
        private static readonly string[] CrfCanSeeSignaturesExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] CrfCanApproveExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfAdmin", "Fusion.CrfUser", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.IT", "Fusion.COM", "Fusion.SuperUser" };
        private static readonly string[] CrfCanApproveEmergencyExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfAdmin", "Fusion.CrfUser", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.IT", "Fusion.COM", "Fusion.SuperUser", "Fusion.SMT" };
        // In effect only Fusion.ChangeBoard can approve emergency signoffs
        private static readonly string[] CrfCanSeeEmergencyExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };
        private static readonly string[] CrfLinksExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin" };

        //Project
        private static readonly string[] ProjectCreateExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin" };
        private static readonly string[] ProjectAddCommentExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin" };
        private static readonly string[] ProjectEnterUnitsOfWorkExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.COM", "Fusion.SMT", "Fusion.SuperUser" };
        private static readonly string[] ProjectChangeHoldStatusExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin" };
        private static readonly string[] ProjectDocumentsExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin" };
        private static readonly string[] ProjectSubscriptionExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin" };
        private static readonly string[] ProjectCanAssignToExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.COM" };
        private static readonly string[] ProjectCanSeeCompleteExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.COM" };
        private static readonly string[] ProjectCanCompleteExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.COM" };
        private static readonly string[] ProjectCanSeeSignaturesExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.COM" };
        private static readonly string[] ProjectCanApproveExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfAdmin", "Fusion.CrfUser", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.IT", "Fusion.COM", "Fusion.SuperUser" };
        private static readonly string[] ProjectLinksExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin" };

        //WorkItems
        private static readonly string[] WorkItemITSignoffsExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.COM" };
        private static readonly string[] WorkItemBusinessSignOffsExcludedRoles = new[] { "Fusion.User", "Fusion.IncidentUser", "Fusion.IncidentAdmin", "Fusion.CrfUser", "Fusion.CrfAdmin", "Fusion.ProjectUser", "Fusion.ProjectAdmin", "Fusion.COM", "Fusion.IT" };

        #endregion

        #region Menuoptions

        /// <summary>
        /// Returns whether the specified user has sufficient privilege for the sub-menu option chosen
        /// </summary>
        /// <param name="requiredRoles">string[] of roles required for the submenu</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool ViewSubMenuOptions(string[] requiredRoles)
        {
            return RoleManager.GetRolesForUser().Intersect(requiredRoles).Any();
        }

        #endregion

        #region Profile

        /// <summary>
        /// Authorizes if user can view Signatures section from within Profile
        /// </summary>
        /// <returns>true if they can, false if not</returns>
        public static bool ProfileViewSignatures()
        {
            return RoleManager.GetRolesForUser().Except(ProfileViewSignatureExcludedRoles).Any();
        }

        /// <summary>
        /// Authorizes if user can view Subscriptions section from within Profile
        /// </summary>
        /// <returns>true if they can, false if not</returns>
        public static bool ProfileViewSubscriptions()
        {
            return RoleManager.GetRolesForUser().Except(ProfileViewSubscriptionExcludedRoles).Any();
        }

        /// <summary>
        /// Authorizes if user can auto assign users from within Profile
        /// </summary>
        /// <returns>true if they can, false if not</returns>
        public static bool ProfileViewAutoAssignTo()
        {
            return RoleManager.GetRolesForUser().Except(ProfileViewAutoAssignToExcludedRoles).Any();
        }

        /// <summary>
        /// Authorizes if user can alter the date for the Change Board Meeting.
        /// </summary>
        /// <returns>true if they can, false if not</returns>
        public static bool ProfileViewAlterChangeBoardMeeting()
        {
            return RoleManager.GetRolesForUser().Except(ProfileViewChangeBoardMeetingExcludedRoles).Any();
        }

        public static IEnumerable<string> GetChangeBoardMeetingRoles()
        {
            return RoleManager.GetRolesForUser().Except(ProfileViewChangeBoardMeetingExcludedRoles);
        }

        #endregion

        #region Incidents

        /// <summary>
        /// Returns whether the user has sufficient privilege to create incidents.
        /// </summary>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool IncidentCreateView()
        {
            return RoleManager.GetRolesForUser().Except(IncidentCreateExcludedRoles).Any();
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to close incidents.
        /// </summary>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool IncidentClosure()
        {
            return RoleManager.GetRolesForUser().Except(IncidentCloseExcludedRoles).Any();
        }

        #endregion

        #region Crf

        /// <summary>
        /// Returns whether the user has sufficient privilege to create crfs.
        /// </summary>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool CrfCreateView()
        {
            return RoleManager.GetRolesForUser().Except(CrfCreateExcludedRoles).Any();
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to approve an emergencysignoff
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool CrfEmergencySignOffCanApprove(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfCanApproveEmergencyExcludedRoles).Any();
                default:
                    return false;
            }
        }
        /// <summary>
        /// Returns whether the user has sufficient privilege to approve an emergencysignoff
        /// </summary>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool CrfCanSetEmergencyChange()
        {
            return RoleManager.GetRolesForUser().Except(CrfCanSeeEmergencyExcludedRoles).Any();
        }

        #endregion

        #region Projects

        /// <summary>
        /// Returns whether the user has sufficient privilege to create projects.
        /// </summary>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool ProjectCreateView()
        {
            return RoleManager.GetRolesForUser().Except(ProjectCreateExcludedRoles).Any();
        }

        #endregion

        #region WorkItems

        /// <summary>
        /// Returns whether the user has sufficient privilege to add comments
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemAddComment(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return RoleManager.GetRolesForUser().Except(IncidentAddCommentExcludedRoles).Any();
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfAddCommentExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectAddCommentExcludedRoles).Any();
            }
            return false;
        }

        private static UserSessionModel GetCurrentUser()
        {
            var session = ObjectFactory.GetInstance<ISession>();
            var currentuser = (UserSessionModel)HttpContext.Current.Session["CurrentUser"];
            if (currentuser == null)
            {
                var dbUser = session.QueryOver<User>().Where(x => x.UserID == Thread.CurrentPrincipal.Identity.Name).SingleOrDefault();
                if (dbUser != null)
                {
                    currentuser = new UserSessionModel()
                    {
                        Id = dbUser.Id,
                        Name = dbUser.Name,
                        AccessLevel = dbUser.AccessLevel,
                        Department = dbUser.Department,
                        EmailAddress = dbUser.EmailAddress,
                        Team = dbUser.Team,
                        UserFK = dbUser.UserFK,
                        UserID = dbUser.UserID
                    };
                    HttpContext.Current.Session["CurrentUser"] = currentuser;
                }
            }

            return currentuser;
        }

        /// <summary>
        /// Returns whether the user is required to enter units of work
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if user must, false if not</returns>
        public static bool UnitsOfWorkRequired(WorkItemTypeEnum workItemType)
        {
            var currentuser = GetCurrentUser();

            if (currentuser == null)
                return false;

            return currentuser.Department.Id == (int)HandlingDepartmentTypeEnum.IT || currentuser.Department.Id == (int)HandlingDepartmentTypeEnum.Guild;
        }

        /// <summary>
        /// Returns whether the user can choose closure reason
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if user must, false if not</returns>
        public static bool ShowClosureReason(WorkItemTypeEnum workItemType)
        {
            var currentuser = GetCurrentUser();

            if (currentuser == null)
                return false;

            return currentuser.Department.Id == (int)HandlingDepartmentTypeEnum.IT || currentuser.Department.Id == (int)HandlingDepartmentTypeEnum.Pmo;
        }
        
        /// <summary>
        /// Returns whether the user has sufficient privilege to place on hold
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemChangeOnHoldStatus(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return RoleManager.GetRolesForUser().Except(IncidentChangeHoldStatusExcludedRoles).Any();
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfChangeHoldStatusExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectChangeHoldStatusExcludedRoles).Any();
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to see documents
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemViewDocuments(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return RoleManager.GetRolesForUser().Except(IncidentDocumentsExcludedRoles).Any();
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfDocumentsExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectDocumentsExcludedRoles).Any();
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to manage subscriptions for a work item
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemChangeSubscription(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return RoleManager.GetRolesForUser().Except(IncidentSubscriptionExcludedRoles).Any();
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfSubscriptionExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectSubscriptionExcludedRoles).Any();
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to assign work item to another user.
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemCanAssignToUser(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return RoleManager.GetRolesForUser().Except(IncidentCanAssignToExcludedRoles).Any();
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfCanAssignToExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectCanAssignToExcludedRoles).Any();
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to see completion of work item.
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemCanSeeComplete(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return false; //Complete is N/A, close only
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfCanSeeCompleteExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectCanSeeCompleteExcludedRoles).Any();
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to complete work item.
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemCanComplete(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return false; //Complete is N/A, close only
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfCanCompleteExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectCanCompleteExcludedRoles).Any();
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to see signatures.
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemCanSeeSignatures(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return false; //Signatures is N/A
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfCanSeeSignaturesExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectCanSeeSignaturesExcludedRoles).Any();
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to approve a work item.
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemCanApprove(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return false; //No approval for incidents.
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfCanApproveExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectCanApproveExcludedRoles).Any();
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to reject a work item.
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemCanReject(WorkItemTypeEnum workItemType)
        {
            //same logic applies
            return WorkItemCanApprove(workItemType);
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to sign off IT sign offs.
        /// </summary>
        /// <param name="signOffType">Type of signoff we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemITSignOffs(SignOffTypeEnum signOffType)
        {
            return signOffType < SignOffTypeEnum.BusinessTesting && !RoleManager.GetRolesForUser().Except(WorkItemITSignoffsExcludedRoles).Any();
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to sign off Business Signoff
        /// </summary>
        /// <param name="signOffType">Type of signoff we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemBusinessSignOffs(SignOffTypeEnum signOffType)
        {
            return signOffType > SignOffTypeEnum.BusinessTesting && !RoleManager.GetRolesForUser().Except(WorkItemBusinessSignOffsExcludedRoles).Any();
        }

        /// <summary>
        /// Returns whether the user has sufficient privilege to manage links for a work item
        /// </summary>
        /// <param name="workItemType">Type of model we are dealing with</param>
        /// <returns>true if privileges are owned, false if not</returns>
        public static bool WorkItemChangeLinks(WorkItemTypeEnum workItemType)
        {
            switch (workItemType)
            {
                case WorkItemTypeEnum.Incident:
                    return RoleManager.GetRolesForUser().Except(IncidentLinksExcludedRoles).Any();
                case WorkItemTypeEnum.Crf:
                    return RoleManager.GetRolesForUser().Except(CrfLinksExcludedRoles).Any();
                case WorkItemTypeEnum.Project:
                    return RoleManager.GetRolesForUser().Except(ProjectLinksExcludedRoles).Any();
            }
            return false;
        }


        #endregion


    }
}
