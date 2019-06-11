# Rewriting links to email addresses

Rather than linking direct to email addresses using `mailto:` on our website, we prefer to link to a form that sends an email. This avoids putting an email address in the page where it can be picked up by spam robots. It also avoids opening an unwanted mail client when most people are using web mail.

`WebsiteFormEmailAddressTransformer` converts an instance of `ContactEmail` (from the [Escc.AddressAndPersonalDetails](https://github.com/east-sussex-county-council/Escc.AddressAndPersonalDetails) project) to the hard-coded URL of a form in the 'Contact us' section of our website.

	var emailTransformer = new WebsiteFormEmailAddressTransformer(Request.Url);
    var viewModel = new MyCustomViewModel
    {
        ExampleEmail = 
			emailTransformer.TransformEmailAddress(
				new ContactEmail(content.GetPropertyValue<string>("email"), "Email us")
			)
	};