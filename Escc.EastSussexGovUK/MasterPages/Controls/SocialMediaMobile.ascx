<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SocialMediaMobile.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.SocialMediaMobile" %>
<EastSussexGovUK:ContextContainer runat="server" CmsEdit="false">
<div class="supporting-text editHelp small medium" id="source" runat="server" visible="false"></div>
<div class="social-mobile text small medium" id="social" runat="server">
    <asp:PlaceHolder runat="server" ID="fbContainer" EnableViewState="false" Visible="false"><p class="facebook">Find us on Facebook: <a id="fb" runat="server" enableviewstate="false"></a></p></asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="twContainer" EnableViewState="false" Visible="false"><p class="twitter">Read updates on Twitter: <a id="tw"  runat="server" href="http://twitter.com/search/{0}">Search for '{0}' on Twitter</a></p></asp:PlaceHolder>
</div>
<ClientDependency:Css runat="server" Files="SocialMediaSmall" />
</EastSussexGovUK:ContextContainer>