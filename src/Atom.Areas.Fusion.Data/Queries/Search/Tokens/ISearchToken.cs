using System.Collections.Generic;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public interface ISearchToken
	{
		string RegularExpression();
		string TokenName();
		string DisplayText(string[] values);
		IList<SearchExample> Examples();
		SearchQuery CreateCriteria(SearchQuery searchCriteria, string value);
	}
}