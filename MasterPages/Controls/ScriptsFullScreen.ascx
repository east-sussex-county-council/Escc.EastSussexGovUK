<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptsFullScreen.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.ScriptsFullScreen" %>
<%-- Grab Google CDN's jQuery, with a protocol relative URL; fall back to local JQuery if necessary --%>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js"></script>
<script>//<![CDATA[
    if (!window.jQuery) document.write('<script src="//www.eastsussex.gov.uk/js/libs/jquery-1.6.4.min.js">\x3C/script>')//]]></script>
<Egms:Script runat="server" Files="Config;Analytics;Statistics" EnableViewState="false" />
