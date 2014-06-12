<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Atom.Areas.Suppliers.Domain.Models.FormatType_Class>>" %>
<option value="">Please choose</option>
<%foreach (var i in Model)
  {  %>
  <option value="<%=i.Class.Class%>"><%=i.Class.ClassDescription %></option>
<%} %>