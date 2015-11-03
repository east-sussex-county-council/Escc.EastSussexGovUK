<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TwitterSearch.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.TwitterSearch" %>
<ClientDependency:Css runat="server" Files="SocialMediaSmall" />
<ClientDependency:Css runat="server" Files="SocialMediaLarge" MediaConfiguration="Large" />
<div class="supporting">
<script src="//widgets.twimg.com/j/2/widget.js"></script>
<script>
new TWTR.Widget({
    version: 2, type: 'search', interval: 30000, rpp: 5, subject: '', width: 'auto', height: 'auto',
    search: '<asp:literal runat="server" id="searchTerm1" />',
    title: '<asp:literal runat="server" id="searchTerm2" /> on Twitter',
    theme: {
        shell: { background: '#056b0c', color: '#ffffff' },
        tweets: { background: '#ffffff', color: '#131313', links: '#056b0c' }
    },
    features: { scrollbar: false, loop: false, live: false, behavior: 'all' }
}).render().start();
</script>
</div>