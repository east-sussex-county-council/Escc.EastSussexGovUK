<%@ Page language="c#" Codebehind="Status404.aspx.cs" AutoEventWireup="True" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.NotFound" validateRequest="false" EnableViewState="false" MasterPageFile="~/masterpages/desktop.master" %>

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
        <div class="text" id="css" runat="server">
            <h1>Page not found</h1>
        
            <p>Sorry &#8211; the page you asked for could not be found. It may have been moved or
                deleted.</p>
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

<asp:content runat="server" ContentPlaceHolderID="supporting" />