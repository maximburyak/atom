<%@ Page Language="C#" MasterPageFile="~/Areas/Fusion/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>
<%@ Import Namespace="BeValued.Mvc.Extensions" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Atom Fusion --> Error
</asp:Content>
<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="fixed">
		<div id="pagetitle">
			<h1>
				An error has occurred</h1>
		</div>
		<div class="box-error">
			<h3>
				Message:
				<%=Model.Exception.Message%></h3>
			<br />
			<p class="form-summary">
				The issue has been logged and we will investigate it as soon as possible.
			</p>
		</div>
	</div>
	<%
		var controller = ViewContext.Controller as Atom.Main.Areas.Fusion.Controllers.BaseController;
		if (controller == null)
			return;

		var logger = log4net.LogManager.GetLogger(ViewContext.Controller.ToString());
		if (logger == null)
			return;

		logger.Error(ViewContext.Controller.ControllerContext.HttpContext.FormatOutputToPlainText(Model.Exception));                
	%>
</asp:Content>
