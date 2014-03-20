<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportApplyPay.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.ReportApplyPay" %>
<div class="rap large" id="rap" runat="server">
<div class="supporting-text" id="report" visible="false" runat="server">
    <h2><a href="/contactus/reportaproblem/default.htm">Report</a></h2>
    <asp:Literal runat="server" ID="reportLinks" />
</div>
<div class="supporting-text" id="apply" visible="false" runat="server">
    <h2><a href="/contactus/apply/default.htm">Apply</a></h2>
    <asp:Literal runat="server" ID="applyLinks" />
</div>
<div class="supporting-text" id="pay" visible="false" runat="server">
    <h2><a href="/contactus/pay/default.htm">Pay</a></h2>
    <asp:Literal runat="server" ID="payLinks" />
</div>
</div>
<div class="supporting-text editHelp large" id="source" runat="server" visible="false"></div>