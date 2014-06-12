<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Atom.Areas.Suppliers.Domain.Models.Format_FormatType>>" %>
<option value="">Please choose</option>
<%foreach (var i in Model)
  {  %>
  <option value="<%=i.FormatType.FormatTypeCode%>"><%=i.FormatType.FormatTypeDescription %></option>
<%} %>