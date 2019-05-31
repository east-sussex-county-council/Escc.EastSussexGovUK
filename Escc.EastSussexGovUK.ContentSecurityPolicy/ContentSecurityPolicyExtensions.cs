using System;
using PeterJuhasz.AspNetCore.Extensions.Security;

namespace Escc.EastSussexGovUK.ContentSecurityPolicy
{
    /// <summary>
    /// Extension methods for building up a content security policy
    /// </summary>
    public static class ContentSecurityPolicyExtensions
    {
        /// <summary>
        /// Adds support for the default policy for www.eastsussex.gov.uk to an existing Content Security Policy - a default source of 'none', resources allowed to load from our own site, plus JQuery from ajax.googleapis.com
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddEastSussexGovUKDefaultPolicy(this CspOptions options)
        {
            var updated = options.Clone();

            updated.DefaultSrc = CspDirective.None;

            if (updated.StyleSrc == null) { updated.StyleSrc = StyleCspDirective.Empty; }
            updated.StyleSrc = updated.StyleSrc.AddSelf().AddSource("https://www.eastsussex.gov.uk");

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSelf()
                .AddSource("https://www.eastsussex.gov.uk")
                .AddSource("https://ajax.googleapis.com");

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSelf()
                .AddDataScheme()
                .AddSource("https://www.eastsussex.gov.uk");

            if (updated.ConnectSrc == null) { updated.ConnectSrc = CspDirective.Empty; }
            updated.ConnectSrc = updated.ConnectSrc.AddSelf()
                .AddSource("https://www.eastsussex.gov.uk")
                .AddSource("https://apps.eastsussex.gov.uk");

            if (updated.ObjectSrc == null) { updated.ObjectSrc = CspDirective.Empty; }
            updated.ObjectSrc = updated.ObjectSrc.AddSelf();

            updated.ReportUri = new Uri("https://eastsussexgovuk.report-uri.io/r/default/csp/enforce");

            return updated;
        }

        /// <summary>
        /// Adds support for Google Fonts to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddGoogleFonts(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.FontSrc == null) { updated.FontSrc = CspDirective.Empty; }
            updated.FontSrc = updated.FontSrc.AddSource("https://fonts.gstatic.com");

            if (updated.StyleSrc == null) { updated.StyleSrc = StyleCspDirective.Empty; }
            updated.StyleSrc = updated.StyleSrc.AddSource("https://fonts.googleapis.com");

            return updated;
        }

        /// <summary>
        /// Adds support for Google Analytics to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddGoogleAnalytics(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://www.google-analytics.com");

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://www.google-analytics.com");

