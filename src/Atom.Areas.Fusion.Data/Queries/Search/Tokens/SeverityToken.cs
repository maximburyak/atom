using System;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class SeverityToken : BaseSearchToken, ISearchToken
	{
		public SeverityToken() : base("priority", 3, "priority", "Low, Medium or High") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>({3}))", RegExShort(), RegExLong(), TokenName(), string.Join("|", new SeverityEnum().ToLowerDescriptionArray(@"\s?")));
		}

		public override string DisplayText(string[] values)
		{
			return string.Format(" with prioriti(es) of: \"{0}\"", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;

			searchCriteria.CreateDefaultDisjunction(TokenName());
			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.Severity == (SeverityEnum)Enum.Parse(typeof(SeverityEnum), _value.ToString().Replace(" ", ""), true));
			return searchCriteria;
		}
	}
}