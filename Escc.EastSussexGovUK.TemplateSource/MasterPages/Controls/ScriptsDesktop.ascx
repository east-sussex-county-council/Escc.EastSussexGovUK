﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptsDesktop.ascx.cs" Inherits="Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls.ScriptsDesktop" %>
<%-- Grab Google CDN's jQuery, with a protocol relative URL; fall back to local JQuery if necessary --%>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js" integrity="sha256-wS9gmOZBqsqWxgIVgA8Y9WcQOa7PgSIX+rPA0VL2rbQ=" crossorigin="anonymous"></script>
<ClientDependency:Script runat="server" Files="Modernizr;JQueryLoad;Config;Analytics;Statistics;Media;Documents;jQueryUI;AutoSuggest;CrossOriginIE;CascadingContent;Alert;JQueryRetry;Email;Heatmap;Languages;Banners;Privacy" EnableViewState="false" />
<ClientDependency:Script Files="PublicLibraries" runat="server" EnableViewState="false" id="publicLibraries" />
