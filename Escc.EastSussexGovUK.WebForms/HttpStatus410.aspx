<%@ Page Language="c#" CodeBehind="HttpStatus410.aspx.cs" AutoEventWireup="true" Inherits="Escc.EastSussexGovUK.WebForms.Status410" EnableViewState="false" %>
<%@ Register TagPrefix="Metadata" Namespace="Escc.Web.Metadata" Assembly="Escc.Web.Metadata" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl ID="headContent" runat="server"
        Title="Page gone"
        IpsvPreferredTerms="Internet"
        LgtlType="Website facilities"
        DateModified="2019-03-22"
        IsInSearch="False" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="bodyclass">no-breadcrumb</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="breadcrumb" />

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <div class="content text-content">
            <h1>Page gone</h1>
        
            <p>Sorry &#8211; the page you asked for has been removed.</p>
            <p>To find what you're looking for:</p>
            <ul>
                <li>start with the menu at the top of any page, and try to find the information you
                    want</li>
                <li>search for what you want using the search box at the top of any page</li>
                <li><a href="https://www.eastsussex.gov.uk/contact-us/">contact us</a> &#8211; we may be able to help.</li>
            </ul>
        </div>
    </div>
</asp:Content>