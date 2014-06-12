<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<Atom.Main.Areas.Fusion.Models.ViewModels.BaseWorkItemViewModel>" %>
<%if (ViewData.Model.WorkItem.WorkStatus < Atom.Areas.Fusion.Domain.Models.WorkItemStatus.Closed && Request.IsAuthenticated)
  { %>
<link href="<%=Url.Stylesheet("wmd.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Url.JavascriptFusion("showdown.js") %>" type="text/javascript"></script>
<script src="<%=Url.JavascriptFusion("wmd.js") %>" type="text/javascript"></script>
<%} %>
