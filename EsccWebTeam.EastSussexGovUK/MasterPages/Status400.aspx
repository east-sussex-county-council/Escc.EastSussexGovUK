<%@ Page Language="c#" CodeBehind="Status400.aspx.cs" AutoEventWireup="true" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Status400" EnableViewState="false" MasterPageFile="~/masterpages/desktop.master" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl ID="headContent" runat="server"
        Title="Bad request"
        IpsvPreferredTerms="Internet"
        LgtlType="Website facilities"
        DateModified="2015-11-05"
        IsInSearch="False" />
    <ClientDependency:Css runat="server" Files="ContentSmall"/>
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium"/>
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large"/>
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page" runat="server">
        <div class="text" id="css" runat="server">
            <h1>Bad request</h1>
        
            <p>Sorry &#8211; this page doesn't work like that.</p>
            <p>To find what you're looking for:</p>
            <ul>
                <li>start with the menu at the top of any page, and try to find the information you
                    want</li>
                <li>search for what you want using the search box at the top of any page</li>
                <li>use our <a href="/contactus/emailus/feedback.aspx">comments form</a> to contact
                    us &#8211; we may be able to help.</li>
            </ul>
        </div>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="supporting" />
