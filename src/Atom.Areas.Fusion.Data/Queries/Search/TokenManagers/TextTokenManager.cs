using System.Collections.Generic;
using Atom.Areas.Fusion.Data.Queries.Search.Tokens;

namespace Atom.Areas.Fusion.Data.Queries.Search.TokenManagers
{
	public class TextTokenManager:TokenManager
	{
		public TextTokenManager()
		{
			SearchTokens = new List<ISearchToken> { new WorkItemTextToken() };
		}
	}
}