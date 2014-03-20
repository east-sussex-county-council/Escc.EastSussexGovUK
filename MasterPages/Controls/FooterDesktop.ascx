<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FooterDesktop.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.FooterDesktop" %>
<%@ Register TagPrefix="EastSussexGovUK" TagName="Languages" Src="languages.ascx" %>
<%@ Register TagPrefix="EastSussexGovUK" TagName="Petitions" Src="petitions.ascx" %>
<footer>
<div class="footer screen" role="contentinfo">
    <EastSussexGovUK:Languages runat="server" EnableViewState="false" />
    <div class="container footer-content">
        <div class="online">
            <nav>
            <h2>Connect with us</h2>
            
            <p class="social"><a href="/contactus/socialmedia/default.htm" id="social" runat="server" enableviewstate="false"><span class="twitter"><span class="facebook"><span class="youtube"><span class="flickr">&nbsp;</span></span></span></span><span class="find">Find us on social media</span></a></p>
            <ul><li><a href="/about/" id="about" runat="server" accesskey="8" enableviewstate="false">About this site</a></li>
            <li><a href="/about/privacypolicy/default.htm" id="privacy" runat="server" enableviewstate="false">Privacy and cookies</a></li></ul>
            </nav>
        </div>
                
        <form method="post" class="newsletter form" action="https://www.eastsussex.gov.uk/registered/updates/subscribe.aspx">
            <h2>Get money for your project with our Funding News</h2>
            <p><label for="newsletter">My email address is:</label></p>
            <input type="email" id="newsletter" name="newsletter" class="email" />
            <input type="hidden" name="item" value="6" />
            <input type="submit" value="Sign up" class="button" />
        </form>

        <EastSussexGovUK:Petitions runat="server" EnableViewState="false" />
    </div>
</div>
</footer>