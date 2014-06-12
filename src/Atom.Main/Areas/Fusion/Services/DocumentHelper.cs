using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain.Models;
using BeValued.Utilities.Extensions;

namespace Atom.Main.Areas.Fusion.Services
{
	public static class DocumentHelper
	{
		public static string DocumentLinks(this HtmlHelper helper, IEnumerable<Document> documents, WorkItemTypeEnum type)
		{
			var sb = new StringBuilder();
			var i = 1;
			foreach (var document in documents)
			{
				sb.Append("<li class=\"case-document-list\">{0}</li>".With(DocumentLink(helper, i++, document, type)));
			}
			return documents.Any() ? sb.ToString() : "<li class=\"case-document-list\">No documents uploaded</li>";
		}

		private static string DocumentLink(this HtmlHelper helper, int i, Document document, WorkItemTypeEnum type)
		{
			var tb = new TagBuilder("a") { InnerHtml = "<span>{0}. {1} (V{2})</span>".With(i, document.FileName, decimal.Round(document.Revision, 1)) };
			tb.Attributes.Add("href", "/Fusion/{0}/Document/{1}".With(type.GetDescription(), document.Id));
			return tb.ToString();
		}

	}
}
