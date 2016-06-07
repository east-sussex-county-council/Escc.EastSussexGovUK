<%@ Page language="c#" Codebehind="Status404.aspx.cs" AutoEventWireup="True" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.NotFound" validateRequest="false" EnableViewState="false" MasterPageFile="Desktop.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
	<Metadata:MetadataControl id="headContent" runat="server"
		Title="Page not found"
        DateIssued="2005-01-27"
        DateModified="2010-09-13"
        IpsvPreferredTerms="Internet"
		LgtlType="Website facilities"
		IsInSearch="False" 
		/>
    <ClientDependency:Css runat="server" Files="ContentSmall"/>
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium"/>
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large"/>
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
    <asp:placeholder runat="server" id="content" />
    </div>
</asp:Content>

<asp:content runat="server" ContentPlaceHolderID="supporting" />