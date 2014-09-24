<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptsDesktop.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.ScriptsDesktop" %>
<%-- Grab Google CDN's jQuery, with a protocol relative URL; fall back to local JQuery if necessary --%>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<Egms:Script runat="server" Files="JQueryLoad;Config;Analytics;Statistics;Media;Documents;jQueryUI;AutoSuggest;CrossOriginIE;SwitchView;Alerts" EnableViewState="false" />
<eastsussexgovuk:contextcontainer runat="server" librarycatalogue="true" enableviewstate="false">
<Egms:Script Files="PublicLibraries" runat="server" EnableViewState="false" />
</eastsussexgovuk:contextcontainer>
