<%@ Control AutoEventWireup="true" CodeBehind="Petitions.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.Petitions" Language="C#" %>
<%@ Register TagPrefix="Feeds" Namespace="EsccWebTeam.Feeds" Assembly="EsccWebTeam.Feeds, Version=1.0.0.0, Culture=neutral, PublicKeyToken=06fad7304560ae6f" %>
<%@ OutputCache Duration="86400" VaryByParam="host" %><%-- host parameter is supplied by MasterPageControl when requesting HTML remotely --%>

<Feeds:FeedControl runat="server" FeedUri="http://epetition.eastsussex.public-i.tv/epetition_core/feed/local_petitions" MaximumItems="5" CssClass="petitions" ID="feed">
	<HeaderTemplate>
	<div>
	    <h2><span>Sign a petition</span></h2>
	    <ul>
	</HeaderTemplate>
	<ItemTemplate>
	    <li><a href="" id="link" runat="server"><asp:Literal ID="title" runat="server" /></a></li>
	</ItemTemplate>
	<FooterTemplate>
	    </ul>
        <p><a runat="server" id="start" href="/yourcouncil/consultation/petitions/default.htm">Start your own petition</a></p>
        </div>
	</FooterTemplate>
</Feeds:FeedControl>