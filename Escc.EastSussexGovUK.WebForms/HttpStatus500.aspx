<%@ Page Language="c#" CodeBehind="HttpStatus500.aspx.cs" AutoEventWireup="true" Inherits="Escc.EastSussexGovUK.WebForms.HttpStatus500" EnableViewState="false" %>
<%@ Register TagPrefix="Metadata" Namespace="Escc.Web.Metadata" Assembly="Escc.Web.Metadata" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl ID="headContent" runat="server"
        Title="Page unavailable"
        IpsvPreferredTerms="Internet"
        LgtlType="Website facilities"
        DateModified="2015-05-11"
        IsInSearch="False" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <div class="text" id="css" runat="server">
            <h1>Page unavailable</h1>
            <p>Sorry this page is temporarily unavailable.</p>
            <p>Please try again later.</p>
        </div>
    </div>
</asp:Content>