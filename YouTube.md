# YouTube support

The `Escc.EastSussexGovUK.ClientDependency` NuGet package adds configuration to `web.config` so that YouTube code hosted in the sitewide `Escc.EastSussexGovUK.TemplateSource` project can be loaded by [Escc.ClientDependencyFramework](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework).

The `Escc.EastSussexGovUK.ContentSecurityPolicy` NuGet package includes an update to the Content Security Policy for ASP.NET Core to allow embedded YouTube videos. The same update to the Content Security Policy is added to `web.config` for MVC5 and WebForms applications by the `Escc.EastSussexGovUK.SecurityConfig` NuGet package and can be referenced using the alias `YouTube`.

## Recognising and embedding YouTube videos

`embed-youtube.js` can be loaded by `Escc.ClientDependencyFramework` using the alias `EmbedYouTube`. It recognises links to YouTube videos in the following format, and updates them to embedded videos:

*  Copy the URL of a video straight from `youtube.com` in your browser:

		<a href="https://www.youtube.com/watch?v=eLwmYS9QQDs" class="embed">Coupe De Ville</a>


*  Copy the URL provided by the Share feature on a video: 

		<a href="https://youtu.be/1CM6wnJ0hsg" class="embed">Lovin' life</a>

It also supports links to HTTP rather than HTTPS and a suffix of `&feature=youtu.be` on the end of the URL to support older ways that YouTube used to format its links, which may still be used on our pages.

### Configuring how videos are embedded

Videos are embedded using YouTube's [privacy enhanced mode](https://support.google.com/youtube/answer/171780?hl=en), which simply means they're loaded from `www.youtube-nocookie.com` instead of `www.youtube.com`.

They are set to resize responsively within their container, maintaining their aspect ratio. This behaviour can be configured for a whole page at a time by adding the following attributes to any HTML element on the page:

*  `data-video-width="int"` - sets a fixed width for videos on the page
*  `data-video-height="int"` - sets a fixed height for videos on the page
*  `data-video-max-width="int"` - updates the maximum width for videos (default 600px)
*  `data-video-resize="bool"` - if set to false, disables responsive resizing of video

For example, to make all videos square and fixed-size:

	<html data-video-width="400" data-video-height="400" data-video-resize="false">

### Loading the embed script only when required

The `Escc.EastSussexGovUK` NuGet package contains `EmbeddedYouTubeVideos`, which is an `IClientDependencySet` that controls the loading of these dependencies when one of the formats listed above is recognised. 

Loading resources using an `IClientDependencySet` is supported by:

*   `ClientDependencySetEvaluator` in `Escc.EastSussexGovUK.Core` (see [ASP.NET Core MVC](DotNetCoreMvc.md))
*   `_FeatureDependencies.cshtml` in `Escc.EastSussexGovUK.Mvc` (see [ASP.NET MVC5](DotNetFrameworkMvc.md))

### Tracking embedded YouTube videos in Google Analytics

Analytics on embedded YouTube videos is provided by a customised version of [YouTube Google Analytics by Lunametrics](https://github.com/Bounteous-Inc/youtube-google-analytics). 

This can be loaded by `Escc.ClientDependencyFramework` using the alias `YouTubeAnalytics`, and is loaded automatically along with the embedding code when you use `EmbeddedYouTubeVideos` to recognise and embed videos as described above.