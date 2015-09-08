<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptsMobile.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.ScriptsMobile" %>
<%-- Grab Google CDN's jQuery, with a protocol relative URL; fall back to local JQuery if necessary --%>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<Egms:Script runat="server" Files="JQueryLoad;Config;Analytics;Statistics;jQueryUI;AutoSuggest;CrossOriginIE;SwitchView;Alert;JQueryRetry;Email;Heatmap;Languages" EnableViewState="false" />
