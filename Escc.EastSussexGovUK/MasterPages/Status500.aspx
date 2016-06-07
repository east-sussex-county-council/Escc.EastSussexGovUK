<%@ Page Language="c#" CodeBehind="Status500.aspx.cs" AutoEventWireup="true" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.HttpStatus" EnableViewState="false" MasterPageFile="Desktop.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl ID="headContent" runat="server"
        Title="Page unavailable"
        IpsvPreferredTerms="Internet"
        LgtlType="Website facilities"
        DateModified="2015-05-11"
        IsInSearch="False" />
    <ClientDependency:Css runat="server" Files="ContentSmall"/>
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium"/>
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large"/>
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <div class="text">
            <h1>Page unavailable</h1>
            <p>Sorry this page is temporarily unavailable.</p>
            <p>Please try again later.</p>
        </div>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="supporting" />
