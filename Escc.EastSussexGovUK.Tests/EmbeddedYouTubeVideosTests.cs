using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;
using Xunit;

namespace Escc.EastSussexGovUK.Tests
{
    public class EmbeddedYouTubeVideosTests
    {
        [Fact]
        public void YouTube_video_URL_is_recognised()
        {
            const string html = "<a href=\"https://www.youtube.com/watch?v=N_dUmDBfp6k\">YouTube video</a>";

            var feature = new EmbeddedYouTubeVideos() {Html = new[] {html}};

            Assert.True(feature.IsRequired());
        }

        [Fact]
        public void YouTube_share_URL_is_recognised()
        {
            const string html = "<a href=\"http://youtu.be/N_dUmDBfp6k\">YouTube video</a>";

            var feature = new EmbeddedYouTubeVideos() { Html = new[] { html } };

            Assert.True(feature.IsRequired());
        }
    }
}
