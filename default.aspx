<%@ Page Language="C#" %>
<% 
if (Request.Url.Host.ToUpperInvariant() == "WWW.EASTSUSSEX.GOV.UK") 
{
    EsccWebTeam.Data.Web.Http.Status301MovedPermanently("https://new.eastsussex.gov.uk");
}
else
{
    EsccWebTeam.Data.Web.Http.Status303SeeOther("default.htm");
}
%>
<%-- Microsoft CMS needs redirect to find home page --%>