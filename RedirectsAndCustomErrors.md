# Redirects and custom errors

HTTP redirects (`301 Permanently Moved` and `303 See Other`) are controlled using the [Escc.Redirects](https://github.com/east-sussex-county-council/Escc.Redirects) project. Redirects are wired up using the 404 page configured in IIS. Requests resulting in a 404 have followed a three-step process:

1. If there is content at the requested URL, return that. If not, hand over to the 404 page. 
2. The 404 page checks for any redirects based on the requested URL, and redirects if a match is found.
3. If no match is found, the 404 page is returned.

## Integrating with Umbraco

The main part of www.eastsussex.gov.uk is based on Umbraco, which is configured to apply redirects using an external 404 page as follows:

1. ASP.NET custom errors are turned on for security, but no custom errors are configured. Configuring custom errors here results in 404 requests for ASP.NET file extensions such as `*.aspx`, when not processed by Umbraco, getting redirected. The user is taken to the URL of the 404 page with an `aspxerrorpath` query string, rather than receiving a 404 result from the URL originally requested. (The configuration is different for other HTTP status codes though, as explained below.)

		<system.web>
			<customErrors mode="On" />
		</system.web>

2. IIS custom errors have the existing response set to `PassThrough`, which allows Umbraco to handle 404 errors. We configure our 404 page here, but only to be read by custom code and inherited by child applications in the same application pool. `PassThrough` means it's not applied here by IIS.   

		<system.webServer>
	 		<httpErrors existingResponse="PassThrough" errorMode="Custom">
	      		<remove statusCode="404" subStatusCode="-1"/>
      			<error statusCode="404" subStatusCode="-1" path="/masterpages/status404.aspx" responseMode="ExecuteURL"/>
    		</httpErrors>
		</system.webServer>

3. To handle 404 errors in Umbraco we register an [IContentFinder](https://our.umbraco.org/documentation/reference/routing/request-pipeline/icontentfinder) as the "last chance content finder", and this content finder passes any Umbraco 404s over to the 404 page configured in `web.config`. `NotFoundContentFinder` is in the [Escc.EastSussexGovUK.Umbraco](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco) project.

4. Umbraco doesn't process all file extensions, so the content finder misses requests for URLs like `*.js` or `*.html`. For those we have an HTTP module that catches any responses with the status set to 404 and passes them over to the 404 page configured in `web.config`. `UmbracoCustomErrorsModule` also lives in the [Escc.EastSussexGovUK.Umbraco](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco) project.
	
		<system.webServer>
			<modules>
		 		<add name="UmbracoCustomErrorsModule" type="Escc.EastSussexGovUK.Umbraco.Errors.UmbracoCustomErrorsModule, Escc.EastSussexGovUK.Umbraco"/>
    		</modules>
		</system.webServer>

5. Applications with a separate IIS application scope below Umbraco go back to using the standard IIS configuration. This is inherited from the Umbraco configuration, so if the child application uses the same application pool as Umbraco all we need to do is reset the `httpErrors` response mode and remove the HTTP module. 

		<system.webServer>
			<httpErrors existingResponse="Replace"/>
			<modules>
		 		<remove name="UmbracoCustomErrorsModule" />
    		</modules>
		</system.webServer>

	If the child application uses a different application pool, the configuration needs to be repeated with new error page paths that are in the same application pool as the child application. If the error pages are in a different application pool to the application itself, IIS will return a blank `403 Forbidden` response instead of the custom error page.

6. Our 404 page isn't part of Umbraco, but it is part of the same IIS application. To make this work we list the path to the 404 page in [umbracoReservedPaths](http://nestorrg-blogs.itequia.com/2009/04/adding-normal-aspx-pages-in-umbraco.html) in `web.config`.

### Custom errors for other HTTP statuses

For HTTP 40x and 50x statuses other than 404, the configuration follows a consistent pattern:

1. ASP.NET custom errors are configured for Umbraco, and can be triggered from your controller or view by throwing an `HttpException`. `ResponseRewrite` mode ensures that the URL of the original request is preserved. 

		<system.web>
		    <customErrors mode="On" redirectMode="ResponseRewrite">
		      <error statusCode="400" redirect="/masterpages/status400.aspx" />
		    </customErrors>
		</system.web>

2. We configure IIS custom errors in the Umbraco application, but only to be inherited by child applications in the same application pool. `PassThrough` means it's not applied here by IIS.   

		<system.webServer>
	 		<httpErrors existingResponse="PassThrough" errorMode="Custom">
	      		<remove statusCode="400" subStatusCode="-1"/>
      			<error statusCode="400" subStatusCode="-1" path="/masterpages/status400.aspx" responseMode="ExecuteURL"/>
    		</httpErrors>
		</system.webServer>

3. Applications with a separate IIS application scope below Umbraco go back to using the standard IIS configuration. This is inherited from the Umbraco configuration so long as the child application uses the same application pool, so all we need to do is reset the `httpErrors` response mode.

		<system.webServer>
			<httpErrors existingResponse="Replace"/>
		</system.webServer>

## When not using Umbraco

Away from Umbraco the configuration is a little different because we don't need to pass errors through to Umbraco's 404 handling. In this case we can turn on custom errors for both ASP.NET and IIS, but we only need to specify URLs in the IIS configuration.

	<system.web>
		<customErrors mode="On" />
	</system.web>

	<system.webServer>
		<httpErrors existingResponse="Replace" errorMode="Custom">
	   		<remove statusCode="400" subStatusCode="-1"/>
    		<error statusCode="400" subStatusCode="-1" path="/masterpages/status400.aspx" responseMode="ExecuteURL"/>
			<!-- Repeat <remove /> and <error /> for each status code to be configured -->
    	</httpErrors>
	</system.webServer>

The path to the error page must be in the same IIS application pool as the application being configured, otherwise IIS will return a blank `403 Forbidden` response instead of the custom error page. This may mean that child applications need to repeat the configuration in order to use updated paths. 

Any virtual directories in the requested path must have their feature permissions set to `Read, Script`. If `Script` is not enabled then requests with a file extension will return a 404 but extensionless URLs will return a 403. In both cases the page configured in the `<httpErrors />` section is displayed.

    <system.webServer>
        <handlers accessPolicy="Read, Script" />
    </system.webServer>