<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlertFullScreen.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.AlertFullScreen" %>
<strong class="alert" role="alert" id="alert" runat="server"><span class="icon"></span><article>
<Egms:Css runat="server" Files="Alert" />
<ServiceClosures:EmergencyClosuresLink id="closures" runat="server" ServiceType="School" NavigateUrl="/educationandlearning/schools/schoolclosures.htm" Text="Emergency school closures">
<HeaderTemplate><p></HeaderTemplate>
<FooterTemplate> &#8211; check if your school is affected.</p></FooterTemplate>
</ServiceClosures:EmergencyClosuresLink>
<p id="disruption" runat="server" enableviewstate="false">Warning of <a href="/news/disruption.htm">disruption to council services</a>.</p>
</article></strong>