using System;
using System.Collections.Generic;
using System.Linq;
using Escc.EastSussexGovUK.Features;
using Escc.Net;
using Escc.Redirects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public class EastSussexGovUKBuilderTests
    {
        [Fact]
        public void AddEastSussexGovUK_requires_IServiceCollection()
        {
            Assert.Throws<ArgumentNullException>(() => EastSussexGovUKBuilder.AddEastSussexGovUK(null, new Mock<IConfiguration>().Object));
        }

        [Fact]
        public void AddEastSussexGovUK_requires_IConfiguration()
        {
            Assert.Throws<ArgumentNullException>(() => EastSussexGovUKBuilder.AddEastSussexGovUK(new Mock<IServiceCollection>().Object, null));
        }

        [Fact]
        public void AddEastSussexGovUK_adds_EmbeddedFileProviders()
        {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder().Build();

            services.AddEastSussexGovUK(config);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<RazorViewEngineOptions>>();

            Assert.Equal(2, options.Value.FileProviders.Where(x => x.GetType() == typeof(EmbeddedFileProvider)).Count());
        }

        private static void TestThatAServiceWithNoDependenciesIsRegistered<T>()
        {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder().Build();

            services.AddEastSussexGovUK(config);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<T>();

            Assert.NotNull(options);
        }

        [Fact]
        public void AddEastSussexGovUK_adds_IHttpContextAccessor()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<IHttpContextAccessor>();
        }


        [Fact]
        public void AddEastSussexGovUK_adds_IViewSelector()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<IViewSelector>();
        }

        [Fact]
        public void AddEastSussexGovUK_adds_IProxyProvider()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<IProxyProvider>();
        }

        [Fact]
        public void AddEastSussexGovUK_adds_IHttpClientProvider()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<IHttpClientProvider>();
        }

        [Fact]
        public void AddEastSussexGovUK_adds_ICacheStrategy_WebChatSettings()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<ICacheStrategy<WebChatSettings>>();
        }

        [Fact]
        public void AddEastSussexGovUK_adds_INotFoundRequestPathResolver()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<INotFoundRequestPathResolver>();
        }

        [Fact]
        public void AddEastSussexGovUK_adds_IRedirectMatcher()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<IRedirectMatcher>();
        }

        [Fact]
        public void AddEastSussexGovUK_adds_IConvertToAbsoluteUrlHandler()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<IConvertToAbsoluteUrlHandler>();
        }

        [Fact]
        public void AddEastSussexGovUK_adds_IPreserveQueryStringHandler()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<IPreserveQueryStringHandler>();
        }

        [Fact]
        public void AddEastSussexGovUK_adds_IClientDependencySetEvaluator()
        {
            TestThatAServiceWithNoDependenciesIsRegistered<IClientDependencySetEvaluator>();
        }
    }
}
