<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderDesktop.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.HeaderDesktop" %>
<%@ Register TagPrefix="NavigationControls" Namespace="EsccWebTeam.NavigationControls" Assembly="EsccWebTeam.NavigationControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=06fad7304560ae6f" %>
<header>
<div class="header" role="banner">
    <div class="context container" runat="server" id="season">  
        <div class="mask" typeof="schema:Organization">  
            <asp:Literal runat="server" ID="logoSmallDivOpen" Text="&lt;div class=&quot;logo-small&quot;&gt;" EnableViewState="false" Visible="false" /><asp:Literal runat="server" ID="logoSmallLinkOpen" Text="&lt;a data-unpublished=&quot;false&quot; href=&quot;" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallLink" Text="/default.htm" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallLinkClose" Text="&quot; class=&quot;screenUrl logo-small&quot;&gt;" enableviewstate="false" /><asp:Literal runat="server" Text="&lt;img alt=&quot;&quot; src=&quot;" enableviewstate="false" /><asp:literal runat="server" id="logoSmall" Text="/img/header/logo-small.gif" enableviewstate="false" /><asp:Literal runat="server" Text="&quot; width=&quot;43&quot; height=&quot;31&quot; /&gt;" enableviewstate="false" /> <span property="schema:name">East Sussex County Council</span><asp:Literal runat="server" ID="logoSmallLinkEnd" Text="&lt;/a&gt;" EnableViewState="false" /><asp:Literal runat="server" ID="logoSmallDivEnd" Text="&lt;/div&gt;" EnableViewState="false" Visible="false" />
            <asp:Literal runat="server" ID="logoLargeLinkOpen" Text="&lt;a data-unpublished=&quot;false&quot; property=&quot;schema:url&quot; href=&quot;" EnableViewState="false" /><asp:Literal runat="server" ID="logoLargeLink" Text="/default.htm" EnableViewState="false" /><asp:Literal runat="server" ID="logoLargeLinkClose" Text="&quot; class=&quot;screenUrl&quot;&gt;" enableviewstate="false" /><asp:Literal runat="server" Text="&lt;img property=&quot;schema:logo&quot; alt=&quot;&quot; src=&quot;" enableviewstate="false" /><asp:literal runat="server" id="logoLarge" Text="/img/header/logo-large.gif" enableviewstate="false" /><asp:Literal runat="server" Text="&quot; width=&quot;118&quot; height=&quot;85&quot; class=&quot;logo-large large&quot; /&gt;" enableviewstate="false" /><asp:Literal runat="server" ID="logoLargeLinkEnd" Text="&lt;/a&gt;" EnableViewState="false" />
            <span runat="server" id="textSize" class="size" enableviewstate="false"></span>
            <a href="/contactus/default.htm" id="contact" class="contact screen" runat="server" accesskey="7" enableviewstate="false">Contact us</a>

            <nav role="navigation">
                <NavigationControls:AzNavigation runat="server" ID="az" TargetFile="/atoz/default.aspx" SkipChars="QX" StyleChars="IZ" LinkTitle="services" CssClass="screen" EnableViewState="false">
                    <HeaderTemplate><h2>Our services:</h2></HeaderTemplate>
                </NavigationControls:AzNavigation>
            </nav>

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
                    <li class="mobile"><a href="/default.htm" id="mobileHome" runat="server" accesskey="1" enableviewstate="false">Home</a></li>
                    <li class="mobile"><a href="/menu.htm" id="mobileMenu" runat="server" accesskey="3" enableviewstate="false">Menu</a></li>
                    <li class="jobs"><a href="/jobs/default.htm" id="jobs" runat="server" enableviewstate="false">Jobs</a></li>
                    <li class="libraries"><a href="/libraries/default.htm" id="libraries" runat="server" enableviewstate="false">Libraries</a></li>
                    <li class="leisure"><a href="/leisureandtourism/default.htm" id="leisure" runat="server" enableviewstate="false">Leisure</a></li>
                    <li class="education"><a href="/educationandlearning/default.htm" id="education" runat="server" enableviewstate="false">Education</a></li>
                    <li class="transport long"><a href="/roadsandtransport/default.htm" id="transport" runat="server" enableviewstate="false">Roads &amp; transport</a></li>
                    <li class="environment long"><a href="/environment/default.htm" id="environment" runat="server" enableviewstate="false">Environment &amp;&nbsp;planning</a></li>
                    <li class="council long"><a href="/yourcouncil/default.htm" id="council" runat="server" enableviewstate="false">Your<br />Council</a></li><%-- <br /> is only for IE6, ensures it always takes two lines --%>
                    <li class="community"><a href="/community/default.htm" id="community" runat="server" enableviewstate="false">Community</a></li>
                    <li class="families"><a href="/childrenandfamilies/default.htm" id="families" runat="server" enableviewstate="false">Families</a></li>
                    <li class="socialcare long"><a href="/socialcare/default.htm" id="socialcare" runat="server" enableviewstate="false">Adult social care&nbsp;&amp;&nbsp;health</a></li>
                    <li class="business"><a href="/business/default.htm" id="business" runat="server" enableviewstate="false">Business</a></li>
                </ul>
            </nav>
        </div>
    </div>
</div>

</div>
</header>
