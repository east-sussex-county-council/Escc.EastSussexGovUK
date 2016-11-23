<%@ Page language="c#" Codebehind="HttpStatus404.aspx.cs" AutoEventWireup="True" Inherits="Escc.EastSussexGovUK.WebForms.NotFound" validateRequest="false" EnableViewState="false" %>
<%@ Register TagPrefix="Metadata" Namespace="Escc.Web.Metadata" Assembly="Escc.Web.Metadata" %>

<asp:Content runat="server" ContentPlaceHolderID="metadata">
	<Metadata:MetadataControl id="headContent" runat="server"
		Title="Page not found"
        DateIssued="2005-01-27"
        DateModified="2010-09-13"
        IpsvPreferredTerms="Internet"
		LgtlType="Website facilities"
		IsInSearch="False" 
		/>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <div class="text" id="css" runat="server">
            <h1>Page not found</h1>
        
            <p>Sorry &#8211; the page you asked for could not be found. It may have been moved or
                deleted.</p>
            <p>To find what you're looking for:</p>
            <ul>
                <li>start with the menu at the top of any page, and try to find the information you
                    want</li>
                <li>search for what you want using the search box at the top of any page</li>
                <li>use our <a href="https://apps.eastsussex.gov.uk/contactus/emailus/feedback.aspx">comments form</a> to contact
                    us &#8211; we may be able to help.</li>
            </ul>
        </div>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="javascript">
    <div runat="server" id="script">
        if (typeof (jQuery) !== "undefined") {
            $(function () {
                // Track in Google Analytics which pages returned a 404
                var requestUrl = $("script[data-request]").data("request");
                var referrerUrl = $("script[data-referrer]").data("referrer");
        
                if (typeof (ga) !== "undefined") {
                    ga('send', 'event', '404', requestUrl, referrerUrl);
                }
            });
        }
    </div>
</asp:Content>