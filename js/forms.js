/* Common functions used on forms, picking up on classes applied to those forms */
if (typeof (jQuery) != 'undefined')
{
    /* Expand dependent fields only when their radio button is selected. 
       The data-groupname attribute can optionally be used to enable multiple independent groups of options on the same page.
       
        Example HTML:
       
        <span class="depends" data-enables="first" data-groupname="mygroup"><input type="radio" /><label /></span>
        <div class="dependentFields" id="first">
        ...
        </div>
    */
    $(function ()
    {
        var dependentFields = [];
        $(".depends").each(function ()
        {
            var enables = "#" + this.getAttribute("data-enables");
            if (enables.length > 1)
            {
                // When the form first loads, hide any groups of dependent options
                // where the option is not already selected
                if ($(":checked", this).length == 0) $(enables).hide();

                // Use data-groupname to support multiple groups of fields on one page
                var groupName = this.getAttribute("data-groupname");
                if (!groupName) groupName = "default";

                // Stash the id of the dependent fields for use in the click event
                if (!dependentFields[groupName]) dependentFields[groupName] = [];
                dependentFields[groupName].push(enables);
            }
        }).click(function ()
        {
            // Use data-groupname to support multiple groups of fields on one page
            var groupName = this.getAttribute("data-groupname");
            if (!groupName) groupName = "default";

            // If there are any sets of dependent fields in the group, ensure all but this are hidden and show this one.
            if (dependentFields[groupName])
            {
                var enables = "#" + this.getAttribute("data-enables");
                $(dependentFields[groupName].join(",")).not(enables).slideUp();
                $(enables).slideDown();

                // Option chosen, so select the first field within that option
                var fields = $("input,select,textarea", enables);
                if (fields.length) fields[0].focus()
            }
        });
    });

    // Apply HTML5 fields types based on classes. 
    // 
    // Intended as a progressive enhancement, which ought in time to be done properly by updating the source HTML. 
    // This will never work in IE because the type property is read-only, and the try/catch block is there to 
    // prevent IE displaying "Error on page". It also uses .setAttribute() rather than JQuery .attr() because 
    // JQuery prefers to throw an exception rather than try to change it.
    $(function ()
    {
        $("input[type=text].email").each(function () { try { this.setAttribute("type", "email"); } catch (err) {} });
        $("input[type=text].phone").each(function () { try { this.setAttribute("type", "tel"); } catch (err) { } });
        $("input[type=text].numeric").each(function () { try { this.setAttribute("type", "number"); } catch (err) { } });
    });
}