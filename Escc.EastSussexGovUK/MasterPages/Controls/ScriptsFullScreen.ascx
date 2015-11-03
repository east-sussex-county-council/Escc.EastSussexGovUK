<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptsFullScreen.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.ScriptsFullScreen" %>
<%-- Grab Google CDN's jQuery, with a protocol relative URL; fall back to local JQuery if necessary --%>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<ClientDependency:Script runat="server" Files="JQueryLoad;Config;Analytics;Statistics;CascadingContent;Alert;JQueryRetry;Heatmap" EnableViewState="false" />
