<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FooterMobile.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.FooterMobile" %>
<%@ Register TagPrefix="EastSussexGovUK" TagName="Languages" Src="languages.ascx" %>
<footer>
<div class="footer screen" role="contentinfo">
    <EastSussexGovUK:Languages runat="server" enableviewstate="false" />
    <div class="container footer-content">
        <div class="online">
            <nav>
            <h2>Find us online</h2>
            <p class="social"><a href="/contactus/socialmedia/default.htm" id="social" runat="server" enableviewstate="false"><span class="twitter"><span class="facebook"><span class="youtube"><span class="flickr"><span class="find">Find us on social media</span></span></span></span></span></a></p>
            <ul>
            <li><a href="/about/" id="about" runat="server" accesskey="8" enableviewstate="false">About this site</a></li>
            <li><a href="/about/privacypolicy/default.htm" id="privacy" runat="server" enableviewstate="false">Privacy and cookies</a></li>
            </ul>
            </nav>
        </div>
    </div>
</div>
</footer>
<noscript><iframe src="//www.googletagmanager.com/ns.html?id=<%= GoogleTagManagerContainerId %>" height="0" width="0" class="hidden"></iframe></noscript>