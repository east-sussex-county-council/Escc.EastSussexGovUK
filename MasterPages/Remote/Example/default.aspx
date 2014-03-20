<%@ Page Title="Example title" %>
<asp:content runat="server" contentplaceholderid="metadata">
    <%-- Include metadata and stylesheets here --%>
</asp:content>

<asp:content runat="server" contentplaceholderid="content">
    <%-- 
    Include your content here. This is between the header and footer, and inside the ASP.NET server form. 
    You can use our layout styles as shown below, or implement your own layout in this area. Our template 
    uses a responsive design which adapts to browser windows from mobile phone sizes up to desktop computers.
    You should aim for your content to work in these environments too.

    .full-page creates a full-width box with a white background to work within
    .text applies standard text styles within .full-page or .article
    
    Alternatively:

    .article creates a main column
    .supporting creates a box which sits alongside .article, within which you can use your own styles
    .supporting-text creates a box which sits alongside .article and has default text styles

    --%>
    <div class="full-page">
        <div class="text">
            <h1>Page heading</h1>
            <p>Example content</p>
        </div>
    </div>

    <div class="article">
        <div class="text">
            <p>Example content</p>
        </div>
    </div>

    <div class="supporting">
        <p>Example content</p>
    </div>

    <div class="supporting-text">
        <h2>Example heading</h2>
        <p>Example content</p>
    </div>
</asp:content>

<asp:content runat="server" contentplaceholderid="supporting">
    <%-- Leave this placeholder here. It prevents default content loading into the page. --%>
</asp:content>

<asp:content runat="server" contentplaceholderid="javascript">
    <%-- Include javascript here. JQuery is already loaded at this point. --%>
</asp:content>