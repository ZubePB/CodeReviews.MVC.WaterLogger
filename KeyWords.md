# Water Logger notes

## Attributes, Properties and Methods used
HTML/Razor tags and tag helpers:

| Name| subname | language| description
|---|---|---|---
| __value__| - |html| Attribute a return value to an element when submitted
| __type__| - |html| Determines the type of the element (used for elements like buttons or inputs)|
| __class__| - |html| Specifies classes that can be used to style the elements in the css files, and also access bootstrap classes
| __onclick__| - |html| Specifies a function in the jss to execute when clicked
| __asp-__| - |razor| Tag helper, help access the page model properties
| - |_area| razor| Helps with routing
| - |_page| Redirects to specified page (like an html href)
| - |_for| Links a property to the element
| - |_validation-for| Checks validations in relation to the datatype and binded properties conditions
| __@__| - |razor| Used to insert C# code into html
| __@page__| - |razor| Define a razor page
| __@model__| - |razor| A model with the same name of the page where data will be retrieved from
| __@section name {}__| - |razor| Use to write C# code 
| __@{}__| - |razor| Used to write C# code block
| __@Html__| - |razor| Gives access to different method to use the page model properties and translate them in the html file
| - |DisplayFor(function)|razor| Display tje property value specified in the function argument
| - |DisplayNameFor(function)|razor| Display the name of the property specified in the function argument
| - |RenderPartialAsync(partilView)|razor|Renders Html markup for specified partial view
| __ViewData["string"]__| - |razor| Attribute a value to the specif
| __OnGet()/OnPost()__| - |razor| Used to call a homonym function in C# to make Get/Post requests. Other actions can be performed too.

C#:

| Name| subname |type| description
|---|---|---|---
| __BindProperty__| - |Attribute|can specify a name or type to use for binding the associated property
| __DisplayFormat()__| - |Attribute|Specify how data fields are displayed
| - |DataFormatString|argument| Specifies the format for the binded data
| - |ApplyFormatEditMode|argument| Indicates if the data format is applied in edition mode
| __Range(min,max,errormessage)__| - |Attribute| Declare a range for the data to be valid and can provide an error message
| __IActionResult__| - |Data type| Type used in controllers providing standardize responses (ex: content responses,redirect responses,static responses, and more)
| __IConfiguration__| - |Data Type| Type used to access the app configuration
| - |Page()|Method| Returns the actual page and an IActionResult
| - |RedirectToPage(page)|Method| Redirects to specified page and an IActionResult
| __OnGet()__| - |Function| Used to initialized any state for a page
| __OnPost()__| - |Function| Handle the post action
| __ModelState__| - |Bind Property| Access the page Model State
| - |IsValid|Attribute| Check if the data model provided fulfill the validations


### Bootstrap
Bootstrap have a simple way to give elemts simple design and properties,<br>
most of the basic elements have a class that is simpy their type name (ex: Button = btn)<br>
or an expected behavior or shape (ex: row).<br>

Bootstrap has a proper syntax, using it's own scale, like the following:
- function-screenSize-intensity (mt-md-3 = margin top- medium screen - margin size of 3)
- behavior-screenSize-emplacement (col-md-4)
- element-type(color) (btn-danger = button-red text)
- element-function or function-element (form-group / control-label)


Other important things about bootstrap:
- rows are separated in 12 columns each
- templates can be found on [getbootstrap.com](https://getbootstrap.com/)