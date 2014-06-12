using System.ComponentModel;
namespace Atom.Areas.Fusion.Domain.Models
{
	public enum SignOffTypeEnum
	{
		[Description("Emergency Change 1")]
		EmergencyChange1 = 1,
		[Description("Emergency Change 2")]
		EmergencyChange2 = 2,
		[Description("Emergency Change 3")]
		EmergencyChange3 = 3,
		[Description("Change Board Acceptance")]
		ChangeBoardAcceptance = 5,
		[Description("Assigned")]
		Assigned = 8,
		[Description("Plan & Design")]
		PlanDesign = 9,
		[Description("IT Development")]
		ITDevelopment = 10,
		[Description("IT Peer Review")]
		ITPeerReview = 15,
		[Description("IT Unit Testing")]
		ITUnitTesting = 20,
		[Description("Business Testing")]
		BusinessTesting = 30,
		[Description("IT Rollout")]
		ITRollout = 40, // IT Roll Out was 30, Business Testing was 40
		[Description("Business Owner")]
		BusinessOwner = 50
	}
}
