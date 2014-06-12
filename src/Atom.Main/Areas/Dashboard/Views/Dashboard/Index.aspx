<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<DashboardViewModel>" MasterPageFile="~/Areas/Dashboard/Views/Shared/Site.Master" %>

<%@ Register src="StartingGrid.ascx" tagname="StartingGrid" tagprefix="uc1" %>
<%@ Register src="MainGrid.ascx" tagname="MainGrid" tagprefix="uc2" %>
<%@ Register src="WinnersPodium.ascx" tagname="WinnersPodium" tagprefix="uc3" %>
<%@ Register src="OutstandingStats.ascx" tagname="OutstandingStats" tagprefix="uc4" %>
<%@ Register src="CompletedStats.ascx" tagname="CompletedStats" tagprefix="uc5" %>

<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">IT Grand Prix</asp:Content>

<asp:Content runat="server" ID="HeadInclude" ContentPlaceHolderID="HeadInclude">
	<%--<meta http-equiv="refresh" content="300" />--%>
	<link href="/Areas/Dashboard/Content/css/complex.css" rel="stylesheet" type="text/css" />
	<link href="/Areas/Dashboard/Content/css/xp-style.css" rel="stylesheet" type="text/css" />
	<script src="/Areas/Dashboard/Content/Scripts/jquery-ui.js" type="text/javascript"></script>
	<script src="/Areas/Dashboard/Content/Scripts/jquery-ui-layout.js" type="text/javascript"></script>
	<script src="/Areas/Dashboard/Content/Scripts/jquery.cookie.js" type="text/javascript"></script>
	<script src="/Areas/Dashboard/Content/Scripts/Dashboard.js" type="text/javascript"></script>
	<script src="/Areas/Dashboard/Content/Scripts/ietextshadow.js" type="text/javascript"></script>
</asp:Content>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

	<div id="logo">
		<img src="/Areas/Dashboard/Content/images/fusionlogo.png" />
	</div>
	<div id="dashboardlogo"></div>
	
	<div id="html-tooltip">
		<div id="tooltip-header">
			<div class="btnclose">[ Close ]</div>
		</div>
		<div id="tooltip-content"></div>
		<div id="tooltip-footer"></div>
	</div>

	<div class="loading">
		<div id="loading-msg">Loading...</div>
	</div>
	
	<div id="mainContent" class="ui-layout-center">		
		<uc2:MainGrid ID="Grid2" runat="server" />
	</div>

	<div class="ui-layout-north">
		
		<%--<div class="stats">
			<uc4:OutstandingStats id="Stats1" runat="server"/>
			<uc5:CompletedStats ID="stats2" runat="server" />
		</div>
		<div class="title"></div>
		<div class="stats">
			
		</div>--%>
	</div>

	<div class="ui-layout-west">
		<uc1:StartingGrid ID="Grid1" runat="server" />
	</div>

	<div class="ui-layout-east">
		<uc3:WinnersPodium ID="Grid3" runat="server" />
	</div>

	<div class="textshadow">.</div>

</asp:Content>
