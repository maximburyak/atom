﻿using Atom.Areas.Fusion.Domain.Models;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{

	public class ClientRequirementToken : BaseSearchToken, ISearchToken
	{
		public ClientRequirementToken() : base("crequirement", 4, "crequirement", "True|False") { }

		public string RegularExpression()
		{
			return string.Format(@"{0}(?:{1})?:(?<{2}>({3}))", RegExShort(), RegExLong(), TokenName(), "True|False|true|false|t|f");

		}

		public override string DisplayText(string[] values)
		{
			return string.Format("With Client Requirement of: {0}", string.Join(",", values));
		}

		public override SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			_value = value;
			bool result;
			if (!bool.TryParse(value, out result))
				result = true;

			searchCriteria.CreateDefaultDisjunction(TokenName());

			searchCriteria.Disjunctions[TokenName()]
				.Add<WorkItem>(x => x.ClientRequirement == result);

			return searchCriteria;
		}
	}
}
