﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderDesktop.ascx.cs" Inherits="Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls.HeaderDesktop" %>
<header>
<div class="header-v2" role="banner">
    <div class="context container" runat="server" id="season">  
        <asp:Literal runat="server" ID="logoSmallDivOpen" Text="&lt;div class=&quot;logo-small&quot;&gt;" EnableViewState="false" Visible="false" /><asp:Literal runat="server" ID="logoSmallLinkOpen" Text="&lt;a href=&quot;" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallLink" Text="/" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallLinkClose" Text="&quot; class=&quot;screenUrl logo-small&quot;&gt;" enableviewstate="false" /><asp:Literal runat="server" Text="&lt;img alt=&quot;&quot; src=&quot;" enableviewstate="false" /><asp:literal runat="server" id="logoSmall" Text="/img/header/logo-small.gif" enableviewstate="false" /><asp:Literal runat="server" Text="&quot; width=&quot;43&quot; height=&quot;31&quot; /&gt;" enableviewstate="false" /> East Sussex County Council<asp:Literal runat="server" ID="logoSmallLinkEnd" Text="&lt;/a&gt;" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallDivEnd" Text="&lt;/div&gt;" EnableViewState="false" Visible="false" />
        <asp:Literal runat="server" ID="logoLargeLinkOpen" Text="&lt;a href=&quot;" EnableViewState="false" /><asp:Literal runat="server" ID="logoLargeLink" Text="/" EnableViewState="false" /><asp:Literal runat="server" ID="logoLargeLinkClose" Text="&quot; class=&quot;screenUrl&quot;&gt;" enableviewstate="false" /><asp:Literal runat="server" Text="&lt;img alt=&quot;East Sussex County Council&quot; src=&quot;" enableviewstate="false" /><asp:literal runat="server" id="logoLarge" Text="/img/header/logo-large.png" enableviewstate="false" /><asp:Literal runat="server" Text="&quot; width=&quot;93&quot; height=&quot;67&quot; class=&quot;logo-large large&quot; /&gt;" enableviewstate="false" /><asp:Literal runat="server" ID="logoLargeLinkEnd" Text="&lt;/a&gt;" EnableViewState="false" />
        <span runat="server" id="textSize" class="size" enableviewstate="false"></span>
        <a href="/contact-us/" id="contact" class="contact screen" runat="server" accesskey="7" enableviewstate="false">Contact us</a>

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
                <li class="mobile"><a href="/" id="mobileHome" runat="server" accesskey="1" enableviewstate="false">Home</a></li>
                <li class="mobile"><a href="/menu/" id="mobileMenu" runat="server" accesskey="3" enableviewstate="false">Menu</a></li>
                <li class="jobs"><a href="/jobs/" id="jobs" runat="server" enableviewstate="false">Jobs</a></li>
                <li class="libraries"><a href="/libraries/" id="libraries" runat="server" enableviewstate="false">Libraries</a></li>
                <li class="leisure"><a href="/leisureandtourism/" id="leisure" runat="server" enableviewstate="false">Leisure</a></li>
                <li class="education"><a href="/educationandlearning/" id="education" runat="server" enableviewstate="false">Education</a></li>
                <li class="transport long"><a href="/roadsandtransport/" id="transport" runat="server" enableviewstate="false">Roads &amp; transport</a></li>
                <li class="environment long"><a href="/environment/" id="environment" runat="server" enableviewstate="false">Environment &amp;&nbsp;planning</a></li>
                <li class="council long"><a href="/yourcouncil/" id="council" runat="server" enableviewstate="false">Your Council</a></li>
                <li class="community"><a href="/community/" id="community" runat="server" enableviewstate="false">Community</a></li>
                <li class="families long"><a href="/childrenandfamilies/" id="families" runat="server" enableviewstate="false">Children &amp;&nbsp;families</a></li>
                <li class="socialcare long"><a href="/socialcare/" id="socialcare" runat="server" enableviewstate="false">Adult social care&nbsp;&amp;&nbsp;health</a></li>
                <li class="business"><a href="/business/" id="business" runat="server" enableviewstate="false">Business</a></li>
            </ul>
        </nav>
    </div>
</div>

</div>
</header>
