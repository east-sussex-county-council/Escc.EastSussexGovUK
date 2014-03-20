<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Alert.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.Alert" %>
<strong class="alert" role="alert" id="alert" runat="server"><span class="icon"></span><article>
<Egms:Css runat="server" Files="Alert" />
<ServiceClosures:EmergencyClosuresLink id="closures" runat="server" ServiceType="School" NavigateUrl="http://www.eastsussex.gov.uk/educationandlearning/schools/schoolclosures.htm" Text="Emergency school closures">
    <HeaderTemplate>
        <p>
    </HeaderTemplate>
    <FooterTemplate> &#8211; check if your school is affected, and subscribe to alerts.</p></FooterTemplate>
</ServiceClosures:EmergencyClosuresLink>
<asp:Literal runat="server" ID="alertText" />
</article></strong>
<div class="text editHelp" id="source" runat="server" visible="false"></div>