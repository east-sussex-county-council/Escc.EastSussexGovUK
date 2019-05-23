# Google Maps support

The `Escc.EastSussexGovUK.ClientDependency` NuGet package adds configuration to `web.config` so that Google Maps code hosted in the sitewide `Escc.EastSussexGovUK.TemplateSource` project can be loaded by [Escc.ClientDependencyFramework](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework).

The `Escc.EastSussexGovUK.ContentSecurityPolicy` NuGet package includes an update to the Content Security Policy for ASP.NET Core to allow embedded Google Maps. The same update to the Content Security Policy is added to `web.config` for MVC5 and WebForms applications by the `Escc.EastSussexGovUK.SecurityConfig` NuGet package and can be referenced using the alias `GoogleMaps`.

## Using the Google Maps API

`google-maps.js` can be loaded by `Escc.ClientDependencyFramework` using the alias `GoogleMaps`. It includes helpers for common tasks using the Google Maps API, including:

*  loading the API
*  creating a map centred on East Sussex 
*  placing a marker on a map
*  setting up search box to zoom to a location

`embed-googlemaps.js` is an example of some of these methods in use.

## Recognising and embedding Google Maps links

`embed-googlemaps.js` can be loaded by `Escc.ClientDependencyFramework` using the alias `EmbedGoogleMaps`. It recognises links to Google Maps in the following format, and updates them to embedded maps:

*  Maps created using [Google My Maps](https://www.google.co.uk/maps/d/u/0/) with a link in the following format:

		<a href="https://maps.google.co.uk/maps/ms?msid=15THr-WmOgF_xi1ATZxLRPlt2rPg&msa=0" class="embed">Map of The Keep</a>

	Note that this is not the current link format given by My Maps, but if you choose a share option you can find the id of the map and replace `15THr-WmOgF_xi1ATZxLRPlt2rPg` in the link shown above to get a link that will embed successfully.

*  Links to the [Location data API provided by the Escc.EastSussexGovUK.Umbraco.Web project](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco/blob/master/Location.md). These links load a set of locations of a given type which can be mapped and is used, for example, to create a [Map of libraries](https://www.eastsussex.gov.uk/libraries/find/). 

		<a href="https://www.eastsussex.gov.uk/umbraco/api/location/list/?type=Library" class="embed">All libraries</a>

The `Escc.EastSussexGovUK` NuGet package contains `EmbeddedGoogleMaps`, which is an `IClientDependencySet` that controls the loading of these dependencies when one of the formats listed above is recognised. 

Loading resources using an `IClientDependencySet` is supported by:

*   `ClientDependencySetEvaluator` in `Escc.EastSussexGovUK.Core`
*   `_FeatureDependencies.cshtml` in `Escc.EastSussexGovUK.Mvc` 