<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupportingContentDesktop.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.SupportingContentDesktop" %>
<%@ Register TagPrefix="EastSussexGovUK" TagName="Adverts" Src="adverts.ascx" %>
<%@ Register TagPrefix="EastSussexGovUK" TagName="ReportApplyPay" Src="reportapplypay.ascx" %>
<EastSussexGovUK:ReportApplyPay runat="server" EnableViewState="false" />
<EastSussexGovUK:Adverts runat="server" EnableViewState="false" />