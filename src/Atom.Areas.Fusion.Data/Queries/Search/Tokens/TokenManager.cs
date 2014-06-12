using System.Collections.Generic;
using System.Text.RegularExpressions;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class TokenManager
	{
		public List<ISearchToken> SearchTokens;
		public Dictionary<ISearchToken, List<string>> SearchMatches;

		public int NumberofMatches;

		public TokenManager()
		{
			SearchMatches = new Dictionary<ISearchToken, List<string>>();
			SearchTokens = new List<ISearchToken>();
		}

		public bool HasMatchedSearch()
		{
			return NumberofMatches > 0;
		}

		public void RunSearch(string search)
		{
			var regEx = CreateRegExFromSearchTokens();
			var regexPattern = new Regex(regEx, RegexOptions.IgnoreCase);
			var m = regexPattern.Match(CleanSearchString(search));

			while (m.Success)
			{
				foreach (var t in SearchTokens)
				{
					var tokenName = t.TokenName();
					if (!m.Groups[tokenName].Success) continue;
					var value = m.Groups[tokenName].Value;

					AddMatch(t, value);
					NumberofMatches++;
				}
				m = m.NextMatch();
			}
		}

		public SearchQuery CreateCriteria(int maxcase)
		{
			var criteria = new BaseSearch { Maxcase = maxcase }.GetQuery();
			var searchQuery = new SearchQuery { Criteria = criteria };

			foreach (var match in SearchMatches)
			{
				searchQuery.AddTokenJunctions(match.Key.TokenName());
				foreach (var item in match.Value)
				{
					searchQuery = match.Key.CreateCriteria(searchQuery, item);
				}
			}

			searchQuery.AddAllJunctions();
		    searchQuery.Criteria.AddOrder<WorkItem>(x => x.Id, Order.Desc);
			return searchQuery;
		}

		public string DisplayText()
		{
			var s = "";
			var i = 0;
			foreach (var match in SearchMatches)
			{
				s += match.Key.DisplayText(match.Value.ToArray());
				if (i >=0 && i< SearchMatches.Count-1)
					s += ", and";

				i++;
			}
			return s;
		}

		private void AddMatch(ISearchToken token, string matchedValue)
		{
			if (SearchMatches.ContainsKey(token))
				SearchMatches[token].Add(matchedValue);
			else
				SearchMatches.Add(token, new List<string> { matchedValue });
		}

		private string CreateRegExFromSearchTokens()
		{
			var regex = "";
			foreach (var t in SearchTokens)
			{
				regex += t.RegularExpression() + "|";
			}

			return regex.Substring(0, regex.Length - 1);
		}

		private string CleanSearchString(string searchString)
		{
			return searchString.Trim().Replace("  ", " ");
		}

	}
}