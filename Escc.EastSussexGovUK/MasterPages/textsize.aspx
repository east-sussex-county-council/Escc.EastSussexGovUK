<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="textsize.aspx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.TextSize" EnableViewState="false" MasterPageFile="Desktop.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="metadata">
	<Metadata:MetadataControl runat="server" 
		Title="Text size changed"
		IsInSearch="False"
		IpsvPreferredTerms="Visual impairment support"
		LgalTypes="Disabled people"
		DateIssued="2010-04-19"
		 />
    <ClientDependency:Css runat="server" Files="ContentSmall"/>
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium"/>
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large"/>
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="content">
<div class="article">
<div class="content text-content">
		<h1>Text size changed</h1>
		<p>The size of the text on our website has been changed.</p>
</div>
</div>
</asp:Content>