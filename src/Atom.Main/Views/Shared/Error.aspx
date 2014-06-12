<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Error.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>
<%@ Import Namespace="BeValued.Mvc.Extensions" %>

<asp:Content ID="errorContent" ContentPlaceHolderID="error" runat="server">
	<div id="head">
		<h1 id="http500">
			500: Server Error
		</h1>
		<div>
			<p>
				Sorry, an error occurred while processing your request. Details of this error have
				been logged.</p>
			<%if (Model != null)
	 { %>
			Message:
			<%=Model.Exception.Message%>
			<%
	
				var controller = ViewContext.Controller as Atom.Main.Controllers.BaseController;
				if (controller == null)
					return;

				var logger = log4net.LogManager.GetLogger(ViewContext.Controller.ToString());
				if (logger == null)
					return;

				logger.Error(ViewContext.Controller.ControllerContext.HttpContext.FormatOutputToPlainText(Model.Exception));
	 }%>
		</div>
	</div>
</asp:Content>
