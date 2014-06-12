using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Data.NHibernate;
using BeValued.Utilities.Extensions;
using BeValued.Utilities.MVC.Services;
using NHibernate;

namespace Atom.Main.Areas.Fusion.Services
{
	public class FusionCacheManager
	{
		private readonly ISession _session;
		private readonly CacheService _cacheService;
		private NHibernateRepository<Area> SupportAreaRepo { get; set; }
		private NHibernateRepository<Category> AreaCategoryRepo { get; set; }
		private NHibernateRepository<ClosureReason> ClosureRepo { get; set; }
		private NHibernateRepository<User> UserRepo { get; set; }
		private NHibernateRepository<Channel> ChannelRepo { get; set; }
		private NHibernateRepository<InsuranceCompany> InsuranceCompanyRepo { get; set; }
		private NHibernateRepository<Supplier> SupplierRepo { get; set; }
        private NHibernateRepository<ProductGroup> ProductGroupsRepo { get; set; }
        private NHibernateRepository<Location> LocationsRepo { get; set; }

		public FusionCacheManager(ISession session)
		{
			_session = session;
		    LocationsRepo = new NHibernateRepository<Location>(_session);
		    _cacheService = new FusionCacheService();
			SupportAreaRepo = new NHibernateRepository<Area>(_session);
			AreaCategoryRepo = new NHibernateRepository<Category>(_session);
			ClosureRepo = new NHibernateRepository<ClosureReason>(_session);
			UserRepo = new NHibernateRepository<User>(_session);
			ChannelRepo = new NHibernateRepository<Channel>(_session);
			InsuranceCompanyRepo = new NHibernateRepository<InsuranceCompany>(_session);
			SupplierRepo = new NHibernateRepository<Supplier>(_session);
			ProductGroupsRepo = new NHibernateRepository<ProductGroup>(_session);
		}

		public IEnumerable<User> ResourceUsers()
		{
			//get all users 
			var allUsers = AllUsers();
			var roleUsers = RoleManager.GetUsersInRole("Fusion.ResourceUser");

			// Get only users where they have the available role.
			var resourceUsers = (from r in roleUsers
								 join a in allUsers on r equals a.UserID
								 select new User { Id = a.Id, Name = a.Name }).ToList();
			return resourceUsers;
		}

		public IEnumerable<User> AllUsers()
		{
			var allUsers = _cacheService.Get("fusion-users", () => new ListTeamUsers().GetQuery(_session).List<User>(), 720);
			return allUsers;
		}

		public IEnumerable<Area> SupportArea()
		{
			var supportareas = _cacheService.Get("fusion-supportareas",
												() => new ListSupportAreas().GetQuery(_session).List<Area>());
			return supportareas;
		}

        public IEnumerable<Location> SupportLocation()
        {
            var supportlocations = _cacheService.Get("fusion-supportlocations",
                                                () => new ListSupportLocations().GetQuery(_session).List<Location>());
            return supportlocations.OrderBy(x=>x.Name).ToList();
        }

		public IList<Category> SupportAreaCategories(int areaId)
		{
            var areas = _cacheService.Get("fusion-areacategory-" + areaId,
											  () =>
											  new ListCategoriesForArea { Id = areaId }.GetQuery(
												_session).List<Area>());
		    var categories = (areas.Count>0) ? areas.First().Categories : new List<Category>();

			return categories.OrderBy(x=>x.Description).ToList();
		}

		public IEnumerable<AdditionalInfoType> AdditionalInformation(int categoryId)
		{
			var infos = _cacheService.Get("fusion-additionalinfos-" + categoryId,
										 () =>
										 new ListAdditionalInfoForCategory { Id = categoryId }.GetQuery(_session)
											.List<Category>().First().AdditionalInfo);

			return infos;
		}

		public IList<Channel> Channels()
		{
			var channels = _cacheService.Get("fusion-channels",
											  () =>
											  ChannelRepo.FindAll(null).OrderBy(x => x.OrderBy).ToList());
			return channels;
		}

        public IEnumerable<ClosureReason> ClosureReasons(string handlingDepartmentId)
        {
            var closurereasons = _cacheService.Get("fusion-closurereasons",
                                                   () => ClosureRepo.FindAll(null))
                                              .Where(x => x.Department == "*" || x.Department == handlingDepartmentId)
                                              .Where(x=>x.Enabled)
                                              .OrderBy(x => x.Description);
            
			return closurereasons;
		}

		public IList<InsuranceCompany> InsuranceCompanies()
		{
			var inscos = _cacheService.Get("fusion-insurancecompanies",
											  () =>
											  InsuranceCompanyRepo.FindAll(null)).ToList();
			return inscos;
		}

		public IList<Supplier> Suppliers()
		{
			var suppliers = _cacheService.Get("fusion-suppliers",
											  () =>
											  SupplierRepo.FindAll(null).Where(x => x.Name != "").OrderBy(x => x.OrderBy).ThenBy(x => x.Name).ToList());
			return suppliers;
		}

		public IList<ProductGroup> ProductGroups()
		{
			var productgroups = _cacheService.Get("fusion-productgroups",
											  () =>
											  ProductGroupsRepo.FindAll(null)).OrderBy(x => x.OrderBy).ThenBy(x => x.Name).ToList();
			return productgroups;
		}

		public IDictionary<int, string> RequiredCrfSignOffs()
		{
			return new SignOffTypeEnum().ToDictionary();
		}

		public IList<User> UsersDepartment(User user)
		{
			var users = _cacheService.Get(user.UserID + "s-deptusers", () =>
				new ListTeamUsers { dept = user.Department.Description }.GetQuery(_session).List<User>(), 120);
			return users;
		}

		public IList<User> UsersInDepartment(User user, HandlingDepartment handlingdept)
		{
			var users = _cacheService.Get(user.UserID + "s-usersindept", () =>
				new ListTeamUsers { dept = handlingdept.Description }.GetQuery(_session).List<User>(), 120);
			return users;
		}

		public IList<User> UsersInTeam(User user)
		{
			var users = _cacheService.Get(user.UserID + "s-usersinteam", () =>
				new ListTeamUsers { Team = user.Team }.GetQuery(_session).List<User>(), 120);
			return users;
		}

		public void RemoveCacheItems()
		{
			_cacheService.RemoveAll();
		}

		public void RemoveCacheId(string cacheId)
		{
			_cacheService.Remove(cacheId);
		}
	}

	public class FusionCacheService : CacheService
	{
		public override string CacheIndexKey
		{
			get { return "fusion"; }
		}
	}
}