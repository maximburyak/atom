using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Domain;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Models.ViewModels;
using BeValued.Data.NHibernate;
using BeValued.Utilities.Extensions;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Services.Domain
{
	public class ProfileService
	{
		private ISession _session;
		private readonly NHibernateRepository<Avatar> _avatarRepo;
		private readonly NHibernateRepository<Subscription> _subscriptionRepo;
		private readonly NHibernateRepository<SupportProfile> _userProfileRepo;
		private readonly NHibernateRepository<Signature> _signatureRepo;
		private readonly NHibernateRepository<Filter> _filterRepo;
		private readonly NHibernateRepository<ChangeBoard> _changeBoardRepo;
		private readonly FusionCacheManager _cacheManager;

		public ProfileService(ISession session)
		{
			_session = session;
			_userProfileRepo = new NHibernateRepository<SupportProfile>(_session);
			_avatarRepo = new NHibernateRepository<Avatar>(_session);
			_subscriptionRepo = new NHibernateRepository<Subscription>(_session);
			_signatureRepo = new NHibernateRepository<Signature>(_session);
			_filterRepo = new NHibernateRepository<Filter>(_session);
			_changeBoardRepo = new NHibernateRepository<ChangeBoard>(_session);
			_cacheManager = new FusionCacheManager(session);
		}

		public virtual SupportProfile GetProfile(int id)
		{
			return _userProfileRepo.Get(id);
		}

		private void Save(SupportProfile profile)
		{
			_userProfileRepo.Save(profile);
		}

		private IList<Filter> Filters(string userid)
		{
			var filters = new ListUserFilters { UserID = userid }
				.GetQuery(_session).List<User>();
			return filters.Any() ? filters.First().Profile.Filters : new List<Filter>();
		}

		public void CreateProfile(User user)
		{
			var profile = new SupportProfile { UserId = user.Id, RefreshSearch = true };
			Save(profile);
		}

		public ProfileViewModel ProfileViewModel(string name)
		{
			var user = GetUser(name);
			if (user.Profile == null)
				CreateProfile(user);

			var subscriptions = new ListMySubscriptions { user = user, showOpenOnly = true }
				.GetQuery(_session).List<Subscription>();

			return new ProfileViewModel(user, subscriptions, Filters(name))
			{
				ResourceUsers = _cacheManager.ResourceUsers()
			};
		}

		public void AddAvatar(string name, SupportProfile profile, HttpFileCollectionBase files)
		{
			var user = GetUser(name);
			if (files.Count == 1)
			{
				if (files[0].ContentLength > 0)
				{
					var fileStream = files[0].InputStream;
					var uploadedImg = Image.FromStream(fileStream);
					var img = ImageHelper.GenerateThumbnail(uploadedImg, 50, 50);
					var avatar = new Avatar();
					user.Profile.Avatars.Add(avatar);
					_avatarRepo.Save(avatar);
					img.Save(HttpContext.Current.Server.MapPath("~/Areas/Fusion/Content/Images/avatars/{0}_{1}.gif".With(user.UserID, avatar.Id)), ImageFormat.Gif);
					user.Profile.CurrentAvatar = avatar.Id;
				}
			}

			if (profile.CurrentAvatar > 0)
				user.Profile.CurrentAvatar = profile.CurrentAvatar;

			Updated(user.Profile, user);
		}

		public virtual User GetUser(int id)
		{
			return new ListSingleUser { id = id }
				.GetQuery(_session).List<User>().First();
		}

		private User GetUser(string name)
		{
			return new ListSingleUser { name = name }
				.GetQuery(_session).List<User>().First();
		}

		private HandlingDepartment GetDepartment(int departmentId)
		{
			return new ListSingleHandlingDept { id = departmentId }
				.GetQuery(_session).List<HandlingDepartment>().First();
		}

		private void Updated(SupportProfile profile, User user)
		{
			_userProfileRepo.Update(user.Profile);
		}

		public Subscription GetSubscription(int id)
		{
			return _subscriptionRepo.Get(id);
		}

		public void AddSignature(string name, SupportProfile profile, HttpFileCollectionBase files)
		{
			var user = GetUser(name);
			if (files.Count == 1)
			{
				if (files[0].ContentLength > 0)
				{
					var fileStream = files[0].InputStream;
					var uploadedImg = Image.FromStream(fileStream);
					var img = ImageHelper.GenerateThumbnail(uploadedImg, 50, 150);
					var signature = new Signature();
					user.Profile.Signatures.Add(signature);
					_signatureRepo.Save(signature);
					img.Save(HttpContext.Current.Server.MapPath("~/Areas/Fusion/Content/Images/signatures/{0}_sig_{1}.gif".With(user.UserID, signature.Id)), ImageFormat.Gif);
					user.Profile.CurrentSignature = signature.Id;
				}
			}

			if (profile.CurrentSignature > 0)
				user.Profile.CurrentSignature = profile.CurrentSignature;

			Updated(user.Profile, user);
		}

		public void UpdateFilter(string userName)
		{
			var user = GetUser(userName);
			user.Profile.ShowFilters = !user.Profile.ShowFilters;
			Updated(user.Profile, user);
		}

		public void RemoveFilter(string userName, int filterid, bool removeDefaultOnly)
		{
			var user = GetUser(userName);
			var filter = Filters(userName).Where(x => x.Id == filterid).FirstOrDefault();

			if (removeDefaultOnly)
				filter.IsDefault = false;
			else
			{
				if (user != null && filter != null)
					_filterRepo.Delete(filter);
			}
			Updated(user.Profile, user);
		}

		public void FilterDefault(string userName, int filterid)
		{
			var user = GetUser(userName);
			foreach (var filter in user.Profile.Filters)
			{
				filter.IsDefault = filter.Id == filterid;
			}
			Updated(user.Profile, user);
		}

		public void AutoAssignToUser(string userName, int assignedDepartment, int assignToUserId)
		{
			var currentuser = GetUser(userName);
			var department = GetDepartment(assignedDepartment);
			var deptusers = new ListTeamUsers { dept = department.Description }.GetQuery(_session).List<User>();

			foreach (var user in deptusers.Where(user => user.Profile != null))
			{
				if (user.Id == assignToUserId)
				{
					if (user.Id == currentuser.Id)
						currentuser.Profile.IsAssignedToAuto = true;
					else
						user.Profile.IsAssignedToAuto = true;
				}
				else
				{
					if (user.Id == currentuser.Id)
						currentuser.Profile.IsAssignedToAuto = false;
					else
						user.Profile.IsAssignedToAuto = false;
				}

				Updated(null, user.Id == currentuser.Id ? currentuser : user);
			}
		}

		public int GetAutoAssignUserId(string userName)
		{
			var currentuser = GetUser(userName);
			var deptusers = new ListTeamUsers { dept = currentuser.Department.Description }.GetQuery(_session).List<User>();

			foreach (var user in deptusers)
			{
				if (user.Profile != null)
				{
					if (user.Profile.IsAssignedToAuto)
						return user.Id;
				}
			}
			return -1;
		}

		public int GetAutoAssignDepartmentId(string userName)
		{
			var currentuser = GetUser(userName);
			return currentuser.Department.Id;
		}

		public IList<User> GetUsersInMyDepartment(string userName)
		{
			var currentuser = GetUser(userName);
			return _cacheManager.UsersDepartment(currentuser);
		}

		public IList<User> GetUsersInMyTeam(string userName)
		{
			var currentuser = GetUser(userName);
			return _cacheManager.UsersInTeam(currentuser);
		}

		public IDictionary<int, string> GetDropDownListDepartments()
		{
			return new HandlingDepartmentTypeEnum().ToDictionary();
		}

		public IList ListUsersByDepartment(int departmentId)
		{
			var department = GetDepartment(departmentId);
			var teamusers = new ListTeamUsers { dept = department.Description }.GetQuery(_session).List<User>();

			var deptusers = teamusers.Where(u => u.Profile != null).Select(u => new { u.Id, u.Name, u.Profile.IsAssignedToAuto }).ToList();

			deptusers.Insert(0, new { Id = -1, Name = "Unassigned", IsAssignedToAuto = deptusers.Exists(u => !u.IsAssignedToAuto) });

			return deptusers;
		}

		public void UpdateChangeBoardMeeting(DateTime cbDate)
		{
			var changeBoard = _changeBoardRepo.Get(1);
			changeBoard.NextMeeting = cbDate;
		}

		public DateTime GetChangeBoardMeetingDate()
		{
			var changeBoard = _changeBoardRepo.Get(1);
			return changeBoard.NextMeeting;
		}
	}
}
