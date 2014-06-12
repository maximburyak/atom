using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Services
{
	public class SubscriptionUserEqualityComparer : IEqualityComparer<User>
	{
		public bool Equals(User x, User y)
		{
			// Check whether the compared objects reference the same data.
		if (ReferenceEquals(x, y)) 
			return true;

		// Check whether any of the compared objects is null.
		if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
			return false;

			return x.UserID == y.UserID;
		}

		public int GetHashCode(User obj)
		{
			return obj.UserID.GetHashCode();
		}
	}
}