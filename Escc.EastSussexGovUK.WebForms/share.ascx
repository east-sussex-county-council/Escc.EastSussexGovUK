﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="share.ascx.cs" Inherits="Escc.EastSussexGovUK.WebForms.share" %>
<%@ Register TagPrefix="EastSussexGovUK" Namespace="Escc.EastSussexGovUK.WebForms" Assembly="Escc.EastSussexGovUK.WebForms" %>
<EastSussexGovUK:ContextContainer runat="server" Plain="false">
<div class="text" id="text" runat="server">
    <aside>
        <p class="screen share-page">
            <a href="https://www.facebook.com/sharer.php?u=<%= EncodedPageUrl %>" target="_blank">Share<span class="aural"> this page on Facebook</span></a>
            <a href="https://twitter.com/intent/tweet?text=<%= EncodedTitle %>+<%= EncodedPageUrl %>+via+&#64;eastsussexcc" target="_blank">Tweet<span class="aural"> this page</span></a>
            <a href="https://apps.eastsussex.gov.uk/contactus/emailus/friend.aspx?url=<%= EncodedPageUrl %>" class="email">Email this page</a>
            <a href="https://apps.eastsussex.gov.uk/contactus/emailus/feedback.aspx?option=web&amp;url=<%= EncodedPageUrl %>&amp;title<%= EncodedTitle %>" class="email" accesskey="9">Comment on this page</a>
        </p>
    </aside>
</div>
</EastSussexGovUK:ContextContainer>