            return updated;
        }

        /// <summary>
        /// Adds support for Google Maps to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddGoogleMaps(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://*.googleapis.com").AddSource("https://maps.gstatic.com").AddUnsafeEval();

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://*.gstatic.com").AddSource("https://*.googleapis.com").AddSource("https://*.ggpht.com");

            if (updated.StyleSrc == null) { updated.StyleSrc = StyleCspDirective.Empty; }
            updated.StyleSrc = updated.StyleSrc.AddUnsafeInline();

            if (updated.FrameSrc == null) { updated.FrameSrc = CspDirective.Empty; }
            updated.FrameSrc = updated.FrameSrc.AddSource("https://maps.google.co.uk").AddSource("https://www.google.com");

            return updated;
        }

        /// <summary>
        /// Adds support for Google Content Experiments to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddGoogleContentExperiments(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddUnsafeInline();

            return updated;
        }

        /// <summary>
        /// Adds support for Google Tag Manager to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddGoogleTagManager(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://tagmanager.google.com/").AddSource("https://www.googletagmanager.com");

            if (updated.StyleSrc == null) { updated.StyleSrc = StyleCspDirective.Empty; }
            updated.StyleSrc = updated.StyleSrc.AddSource("https://www.googletagmanager.com").AddSource("https://tagmanager.google.com");

            return updated;
        }

        /// <summary>
        /// Adds support for Google Recaptcha to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddGoogleRecaptcha(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://www.google.com").AddSource("https://www.gstatic.com");

            if (updated.FrameSrc == null) { updated.FrameSrc = CspDirective.Empty; }
            updated.FrameSrc = updated.FrameSrc.AddSource("https://www.google.com");

            if (updated.StyleSrc == null) { updated.StyleSrc = StyleCspDirective.Empty; }
            updated.StyleSrc = updated.StyleSrc.AddUnsafeInline();

            return updated;
        }

        /// <summary>
        /// Adds support for Crazy Egg to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddCrazyEgg(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://*.crazyegg.com").AddSource("https://s3.amazonaws.com");

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://*.crazyegg.com").AddSource("https://gtrk.s3.amazonaws.com").AddSource("https://s3.amazonaws.com");

            return updated;
        }

        /// <summary>
        /// Adds support for the <a href="https://developers.facebook.com/docs/plugins/page-plugin">Facebook Page Plugin</a> to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <remarks>Facebook only needs style-src 'sha256-W2kUcrmSyYrtLKKok5R0tuGKVjGmCtnA6wr7AIdSwgU=' rather than unsafe-inline, but if a hash or nonce is used then unsafe-inline is ignored for others that need it (ie Twitter)</remarks>
        public static CspOptions AddFacebook(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://connect.facebook.net");

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://www.facebook.com");

            if (updated.FrameSrc == null) { updated.FrameSrc = CspDirective.Empty; }
            updated.FrameSrc = updated.FrameSrc.AddSource("https://*.facebook.com");

            if (updated.StyleSrc == null) { updated.StyleSrc = StyleCspDirective.Empty; }
            updated.StyleSrc = updated.StyleSrc.AddUnsafeInline();

            return updated;
        }

        /// <summary>
        /// Adds support for Twitter feeds to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddTwitter(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://*.twitter.com").AddSource("https://*.twimg.com");

            if (updated.FrameSrc == null) { updated.FrameSrc = CspDirective.Empty; }
            updated.FrameSrc = updated.FrameSrc.AddSource("https://*.twitter.com");

            if (updated.StyleSrc == null) { updated.StyleSrc = StyleCspDirective.Empty; }
            updated.StyleSrc = updated.StyleSrc.AddSource("https://platform.twitter.com").AddSource("https://*.twimg.com").AddUnsafeInline();

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://*.twitter.com").AddSource("https://*.twimg.com").AddDataScheme();

            if (updated.ConnectSrc == null) { updated.ConnectSrc = CspDirective.Empty; }
            updated.ConnectSrc = updated.ConnectSrc.AddSource("https://syndication.twitter.com");

            return updated;
        }

        /// <summary>
        /// Adds support for YouTube videos to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddYouTube(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.FrameSrc == null) { updated.FrameSrc = CspDirective.Empty; }
            updated.FrameSrc = updated.FrameSrc.AddSource("https://www.youtube-nocookie.com");

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://www.youtube.com").AddSource("https://s.ytimg.com");

            return updated;
        }

        /// <summary>
        /// Adds support for an East Sussex 1Space search widget to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddEastSussex1Space(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://1space.eastsussex.gov.uk");

            return updated;
        }

        /// <summary>
        /// Adds support for web chat to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddWebChat(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://v4in1-si.click4assistance.co.uk");

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://v4in1-si.click4assistance.co.uk").AddUnsafeInline().AddUnsafeEval();

            if (updated.FrameSrc == null) { updated.FrameSrc = CspDirective.Empty; }
            updated.FrameSrc = updated.FrameSrc.AddSource("https://v4in1-ti.click4assistance.co.uk");

            return updated;
        }

        /// <summary>
        /// Adds support for an ESCIS search widget to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddEscis(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.FrameSrc == null) { updated.FrameSrc = CspDirective.Empty; }
            updated.FrameSrc = updated.FrameSrc.AddSource("https://www.escis.org.uk");

            return updated;
        }

        /// <summary>
        /// Adds support for server-side Ordnance Survey maps to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddOldOrdnanceSurveyMaps(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://maps2.eastsussex.gov.uk");

            return updated;
        }

        /// <summary>
        /// Adds support for client-side Ordnance Survey maps to an existing Content Security Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CspOptions AddOrdnanceSurveyMaps(this CspOptions options)
        {
            var updated = options.Clone();

            if (updated.ScriptSrc == null) { updated.ScriptSrc = ScriptCspDirective.Empty; }
            updated.ScriptSrc = updated.ScriptSrc.AddSource("https://maps1.eastsussex.gov.uk")
                .AddSource("https://maps2.eastsussex.gov.uk")
                .AddSource("https://cdnjs.cloudflare.com")
                .AddSource("https://serverapi.arcgisonline.com")
                .AddUnsafeEval();

            if (updated.StyleSrc == null) { updated.StyleSrc = StyleCspDirective.Empty; }
            updated.StyleSrc = updated.StyleSrc.AddSource("https://serverapi.arcgisonline.com").AddUnsafeInline();

            if (updated.ImgSrc == null) { updated.ImgSrc = CspDirective.Empty; }
            updated.ImgSrc = updated.ImgSrc.AddSource("https://maps1.eastsussex.gov.uk").AddSource("https://maps2.eastsussex.gov.uk").AddSource("https://serverapi.arcgisonline.com");

            return updated;
        }

        /// <summary>
        /// Adds support for localhost URLs to any directive of an existing Content Security Policy that already supports another source
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <remarks>For use in development</remarks>
        public static CspOptions AddLocalhost(this CspOptions options)
        {
            var updated = options.Clone();
            if (updated.ChildSrc != null)
            {
                updated.ChildSrc = updated.ChildSrc.AddSource("https://localhost:*");
            }
            if (updated.ConnectSrc != null)
            {
                updated.ConnectSrc = updated.ConnectSrc.AddSource("https://localhost:*");
            }
            if (updated.FontSrc != null)
            {
                updated.FontSrc = updated.FontSrc.AddSource("https://localhost:*");
            }
            if (updated.FrameSrc != null)
            {
                updated.FrameSrc = updated.FrameSrc.AddSource("https://localhost:*");
            }
            if (updated.ImgSrc != null)
            {
                updated.ImgSrc = updated.ImgSrc.AddSource("https://localhost:*");
            }
            if (updated.ManifestSrc != null)
            {
                updated.ManifestSrc = updated.ManifestSrc.AddSource("https://localhost:*");
            }
            if (updated.MediaSrc != null)
            {
                updated.MediaSrc = updated.MediaSrc.AddSource("https://localhost:*");
            }
            if (updated.ObjectSrc != null)
            {
                updated.ObjectSrc = updated.ObjectSrc.AddSource("https://localhost:*");
            }
            if (updated.ScriptSrc != null)
            {
                updated.ScriptSrc = updated.ScriptSrc.AddSource("https://localhost:*");
            }
            if (updated.StyleSrc != null)
            {
                updated.StyleSrc = updated.StyleSrc.AddSource("https://localhost:*");
            }
            if (updated.WorkerSrc != null)
            {
                updated.WorkerSrc = updated.WorkerSrc.AddSource("https://localhost:*");
            }
            return updated;
        }
    }

}
