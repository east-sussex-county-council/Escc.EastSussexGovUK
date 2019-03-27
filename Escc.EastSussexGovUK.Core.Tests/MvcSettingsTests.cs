using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public class MvcSettingsTests
    {
        [Fact]
        public void MvcSettings_has_default_ViewParameterName()
        {
            var settings = new MvcSettings();

            Assert.NotNull(settings.ViewParameterName);
            Assert.NotEqual(string.Empty, settings.ViewParameterName);
        }

        [Fact]
        public void MvcSettings_has_default_DesktopViewPath()
        {
            var settings = new MvcSettings();

            Assert.StartsWith("~/", settings.DesktopViewPath);
            Assert.EndsWith(".cshtml", settings.DesktopViewPath);
        }

        [Fact]
        public void MvcSettings_has_default_FullScreenViewPath()
        {
            var settings = new MvcSettings();

            Assert.StartsWith("~/", settings.FullScreenViewPath);
            Assert.EndsWith(".cshtml", settings.FullScreenViewPath);
        }

        [Fact]
        public void MvcSettings_has_default_PlainViewPath()
        {
            var settings = new MvcSettings();

            Assert.StartsWith("~/", settings.PlainViewPath);
            Assert.EndsWith(".cshtml", settings.PlainViewPath);
        }
    }
}
