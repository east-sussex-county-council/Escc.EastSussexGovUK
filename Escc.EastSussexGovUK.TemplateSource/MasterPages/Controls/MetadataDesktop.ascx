<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetadataDesktop.ascx.cs" Inherits="Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls.MetadataDesktop" %>
<meta charset="UTF-8" />
<meta name="viewport" content="width=device-width, initial-scale=1.0" />
<link rel="dns-prefetch" href="//eastsussexgovuk.blob.core.windows.net" />
<ClientDependency:Css runat="server" Files="Template1Small;TemplatePrint;JQueryUI;EmailSmall" Moveable="false" EnableViewState="false" />
<ClientDependency:Css runat="server" Files="Template2Medium" MediaConfiguration="Medium" Moveable="false" EnableViewState="false" />
<ClientDependency:Css runat="server" Files="Template3Large;TemplateIE6Large;EmailLarge" MediaConfiguration="Large" Moveable="false" EnableViewState="false" />
<%-- All JavaScript at the bottom, except for Modernizr and -prefix-free which enable HTML5/CSS3 and need to be early to avoid flash of unstyled content --%>
<ClientDependency:Script runat="server" Files="Modernizr" MergeWithSimilar="false" Moveable="false" EnableViewState="false" />
