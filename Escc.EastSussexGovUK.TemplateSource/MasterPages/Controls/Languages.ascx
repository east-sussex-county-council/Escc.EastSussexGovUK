﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Languages.ascx.cs" Inherits="Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls.Languages" %>
<%-- Regex explained at http://stackoverflow.com/questions/406230/regular-expression-to-match-string-not-containing-a-word --%>
<div class="languages" id="languages" runat="server">
    <div class="container">
        <ul>
            <li lang="zh-Hans" xml:lang="zh-Hans" class="nonLatin">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=zh-Hans" hreflang="zh-Hans" rel="nofollow alternate" id="chinese" runat="server">中文</a>
            </li>
            <li lang="ar" xml:lang="ar" dir="rtl" class="nonLatin">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=ar" hreflang="ar" rel="nofollow alternate" id="arabic" runat="server">العربية</a>
            </li>
            <li lang="ur" xml:lang="ur" dir="rtl" class="nonLatin">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=ur" hreflang="ur" rel="nofollow alternate" id="urdu" runat="server">اردو</a>
            </li>
            <li lang="ku" xml:lang="ku" dir="rtl" class="nonLatin">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=ku" hreflang="ku" rel="nofollow alternate" id="kurdish" runat="server">کوردی سۆرانی</a>
            </li>
            <li lang="pt" xml:lang="pt">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=pt" hreflang="pt" rel="nofollow alternate" id="portugese" runat="server">Português</a>
            </li>
            <li lang="pl" xml:lang="pl">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=pl" hreflang="pl" rel="nofollow alternate" id="polish" runat="server">Polski</a>
            </li>
            <li lang="sk" xml:lang="sk">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=sk" hreflang="sk" rel="nofollow alternate" id="slovakian" runat="server">Slovenčina</a>
            </li>
            <li lang="tr" xml:lang="tr">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=tr" hreflang="tr" rel="nofollow alternate" id="turkish" runat="server">Türkçe</a>
            </li>
            <%-- <li lang="ps-AF" xml:lang="ps-AF" dir="rtl" class="nonLatin">
                <a href="https://apps.eastsussex.gov.uk/contactus/emailus/translation.aspx?c=ps-AF" hreflang="ps-AF" rel="nofollow alternate">پښتو</a>
            </li>--%>
            <li lang="bfi" xml:lang="bfi">
                <a href="https://www.eastsussex.gov.uk/contact-us/bslintro/?" hreflang="bfi" rel="nofollow alternate" id="bsl" runat="server">British Sign Language (BSL)</a>
            </li>
        </ul>
    </div>
</div>