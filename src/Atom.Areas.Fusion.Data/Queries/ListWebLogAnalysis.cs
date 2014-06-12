using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Atom.Areas.Fusion.Domain;
using BeValued.Utilities.Utilities;
using NHibernate;
using Atom.Areas.Fusion.Domain.Models;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;
using NHibernate.Transform;
using Expression = NHibernate.Criterion.Expression;
using NHibernate.SqlCommand;

namespace Atom.Areas.Fusion.Data.Queries
{
	public class ListWebLogAnalysis : IQuery
	{
		public const int DefaultMaxResults = 5;
		
		public bool IsSortDesc			{ get; set; }
		public string SortColumn		{ get; set; }
		public DateTime? FromDate		{ get; set; }
		public DateTime? ToDate			{ get; set; }
		public int MaxResults			{ get; set; }
		public string Website			{ get; set; }
		public string PathFilter		{ get; set; }
		public string AccessCountFrom	{ get; set; }
		public string AccessCountTo		{ get; set; }

		public ListWebLogAnalysis()
		{
			if (MaxResults == 0)
				MaxResults = 100;

			FromDate = null;
			ToDate = null;
		}

		public ICriteria GetQuery(ISession session)
		{
			var id = PropertyUtil.GetName<WebLogAnalysis>(x => x.Id);
			var website = PropertyUtil.GetName<WebLogAnalysis>(x => x.Website);
			var accessCount = PropertyUtil.GetName<WebLogAnalysis>(x => x.AccessCount);
			var firstAccessed = PropertyUtil.GetName<WebLogAnalysis>(x => x.FirstAccessed);
			var lastAccessed = PropertyUtil.GetName<WebLogAnalysis>(x => x.LastAccessed);
			var processingTime = PropertyUtil.GetName<WebLogAnalysis>(x => x.ProcessingTime);
			var avgProcessingTime = PropertyUtil.GetName<WebLogAnalysis>(x => x.AvgProcessingTime);
			var webpath = PropertyUtil.GetName<WebLogAnalysis>(x => x.WebPath);
			var path = PropertyUtil.GetName<WebLogAnalysis>(x => x.Path);
			var fileName = PropertyUtil.GetName<WebLogAnalysis>(x => x.FileName);
			var extension = PropertyUtil.GetName<WebLogAnalysis>(x => x.Extension);

			var logTime = PropertyUtil.GetName<CompoundWebLog>(x => x.LogTime);
			var relativePath = PropertyUtil.GetName<CompoundWebLog>(x => x.RelativePath);

			var countProjection = LambdaProjection.Count<CompoundWebLog>(x => x.LogTime);
			var projection = Projections.ProjectionList()
				.Add(LambdaProjection.Max<CompoundWebLog>(x => x.FileId), id)
				.Add(countProjection, accessCount)
				//.Add(LambdaProjection.Max<CompoundWebLog>(x => x.Website), website)
				.Add(LambdaProjection.Min<CompoundWebLog>(x => x.LogTime), firstAccessed)
				.Add(LambdaProjection.Max<CompoundWebLog>(x => x.LogTime), lastAccessed)
				.Add(LambdaProjection.Sum<CompoundWebLog>(x => x.TimeTaken), processingTime)
				.Add(LambdaProjection.Max<CompoundWebLog>(x => x.FullName), path)
				.Add(LambdaProjection.Max<CompoundWebLog>(x => x.Name), fileName)
				.Add(LambdaProjection.Max<CompoundWebLog>(x => x.Extension), extension)
				.Add(LambdaProjection.GroupProperty<CompoundWebLog>(x => x.RelativePath), webpath)
				.Add(LambdaProjection.GroupProperty<CompoundWebLog>(x => x.Website), website);

			ICriteria criteria = session.CreateCriteria<CompoundWebLog>();

			// SQL Where clause
			if (IsNotNull(FromDate))			criteria.Add(Expression.Ge(logTime, FromDate));
			if (IsNotNull(ToDate))				criteria.Add(Expression.Lt(logTime, ToDate));
			if (IsNotNull(Website))				criteria.Add(Expression.Eq(website, Website));
			if (IsNotNullOrEmpty(PathFilter))	criteria.Add(Expression.InsensitiveLike(relativePath, PathFilter.Replace("*", "%")));
			
			// SQL Having clause			
			if (IsNotNullOrEmpty(AccessCountFrom))	criteria.Add(Restrictions.Ge(countProjection, AccessCountFrom));
			if (IsNotNullOrEmpty(AccessCountTo))	criteria.Add(Restrictions.Le(countProjection, AccessCountTo));

			criteria.SetProjection(projection);
			criteria.AddOrder(
				(IsSortDesc)
					? Order.Desc(SortColumn)
					: Order.Asc(SortColumn));

			criteria.SetResultTransformer(new AliasToBeanResultTransformer(typeof (WebLogAnalysis)));
			criteria.SetMaxResults(MaxResults);

			return criteria; 
		}

		private static bool IsNotNullOrEmpty(string str)
		{
			return IsNotNull(str) && IsNotEmpty(str);
		}

		private static bool IsNotNull(object obj)
		{
			return obj != null;
		}

		private static bool IsNotEmpty(string str)
		{
			return str != "";
		}

		private string WebLogProp(Expression<Func<WebLog>> exp)
		{
			return ReflectionUtil.GetPropertyName<WebLog>(exp);
		}



	}
}
