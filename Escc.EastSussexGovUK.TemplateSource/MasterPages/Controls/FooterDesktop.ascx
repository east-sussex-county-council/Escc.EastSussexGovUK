<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FooterDesktop.ascx.cs" Inherits="Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls.FooterDesktop" %>
<%@ Register TagPrefix="EastSussexGovUK" TagName="Languages" Src="languages.ascx" %>
<footer>
<div class="footer screen" role="contentinfo">
    <EastSussexGovUK:Languages runat="server" enableviewstate="false" />
    <div class="container footer-content">
        <div class="online">
            <nav>
            <h2>Find us online</h2>
            <p class="social"><a href="/contact-us/socialmedia/our-social-media-sites/" id="social" runat="server" enableviewstate="false"><span class="twitter"><span class="facebook"><span class="instagram"><span class="youtube"><span class="find">Find us on social media</span></span></span></span></span></a></p>
            <ul>
            <li><a href="/about-this-site/" id="about" runat="server" accesskey="8" enableviewstate="false">About this site</a></li>
            <li><a href="/about-this-site/privacy-and-cookies-on-this-site/" id="privacy" runat="server" enableviewstate="false">Privacy and cookies</a></li>
            </ul>
            </nav>
        </div>
    </div>
</div>
</footer>
<noscript><iframe src="//www.googletagmanager.com/ns.html?id=<%= GoogleTagManagerContainerId %>" height="0" width="0" class="hidden"></iframe></noscript>
<script type="application/ld+json">{"@context":"http://schema.org","@type":"GovernmentOrganization","name":"East Sussex County Council","url":"https://www.eastsussex.gov.uk","logo":"https://www.eastsussex.gov.uk/img/header/logo-large.png"}</script>