<%@ Page Language="c#" CodeBehind="Status403.aspx.cs" AutoEventWireup="true" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.HttpStatus403" EnableViewState="false" MasterPageFile="Desktop.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl ID="headContent" runat="server"
        Title="Forbidden"
        IpsvPreferredTerms="Internet"
        LgtlType="Website facilities"
        DateModified="2016-05-02"
        IsInSearch="False" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <div class="text">
            <h1>Forbidden</h1>
            <p>Sorry, you're not allowed to view that page.</p>
        </div>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="supporting" />