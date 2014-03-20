<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="share.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.share" %>
<EastSussexGovUK:ContextContainer runat="server" Plain="false" CmsEdit="false">
<div class="share screen">
    <aside>
    <div class="aside">
    <a href="https://twitter.com/share" class="twitter-share-button" data-count="horizontal" data-via="EastSussexCC">Tweet</a>
    <EastSussexGovUK:ContextContainer runat="server" DoNotTrack="false">
        <div class="fb-like" data-layout="button_count" data-send="false" data-width="80" data-show-faces="false" data-font="arial"></div>
    </EastSussexGovUK:ContextContainer>
    <EastSussexGovUK:ContextContainer runat="server" DoNotTrack="true">
        <a href="http://www.facebook.com/sharer.php?u={0}" id="facebook" runat="server" class="facebook fb-dnt" target="_blank">Share<span class="aural"> this page on Facebook</span></a>
    </EastSussexGovUK:ContextContainer>
    <div>
    <p class="send button-nav"><a href="https://www.eastsussex.gov.uk/contactus/emailus/friend.aspx?url={0}" runat="server" id="email">Email this page</a></p>
    <p class="comment button-nav"><a href="https://www.eastsussex.gov.uk/contactus/emailus/feedback.aspx?option=web&amp;amp;url={0}&amp;amp;title={1}" runat="server" id="comment" accesskey="9">Comment on this page</a></p>
    </div>
    </div>
    </aside>
</div>
<Egms:Script runat="server" Files="SocialMedia" />
</EastSussexGovUK:ContextContainer>