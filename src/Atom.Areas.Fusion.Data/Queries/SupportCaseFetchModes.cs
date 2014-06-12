using NHibernate;

namespace Atom.Areas.Fusion.Data.Queries
{
	public static class SupportCaseFetchModes
	{
		public static ICriteria SetFetchModes(this ICriteria criteria)
		{
			criteria.SetFetchMode("System", FetchMode.Eager)
				.SetFetchMode("System.Area", FetchMode.Eager)
				.SetFetchMode("System.Category", FetchMode.Eager)
				.SetFetchMode("AssignedTo.Profile", FetchMode.Eager)
				.SetFetchMode("CreatedBy.Profile", FetchMode.Eager);
			return criteria;
		}
	}
}