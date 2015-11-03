<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacebookLike.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.FacebookLike" %>
<EastSussexGovUK:ContextContainer runat="server" DoNotTrack="false">
<div class="supporting">
    <ClientDependency:Css runat="server" Files="SocialMediaLarge" MediaConfiguration="Large" />
    <ClientDependency:Script runat="server" Files="SocialMedia" />
    <div class="fb-like-box" data-width="252" data-header="true" runat="server" id="facebook"></div>
</div>
</EastSussexGovUK:ContextContainer>
<EastSussexGovUK:ContextContainer runat="server" DoNotTrack="true">
<div class="supporting-text">
    <p class="facebook">Find us on Facebook: <a id="fb" runat="server" enableviewstate="false"></a></p>
</div>
</EastSussexGovUK:ContextContainer>
<ClientDependency:Css runat="server" Files="SocialMediaSmall" />
