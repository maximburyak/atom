using System.Collections.Generic;
using Atom.Areas.Fusion.Data.Queries.Search.Tokens;

namespace Atom.Areas.Fusion.Data.Queries.Search.TokenManagers
{
	public class DigitTokenManager:TokenManager
	{
		public DigitTokenManager()
		{
			SearchTokens = new List<ISearchToken> { new WorkItemDigitToken()};
		}
	}
}