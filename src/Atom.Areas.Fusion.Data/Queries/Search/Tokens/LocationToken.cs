using System;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class LocationToken : BaseSearchToken, ISearchToken
	{
		public LocationToken() : base("location", 3, "location", "Dept") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>({3}))", RegExShort(), RegExLong(), TokenName(), string.Join("|", new LocationEnum().ToLowerArray()));
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" In Location(s): \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;

			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
			.Add<SupportIncident>(x => x.Location == (Location)Enum.Parse(typeof(Location), _value.ToString().Replace(" ", ""), true));
			return searchCriteria;
		}
	}
}