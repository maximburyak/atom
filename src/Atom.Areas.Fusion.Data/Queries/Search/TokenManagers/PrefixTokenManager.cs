using System.Collections.Generic;
using Atom.Areas.Fusion.Data.Queries.Search.Tokens;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Areas.Fusion.Data.Queries.Search.TokenManagers
{
	public class PrefixTokenManager : TokenManager
	{
		public PrefixTokenManager()
		{
			SearchTokens = new List<ISearchToken>
			               	{
								new AssignedNameToken(),
								new AssignedToken(),
								new WorkItemToken(),
								new WorkItemTypeIndividualToken(WorkItemTypeEnum.Crf),
								new WorkItemTypeIndividualToken(WorkItemTypeEnum.Project),
								new WorkItemTypeIndividualToken(WorkItemTypeEnum.Incident),
								new AssignedRequireSignOffToken(),
								new NextSignOffToken(),
								new RaisedToken(),
								new RaisedNameToken(),
								new FromDateToken(),
								new ToDateToken(),
								new StatusToken(),
								new InfrastructureDailyWorkToken(),
								new LastUpdatedByToken(),
								new LocationToken(),
								new LastUpdatedByNameToken(),
								new HandlingDepartmentToken(),
								new WorkItemTypeToken(),
								new WorkItemHistoryToken(),
								new ChangeBoardMeetingToken(),
								new HouseKeepingToken(),
								new ClientRequirementToken(),
								new SeverityToken()
			               	};
		}
	}
}