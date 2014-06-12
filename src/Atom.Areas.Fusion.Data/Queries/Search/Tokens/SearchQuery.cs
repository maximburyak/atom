using System.Collections.Generic;
using NHibernate.Criterion;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public class SearchQuery
	{
		public DetachedCriteria Criteria { get; set; }
		public Dictionary<string, Disjunction> Disjunctions { get; set; }
		public Dictionary<string, Conjunction> Conjunctions { get; set; }

		public SearchQuery()
		{
			Disjunctions = new Dictionary<string, Disjunction>();
			Conjunctions = new Dictionary<string, Conjunction>();
		}

		public void AddTokenJunctions(string tokenName)
		{
			Disjunctions.Add(tokenName, null);
			Conjunctions.Add(tokenName, null);
		}

		public void CreateDefaultDisjunction(string tokenName)
		{
			if (Disjunctions[tokenName] == null)
				Disjunctions[tokenName] = Restrictions.Disjunction();
		}

		public void CreateDefaultConjunction(string tokenName)
		{
			if (Conjunctions[tokenName] == null)
				Conjunctions[tokenName] = Restrictions.Conjunction();
		}

		public void AddAllJunctions()
		{
			foreach (var c in Conjunctions)
			{
				if (c.Value != null)
					Criteria.Add(c.Value);
			}
			foreach (var c in Disjunctions)
			{
				if (c.Value != null)
					Criteria.Add(c.Value);
			}
		}
	}
}