using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Escc.Net;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using Xunit;

namespace Escc.EastSussexGovUK.Tests
{
    public class RemoteMasterPageHtmlProviderTests
    {
        [Fact]
        public async Task Invalid_User_Agent_is_accepted()
        {
            // .NET user agent parser considers this invalid but it's a real-world value. Many other real-world ones fail too.
            var userAgent = "+https://code.google.com/p/feedparser/";
            var html = "<!DOCYTPE html><html>";
            var httpClientAccessor = new Mock<IHttpClientProvider>();
            var messageHandler = new Mock<HttpMessageHandler>();
            messageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(html)
                });
            var response = new Mock<HttpResponseMessage>();
            httpClientAccessor.Setup(x => x.GetHttpClient()).Returns(new HttpClient(messageHandler.Object));
            var breadcrumb = new Mock<IBreadcrumbProvider>();

            var htmlProvider = new RemoteMasterPageHtmlProvider(new Uri("https://www.example.org"), httpClientAccessor.Object, userAgent, null);
            var htmlReturned = await htmlProvider.FetchHtmlForControl("/", new Uri("https://www.example.org"), "HtmlTag", breadcrumb.Object, 1, false);

            Assert.Equal(html, htmlReturned);
        }
    }
}
