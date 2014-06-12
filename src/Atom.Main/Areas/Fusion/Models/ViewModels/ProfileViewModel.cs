using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain.Models;
using Filter = Atom.Areas.Fusion.Domain.Models.Filter;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class ProfileViewModel
	{
		public User User { get; set; }
		public IList<Subscription> Subscriptions { get; set; }
		public IList<Filter> Filters { get; set; }
        public IEnumerable<User> ResourceUsers { get; set; }
        public IDictionary<int, string> DropDownListDepartments { get; set; }
        public IEnumerable<SelectListItem> DropDownListUsers { get; set;}

        public int AutoAssignedToUserId { get; set; }
        public int AutoAssignedToDepartmentId { get; set; }
        public DateTime ChangeBoardMeetingDate { get; set; }

        public ProfileViewModel(User user, IList<Subscription> subscriptions, IList<Filter> filters)
		{
			User = user;
			Subscriptions = subscriptions ?? new List<Subscription>();
			Filters = filters ?? new List<Filter>();
        }

	}
}