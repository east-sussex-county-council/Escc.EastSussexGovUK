# Pop-up tips for form controls

The [General Data Protection Regulation (GDPR)](http://www.eugdpr.org/) requires us to inform people about what happens to their data at the time we collect it. For forms the [advice from the Information Commissioner's Office (ICO)](https://ico.org.uk/for-organisations/guide-to-data-protection/privacy-notices-transparency-and-control/?template=pdf&patch=33#justintime) is to use pop-up tips to provide this information where it is proportionate to do so.

We can do this in an accessible way using the [BeautyTips](https://github.com/east-sussex-county-council/BeautyTips) library. On a page where you want to use pop-up tips you need to include two JavaScript files which are hosted with our sitewide scripts.

**MVC:**

	@using ClientDependency.Core.Mvc
	@using Escc.ClientDependencyFramework
	
	Html.RequiresJs(JsFileAlias.Resolve("Tips"));
	Html.RequiresJs(JsFileAlias.Resolve("DescribedByTips"));

**WebForms:**

    <ClientDependency:Script runat="server" Files="Tips;DescribedByTips" />

In the HTML of your form you need to add the `aria-describedby` attribute to your form field with a value that matches the `id` attribute of another HTML element containing the content of the tip. Using `aria-describedby` ensures that the content of the tip is available to assistive technologies as they move through the form (tested with [NVDA](https://www.nvaccess.org/)).

By default the pop-up tip will appear on whichever side of the form control has the most space. You can change this by adding up to four positions in order of preference in a `data-tip-positions` attribute. The supported positions are `top`, `bottom`, `left` and `right`.

You also need to add the `describedby-tip` class to indicate that you want a pop-up tip, as there may be other reasons why `aria-describedby` is being used.

    <label for="example">Example</label>
    <input type="text" id="example" aria-describedby="example-help" class="describedby-tip" data-tip-positions="top right bottom" />
    <p id="example-help">We'll use this data for xxx. We won't use it for anything else or share it. You can ask us to delete it.</p>

You can wire up the tip from JavaScript instead of adding the `describedby-tip` class. This script adds a plugin to jQuery, which you can call on any jQuery element set:

	$("#element-that-needs-a-tip").describedByTip(); 

This also supports passing a [BeautyTips](https://github.com/east-sussex-county-council/BeautyTips) options parameter:

	var options = { width: 200 };
	$("#element-that-needs-a-tip").describedByTip(options); 


