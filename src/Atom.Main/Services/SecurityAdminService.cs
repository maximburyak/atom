using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using Atom.Main.Domain;
using Atom.Main.Models.ViewModels;

namespace Atom.Main.Services
{
	public class SecurityAdminService
	{
		private string connStr { get; set; }
		private string ApplicationName { get; set; }

		public SecurityAdminService(string connectionString, string ApplicationName)
		{
			connStr = connectionString;
			this.ApplicationName = ApplicationName;
		}

		public List<SecurityUser> ListUsers()
		{
			return new List<SecurityUser>();
		}

		private List<SecurityRole> GetRolesAvailableForUser(string userid, params string[] rolestoExclude)
		{
		    return new List<SecurityRole>();
		}

		public SecurityAdminViewModel SecurityAdminViewModel()
		{
			// get atomUsers users, and non atom users
			var users = ListUsers();
			var atomUsers = (from y in users
							 where y.applicationAccess == 1
							 select new SecurityUser { id = y.id, FullName = y.FullName, userid = y.userid }).ToList();
			var wildUsers = (from y in users
							 where y.applicationAccess == 0
							 select new SecurityUser { id = y.id, FullName = y.FullName, userid = y.userid }).ToList();

			var allUsers = new List<SecurityUser>(atomUsers);
			allUsers.AddRange(wildUsers);

			var model = new SecurityAdminViewModel
							{
								AllUsers = allUsers,
								AtomUsers = atomUsers,
								WildUsers = wildUsers,
								HasRoles = new List<SecurityRole>(),
								HaveRoles = new List<SecurityRole>()
							};
			return model;
		}

		public List<SecurityRole> GetExistingRoleForUser(string id)
		{
			var roles = (from r in RoleManager.GetRolesForUser(id)
						 select new SecurityRole(r, r)).ToList();
			return roles;
		}

		public List<SecurityRole> GetRolesForUser(string id)
		{
			return GetRolesAvailableForUser(id, ((RoleManager.IsUserInRole("Fusion.IT")) ? "" : "Fusion.IT"));
		}
	}
}
