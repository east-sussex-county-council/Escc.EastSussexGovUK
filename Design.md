# Sitewide design

The design of the [East Sussex County Council website](https://www.eastsussex.gov.uk) is mostly implemented in this project. It consists of a set of page elements which are reused on WebForms master pages and MVC layouts for both .NET Framework and .NET Core to avoid duplication.

The master pages and MVC layouts themselves are deliberately minimal, with most of the work done by remotely-hosted controls that together make up the website design. 

This allows all parts of the website, including third-party services and other applications hosted separately from the main website, to use a consistent approach to loading the template. The template is kept up-to-date and in sync across all parts of the site without having to keep track of changes and manually update the files in each location.

- Applying the sitewide design with [ASP.NET Core MVC](DotNetCoreMvc.md)
- Applying the sitewide design with [ASP.NET MVC5](DotNetFrameworkMvc.md)
- Applying the sitewide design with [ASP.NET WebForms](DotNetFrameworkWebForms.md)

## Serving the remote master page

The `Escc.EastSussexGovUK.TemplateSource` project defines the remote master page, and the controls which make up the page are served from a deployment of this project.

The site serving the remote master page should configure a `BaseUrl` in `web.config` inside the folder where `control.aspx` is hosted. The `BaseUrl` should be the domain where sitewide features like images and text size control are hosted (typically www.eastsussex.gov.uk). This `BaseUrl` is prepended to all relative links and images in the template, to create absolute links back to the central site. This means that the consuming application doesn't need to host its own copy of these files. The `BaseUrl` can include a `{hostname}` token which is replaced with the value of `HttpContext.Current.Request.Url.Authority`.

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>

      <Escc.EastSussexGovUK>
        <GeneralSettings>
          <add key="BaseUrl" value="https://www.eastsussex.gov.uk" />
        </GeneralSettings>
      <Escc.EastSussexGovUK>
	</configuration>

## An alternative approach for third-party applications

The approach described above is designed primarily with ASP.NET in mind, and does involve installing a NuGet package with minimal master pages or MVC layouts on the consuming site. 

An alternative and usually better approach for third-parties which need to use our design is for us to set up a page on our website using the remote master page, with tokens to indicate where the metadata, content and other elements should go, and then a third-party can download a copy of the page on a regular schedule (or on every request) and use it as a template in their application. The tokens, breadcrumb trail, CSS and other elements can be changed to suit the needs to the consuming application.

This has two main advantages: 

* it can fit in more easily with other technologies
* all of the template elements are controlled by us, which makes them easier (and cheaper) to keep up-to-date.

An example of this approach in use is the [modern.gov template](https://www.eastsussex.gov.uk/moderngov/template.aspx) used by our [democracy pages](https://democracy.eastsussex.gov.uk/).

## CSS, JavaScript and images

CSS, JavaScript and image files used by two or more applications belong in the `Escc.EastSussexGovUK.TemplateSource` project along with the remote master page. This includes the CSS for our responsive design. Documentation is included within the CSS files, and in our [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide). 

Our sitewide CSS and JavaScript files as part of the remote master page are minified using [YUI Compressor](https://www.nuget.org/packages/YUICompressor.NET.MSBuild).

The printer icon used in our CSS was made by [Yannick](http://yanlu.de). It's from [flaticon.com](http://www.flaticon.com) and licensed under [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/).