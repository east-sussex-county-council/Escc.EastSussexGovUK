<%@ Page Language="c#" CodeBehind="Status400.aspx.cs" AutoEventWireup="true" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Status400" EnableViewState="false" MasterPageFile="Desktop.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl ID="headContent" runat="server"
        Title="Bad request"
        IpsvPreferredTerms="Internet"
        LgtlType="Website facilities"
        DateModified="2015-11-05"
        IsInSearch="False" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page" id="errorContainer" runat="server">
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="supporting" />
