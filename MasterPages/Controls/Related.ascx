<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Related.ascx.cs" Inherits="EsccWebTeam.EastSussexGovUK.MasterPages.Controls.Related" %>
<EastSussexGovUK:ContextContainer runat="server" Legacy="false">
    <div class="related" role="navigation">
    <aside>
        <asp:PlaceHolder runat="server" id="pagesSection">
        <div class="section">
        <section>
            <h2>Related pages</h2>
            <asp:PlaceHolder runat="server" ID="pagesSectionContent" />
        </section>
        </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="pagesOnly">
            <h2>Related pages</h2>
            <asp:PlaceHolder runat="server" ID="pagesOnlyContent" />
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" id="sitesSection">
        <div class="section">
        <section>
            <h2>Related websites</h2>
            <asp:PlaceHolder runat="server" ID="sitesSectionContent" />
        </section>
        </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="sitesOnly">
            <h2>Related websites</h2>
            <asp:PlaceHolder runat="server" ID="sitesOnlyContent" />
        </asp:PlaceHolder>
    </aside>
    </div>
</EastSussexGovUK:ContextContainer>