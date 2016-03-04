<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptsDesktop.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.ScriptsDesktop" %>
<%-- Grab Google CDN's jQuery, with a protocol relative URL; fall back to local JQuery if necessary --%>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<ClientDependency:Script runat="server" Files="JQueryLoad;Config;Analytics;Statistics;Media;Documents;jQueryUI;AutoSuggest;CrossOriginIE;SwitchView;CascadingContent;Alert;JQueryRetry;Email;Heatmap;Languages;Banners;TagManager" EnableViewState="false" />
<EastSussexGovUK:ContextContainer runat="server" librarycatalogue="true" enableviewstate="false">
<ClientDependency:Script Files="PublicLibraries" runat="server" EnableViewState="false" />
</EastSussexGovUK:ContextContainer>
