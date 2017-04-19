<%@ Page Language="c#" CodeBehind="HttpStatus403.aspx.cs" AutoEventWireup="true" Inherits="Escc.EastSussexGovUK.WebForms.HttpStatus403" EnableViewState="false" %>
<%@ Register TagPrefix="Metadata" Namespace="Escc.Web.Metadata" Assembly="Escc.Web.Metadata" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl ID="headContent" runat="server"
        Title="Forbidden"
        IpsvPreferredTerms="Internet"
        LgtlType="Website facilities"
        DateModified="2016-05-02"
        IsInSearch="False" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="bodyclass">no-breadcrumb</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="breadcrumb" />

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <div class="text" id="css" runat="server">
            <h1>Forbidden</h1>
            <p>Sorry, you're not allowed to view that page.</p>
        </div>
    </div>
</asp:Content>