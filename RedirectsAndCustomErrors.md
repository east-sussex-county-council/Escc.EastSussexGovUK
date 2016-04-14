# Redirects and custom errors

HTTP redirects (`301 Permanently Moved` and `303 See Other`) are controlled using the [Escc.Redirects](https://github.com/east-sussex-county-council/Escc.Redirects) project. Redirects are wired up using the 404 page configured in IIS. Requests resulting in a 404 have followed a three-step process:

1. If there is content at the requested URL, return that. If not, hand over to the 404 page. 
2. The 404 page checks for any redirects based on the requested URL, and redirects if a match is found.
3. If no match is found, the 404 page is returned.

## Integrating with Umbraco

The main part of www.eastsussex.gov.uk is based on Umbraco, which is configured to apply redirects using an external 404 page as follows:

1. ASP.NET custom errors are turned on for security, but no custom errors are configured. Configuring custom errors here results in 404 requests for ASP.NET file extensions such as `*.aspx`, when not processed by Umbraco, getting redirected. The user is taken to the URL of the 404 page with an `aspxerrorpath` query string, rather than receiving a 404 result from the URL originally requested.

		<system.web>
			<customErrors mode="On" />
		</system.web>

2. IIS custom errors have the existing response set to `PassThrough`, which allows Umbraco to handle 404 errors. We configure our 404 page here, but only to be read by custom code and inherited by child applications. `PassThrough` means it's not applied here by IIS.   

		<system.webServer>
	 		<httpErrors existingResponse="PassThrough" errorMode="Custom">
	      		<remove statusCode="404" subStatusCode="-1"/>
      			<error statusCode="404" subStatusCode="-1" path="/masterpages/status404.aspx" responseMode="ExecuteURL"/>
    		</httpErrors>
		</system.webServer>

3. To handle 404 errors in Umbraco we register an [IContentFinder](https://our.umbraco.org/documentation/reference/routing/request-pipeline/icontentfinder) as the "last chance content finder", and this content finder passes any Umbraco 404s over to the 404 page configured in `web.config`. `NotFoundContentFinder` is in the [Escc.EastSussexGovUK.UmbracoViews](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.UmbracoViews) project.

4. Umbraco doesn't process all file extensions, so the content finder misses requests for URLs like `*.js` or `*.html`. For those we have an HTTP module that catches any responses with the status set to 404 and passes them over to the 404 page configured in `web.config`. `UmbracoCustomErrorsModule` also lives in the [Escc.EastSussexGovUK.UmbracoViews](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.UmbracoViews) project.
	
		<system.webServer>
			<modules>
		 		<add name="UmbracoCustomErrorsModule" type="Escc.EastSussexGovUK.UmbracoViews.Errors.UmbracoCustomErrorsModule, Escc.EastSussexGovUK.UmbracoViews"/>
    		</modules>
		</system.webServer>

5. Applications with a separate IIS application scope below Umbraco go back to using the standard IIS configuration. This is inherited from the Umbraco configuration, so all we need to do is reset the `httpErrors` response mode and remove the HTTP module.

		<system.webServer>
			<httpErrors existingResponse="Replace"/>
			<modules>
		 		<remove name="UmbracoCustomErrorsModule" />
    		</modules>
		</system.webServer>

6. Our 404 page isn't part of Umbraco, but it is part of the same IIS application. To make this work we list the path to the 404 page in [umbracoReservedPaths](http://nestorrg-blogs.itequia.com/2009/04/adding-normal-aspx-pages-in-umbraco.html) in `web.config`.

## Custom errors for other HTTP statuses

For HTTP 40x and 50x statuses other than 404, the configuration follows a consistent pattern:

1. ASP.NET custom errors are configured for Umbraco, and can be triggered from your controller or view by throwing an `HttpException`. `ResponseRewrite` mode ensures that the URL of the original request is preserved. 

		<system.web>
		    <customErrors mode="On" redirectMode="ResponseRewrite">
		      <error statusCode="400" redirect="/masterpages/status400.aspx" />
		    </customErrors>
		</system.web>

2. We configure IIS custom errors in the Umbraco application, but only to be inherited by child applications. `PassThrough` means it's not applied here by IIS.   

		<system.webServer>
	 		<httpErrors existingResponse="PassThrough" errorMode="Custom">
	      		<remove statusCode="400" subStatusCode="-1"/>
      			<error statusCode="400" subStatusCode="-1" path="/masterpages/status400.aspx" responseMode="ExecuteURL"/>
    		</httpErrors>
		</system.webServer>

3. Applications with a separate IIS application scope below Umbraco go back to using the standard IIS configuration. This is inherited from the Umbraco configuration, so all we need to do is reset the `httpErrors` response mode.

		<system.webServer>
			<httpErrors existingResponse="Replace"/>
		</system.webServer>

## Using an HTTP module for redirects

Away from Umbraco we also make use of the `RedirectsModule` documented in [Escc.Redirects](https://github.com/east-sussex-county-council/Escc.Redirects) to apply the same redirects on other sub-domains. This is configured at the root of the sub-domain by applying `RedirectsModule.transform.config`.

The `RedirectsModule` breaks the Umbraco back-office login, and also means there is a database query for every request. For these reasons the approach using the 404 page is recommended.