using System;
using System.Security.Principal;

namespace Atom.Main.Domain
{
	[Serializable]
	public class Identity : MarshalByRefObject, IIdentity
	{
		readonly bool _isAuthenticated;
		readonly string _name;

		public Identity(bool isAuthenticated, string name)
		{
			_isAuthenticated = isAuthenticated;
			_name = name;
		}

		public string AuthenticationType
		{
			get { return "Forms"; }
		}

		public bool IsAuthenticated
		{
			get { return _isAuthenticated; }
		}

		public string Name
		{
			get { return _name; }
		}
	}
}
