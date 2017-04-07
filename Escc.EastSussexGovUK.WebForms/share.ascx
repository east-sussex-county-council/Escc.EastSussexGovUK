<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="share.ascx.cs" Inherits="Escc.EastSussexGovUK.WebForms.share" %>
<%@ Register TagPrefix="EastSussexGovUK" Namespace="Escc.EastSussexGovUK.WebForms" Assembly="Escc.EastSussexGovUK.WebForms" %>
<EastSussexGovUK:ContextContainer runat="server" Plain="false">
<div class="share screen " id="text" runat="server">
    <h2>Share this page</h2>
    <ul>
        <li><a href="https://apps.eastsussex.gov.uk/contactus/emailus/friend.aspx?url=<%= EncodedPageUrl %>" title="Share this page by email">Share this page by email</a></li>
        <li><a href="https://www.facebook.com/sharer.php?u=<%= EncodedPageUrl %>" target="_blank" title="Share this page on Facebook">Share this page on Facebook</a></li>
        <li><a href="https://twitter.com/intent/tweet?text=<%= EncodedTitle %>+<%= EncodedPageUrl %>+via+&#64;eastsussexcc" target="_blank" title="Tweet about this page">Tweet about this page</a></li>
        <li><a href="https://www.linkedin.com/shareArticle?mini=true&amp;url=<%= EncodedPageUrl %>&amp;title=<%= EncodedTitle %>&amp;summary=<%= EncodedDescription %>&amp;source=East+Sussex+County+Council" target="_blank" title="Share this page on LinkedIn">Share this page on LinkedIn</a></li>
    </ul>
</div>
</EastSussexGovUK:ContextContainer>