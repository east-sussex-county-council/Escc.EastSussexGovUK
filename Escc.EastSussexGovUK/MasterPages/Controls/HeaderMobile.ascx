<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderMobile.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.HeaderMobile" %>
<header>
<div class="header" role="banner">
    <div class="context container">
        <div class="topbar">
            <asp:Literal runat="server" ID="logoSmallDivOpen" Text="&lt;div class=&quot;logo-small&quot;&gt;" EnableViewState="false" Visible="false" /><asp:Literal runat="server" ID="logoSmallLinkOpen" Text="&lt;a href=&quot;" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallLink" Text="/default.htm" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallLinkClose" Text="&quot; class=&quot;screenUrl logo-small&quot;&gt;" enableviewstate="false" /><asp:Literal runat="server" Text="&lt;img alt=&quot;&quot; src=&quot;" enableviewstate="false" /><asp:literal runat="server" id="logoSmall" Text="/img/header/logo-small.gif" enableviewstate="false" /><asp:Literal runat="server" Text="&quot; width=&quot;43&quot; height=&quot;31&quot; /&gt;" enableviewstate="false" /> East Sussex County Council<asp:Literal runat="server" ID="logoSmallLinkEnd" Text="&lt;/a&gt;" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallDivEnd" Text="&lt;/div&gt;" EnableViewState="false" Visible="false" />
            <a href="/contactus/default.htm" class="contact screen" id="contact" runat="server" accesskey="7" enableviewstate="false">Contact us</a>
        </div>
  
        <asp:literal runat="server" text="&lt;form action=&quot;" enableviewstate="false" /><asp:literal runat="server" text="/search/search.aspx" id="searchUrl" enableviewstate="false" /><asp:literal runat="server" text="&quot; method=&quot;get&quot; id=&quot;search&quot; role=&quot;search&quot; class=&quot;screen&quot;&gt;" enableviewstate="false" />
            <div class="search-inner">
                <div class="term-outer">
                    <div class="term-inner">
                        <label for="q" class="aural">Search</label>
                        <input type="search" class="search" placeholder="Enter your search term" id="q" name="q" accesskey="4" value="<%= System.Web.HttpUtility.HtmlEncode(Request.QueryString["q"]) %>" />
                    </div>
                </div>
                <div class="submit"><input type="submit" value="Go" /></div>
            </div>
        <asp:literal runat="server" text="&lt;/form&gt;" enableviewstate="false" />
                        
        <div id="menu" role="navigation" class="screen">
        <nav>
            <ul>
                <li><a href="/default.htm" id="mobileHome" runat="server" accesskey="1" enableviewstate="false">Home</a></li>
                <li><a href="/menu.htm" id="mobileMenu" runat="server" accesskey="3" enableviewstate="false">Menu</a></li>
            </ul>
        </nav>
        </div>
    </div>
           
</div>
</header>
