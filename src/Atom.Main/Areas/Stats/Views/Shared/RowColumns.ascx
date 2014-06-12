<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Atom.Main.Areas.Stats.Services" %>
<%@ Import Namespace="System.ComponentModel" %>

	<%
		foreach (var property in Model.GetType().GetProperties())
		{
			var value = property.GetValue(Model, null) ?? "";
			var columnName = property.Name;
			var formattedValue = FormatHelper.TryFormatObject(value);
			var colDisplayName = columnName;
			
			var attrs = (DisplayNameAttribute)Attribute.GetCustomAttribute(property, typeof(DisplayNameAttribute));
			if (attrs != null)
				colDisplayName = attrs.DisplayName;
			%>
			<td 
				columnName="<%=columnName%>" 
				class="<%=FormatHelper.IdentifyClass(value)%>"
				title="<%=colDisplayName%>: <%=formattedValue%>"
			>
				<%=formattedValue%>
			</td>
			<%
		}
	%>