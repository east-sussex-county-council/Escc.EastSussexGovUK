<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendar.aspx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Data.calendar" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl runat="server"
        id="headcontent" 
        Title="Subscribe to calendar: {0}"
        Description="Subscribe to calendar: {0}"
        DateCreated="2012-06-29"
        IpsvPreferredTerms="Internet"
    />
    <ClientDependency:Css runat="server" Files="ContentSmall" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="article">
        <article id="article" runat="server">
            <div class="content text-content">
    <h1 runat="server" id="heading">Subscribe to calendar: {0}</h1>

    <p>Click 'Subscribe to calendar' to add these dates to your own calendar, and get updates automatically. 
    You can find instructions for subscribing in popular calendars below.</p>
    <p class="main-action"><a runat="server" id="subscribe" type="text/calendar">Subscribe to calendar</a></p>
    
    <p>If you don't want to subscribe to updates, <a runat="server" id="download" type="text/calendar">download the calendar</a> instead.</p>
    
    
    <h2>How to subscribe</h2>
    <ul>
    <li><a href="https://support.apple.com/kb/PH11523?locale=en_GB">Apple Calendar: Subscribe to calendars</a></li>
    <li><a href="http://office.microsoft.com/en-us/outlook-help/view-and-subscribe-to-internet-calendars-HA010167325.aspx#BM2">Microsoft Outlook: Add an Internet calendar subscription</a></li>
    <li><a href="http://windows.microsoft.com/en-GB/hotmail/calendar-subscribe-calendar-ui">Outlook.com: Import or subscribe to a calendar</a></li>
    <li><a href="http://support.google.com/calendar/bin/answer.py?hl=en&amp;answer=37100">Google Calendar: Subscribe to calendars</a></li>
    </ul>

                </div>
        </article>
    </div>
    <EastSussexGovUK:MasterPageControl runat="server" Control="Banners" />
</asp:Content>