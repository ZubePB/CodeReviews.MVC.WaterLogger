# Water Logger notes

## Get familiar with the Application
### Folders and Files:
- __Connected Services__: (not used here) Help connect with services in the cloud and other APIs
- __Dependecies__: Visualizing everything needed for the projet to run
- __Pages__: Generate html pages
	- __Shared__: contain anything that can be used in multiple parts
        - _Layout.cshtml_: determine the general page structure
        - _ _ValidationScriptsPartial.cshtml_: inclue partial scripts used to validate data
	- _ _ViewImports.cshtml_: centralize everything the is used repetedly
	- _ _ViewStart.cshtml_: code run at the moment of each page execution
	- _Error.cshtml_: the page displayed in case of error
	- _Index.cshtml_: default home page
	- _Privacy.cshtml_: Privacy policy
- __Properties__:
    - _launchSettings.json_: how project run
- __wwwroot__: contains all static files
    - __css__: styling
	- __js__: scripts
	- __lib__: libraries the app rely upon(jquery and bootstrap)
- _appSettings.json_: app configurations
- _Program.cs_: starting point and setup

### Basic Project
A simple page with a home page and a privacy page.<br>
It is fully responsive using bootstrap.<br>
It's the result of scaffolding (copying/cloning a template).<br>
Other templates exists for web apps and other type of apps.

### Clean up unused part for the project
- Delete unused part of the layout (here the privacy link)
- Delete Error.cshtml and Privacy.cshtml
- Delete the html content from index.cshtml
- Delete app.UseAuthorization() from Program.cs

## Start Working
### Database
Here DBBrowser for Sqlite will be used.
- Add a connection string to appSettings.json and create the db manually:
```
[DB Browser]

CREATE TABLE IF NOT EXISTS drinking_water(
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Date TEXT,
Quantity INTEGER)
```

### Frist code
#### Generated code overview
Index.cshtml:
```
@page
[.cshtml]

@model IndexModel
@{
    ViewData["Title"] = "Home page";
}
```

- C# and HTML are linked using razor syntax.
- '@' is used to insert C# code into html
    - @page: define a razor page
	- @model: a model with the same name of the page where data will be retrieved from
	- @{}: used to write code

#### First element create
```
[html]

<button type="button" class="btn btn-succes" asp-area="" asp-page="/Create">Add Record</button>
```

A simple button with some attribute:
- type: determine the type of button
- class: use bootstrap for styles
- asp-...: tag helper
    - area: helps with routing (not used here)
	- page: redirect to sepcified page (translate to an html href tag redirecting to a page)
Tag helpers are used so the program knows how to handle data and what to do with it.

#### Create e new Page
Add a new razor page name 'Create' with a page model and add some html:
```
[html]

<h4>Add Record</h4>
<hr/>
<div class="row">
    <div class="col-md-4">
        <form method="post">
            <div class="form-group">
                <label class="control-label" asp-for="DrinkingWater.Date"></label>
                <input class="from-control" type="date" asp-for="DrinkingWater.Date" />
                <span asp-validation-for="DrinkingWater.Date" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label class="control-label" asp-for="DrinkingWater.Quantity"></label>
                <input class="form-control" type="number" asp-for="DrinkingWater.Quantity" />
                <span asp-validation-for="DrinkingWater.Quantity" class="text-danger"></span>
            </div>
            <div class="form-group">
                <button type="submit" value="Create" class="btn btn-success" />
            </div>
        </form>
    </div>
</div>
```
DrinkingWater is the data model that'll be used.<br>
Tag helpers:
- asp-for: link the property to the element
- asp-validation-for: check validation in relation to the data type used

#### Create the Model
In a new Models folder a DrinkingWater class is created.<br>
This model will be able to store all useful values needed to interact with data.
```
[C#]

namespace WaterLogger.Models;

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}
```

#### PageModel
It contains the code behind the pages with methods representing HTTP Requests.
```
[C#]

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WaterLogger.Pages;

public class CreateModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}
```
- IActionResults: Type used in controllers providing standardize responses (ex: content responses,redirect responses,static responses, and more)
- OnGet(): Initialize any state for the page
    - Page(): IActionResults, return the actual page

Then the datatype model is added to the page:
```
[C#]

[BindProperty]
public DrinkingWater DrinkingWater {get;set;}
```

#### PageModel code
The connection string is injected using dependecy injection:
```
[C#]

public IConfiguration _configuration { get; set; }
...
public CreateModel(IConfiguration configuration) => _configuration = configuration;
```

Then an OnPost() Method is created to be triggered when the form is submitted:
```
[C#]

using Microsoft.Data.Sqlite;
...
public IActionResult OnPost()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }
    using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO drinking_water(date,quantity) VLUES('{DrinkingWater.Date.ToString("yyyy.MM.dd HH:mm")}','{DrinkingWater.Quantity}'";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    };

    return RedirectToPage("/Index");
}
```
- Microsot.Data.Sqlite: Allows to use sqlite to connect to the database
- __ModelState__: used to validate a form and tell if there are errors 
- __SqliteConnection__: it is in a using statement so it can be disposed after its use to avoid using memory for nothing.
- _RedirectToPage()_: IActionResults, redirects to specified page

## Pursuing the application
All the records will be displayed on the page.<br>
To do it, the data need to be retrieved from the database and injected into the html.
### Retrieving data
__Index.cshtml.cs__:<br>
The data is retrieved by reading a raw sqlite command and parse its values into new DrinkingWater object that are then put into a List.<br>
The code is then as following:
```
[C#]

public class IndexModel : PageModel
{
    public IConfiguration _configuration { get; set; }
    public List<DrinkingWater> Records { get; set; }

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
        Records = GetAllRecords();
    }

    private List<DrinkingWater> GetAllRecords()
    {
        List<DrinkingWater> AllRecords = [];
        using(SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand sqliteCmd = connection.CreateCommand();
            sqliteCmd.CommandText = "SELECT * FROM drinking_water";
            List<DrinkingWater> tableData = [];
            SqliteDataReader reader = sqliteCmd.ExecuteReader();

            while (reader.Read())
            {
                AllRecords.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.Parse(reader.GetString(1),CultureInfo.CurrentUICulture.DateTimeFormat),
                        Quantity = reader.GetInt32(1),
                    });
            }
            return AllRecords;
        }
    }
}
```
- The data is stokcked in a 'Records' property so it can be used by razor syntax
- As it was done for the 'Create' page, the configuration is used to access db connection string

__Index.cshtml__:<br>
The data need to be showned.<br>
To do this, a table will be created using the data present in Model.Records.<br>
First the headers must be created:
```
[html]

<table id="records" class="table">
    <thead>
        <tr>
            <th>@Html.DisplayNameFor(model => model.Records[0].Date)</th>
            <th>@Html.DisplayNameFor(Model => Model.Records[0].Quantity)</th>
            <th></th>
        </tr>
    </thead>
</table>
```
- @Html.DisplayNameFor(): method that takes a function to get the name of a property and display it
<br>
Then the table must be filled with data using razor syntax:
```
[html]

<table id="records" class="table">
    ...
    <tbody>
        @foreach(var record in Model.Records)
        {
            <tr class="list-row">
                <td>@Html.DisplayFor(model => record.Date)</td>
                <td>@Html.DisplayFor(Model => record.Quantity)</td>
                <td>
                    <a type="button" class="btn-danger" asp-page="./Delete" asp-route-id="@record.Id">Delete</a>
                    <a button type="button" class="btn btn-primary" asp-page="./Create" asp-route-id="@record.Id">Edit</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

### Delete Page
It's structure is similar to the Index page.<br>
Instead of getting all records, only one will be retrieved using it's Id:
```
[C#]

public class IndexModel : PageModel
{
    public IConfiguration _configuration { get; set; }
    public List<DrinkingWater> Records { get; set; }

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
        Records = GetAllRecords();
    }

    private List<DrinkingWater> GetAllRecords()
    {
        List<DrinkingWater> AllRecords = [];
        using(SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand sqliteCmd = connection.CreateCommand();
            sqliteCmd.CommandText = "SELECT * FROM drinking_water";
            List<DrinkingWater> tableData = [];
            SqliteDataReader reader = sqliteCmd.ExecuteReader();

            while (reader.Read())
            {
                AllRecords.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.Parse(reader.GetString(1),CultureInfo.CurrentUICulture.DateTimeFormat),
                        Quantity = reader.GetInt32(1),
                    });
            }
            return AllRecords;
        }
    }
}
```

An OnPost() method will be used to delete the object.<br>
It is used instead of an OnDelete() method because razor pages are made to be used with views.<br>
As views are using forms and forms don't support a delete method, another method must be used instead:
```
[C#]

public IActionResult OnPost(int id)
{
    using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
    {
        connection.Open();
        SqliteCommand sqliteCmd = connection.CreateCommand();
        sqliteCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = {id}";
        sqliteCmd.ExecuteNonQuery();
        connection.Close();
    }
    return RedirectToPage("./Index");
}
```

The view will be used to have a confirmation from the user and to show information about the record:
```
[html]

@page
@model WaterLogger.Pages.DeleteModel
@{
}

<h3>Are you sure you want to delete this record?</h3>

<div>
    <dl class="row">
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.DrinkingWater.Date)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.DrinkingWater.Date)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.DrinkingWater.Quantity)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayNameFor(model => model.DrinkingWater.Quantity)
        </dd>
    </dl>
    <form method="post">
        <input type="hidden" asp-for="DrinkingWater.Id" />
        <input type="submit" value="Delete" class="btn btn-danger" /><br />
        <a asp-page="./Index">Back to list</a>
    </form>
</div>
```

The properties are again accessed using the @Html tag.<p>
The form contains of an hidden input, related to the Id as a way to use it for the deleting method.<br>

### Update record
The base structure of the page is the same as the delete page:
```
[C#]

public class UpdateModel : PageModel
{
    public IConfiguration _configuration { get; set; }
    [BindProperty]
    public DrinkingWater DrinkingWater { get; set; }
    public UpdateModel(IConfiguration configuration) => _configuration = configuration;
    public void OnGet(int id)
    {
        DrinkingWater = GetById(id);
    }

    private DrinkingWater GetById(int id)
    {
        DrinkingWater drinkingWater;
        using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand sqliteCmd = connection.CreateCommand();
            sqliteCmd.CommandText = $"SELECT * FROM drinking_water WHERE Id = {id}";
            List<DrinkingWater> tableData = [];
            SqliteDataReader reader = sqliteCmd.ExecuteReader();
            reader.Read();
            drinkingWater = new DrinkingWater
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1), CultureInfo.CurrentUICulture.DateTimeFormat),
                Quantity = reader.GetInt32(2),
            };
            return drinkingWater;
        }
    }
}
```

The OnPost() method will be different.<br>
Again, a post method is used because razor pages don't support an OnUpdate() method:
```
[C#]


public IActionResult OnPost()
{
    if (ModelState.IsValid)
    {
        using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand sqliteCmd = connection.CreateCommand();
            sqliteCmd.CommandText = $"UPDATE drinking_water SET Date ='{DrinkingWater.Date.ToString("yyyy.MM.dd HH:mm")}', Quantity{DrinkingWater.Quantity} WHERE Id = {DrinkingWater.Id}";
            sqliteCmd.ExecuteNonQuery();
            connection.Close();
        }
        return RedirectToPage("./Index");
    }
    return Page();
}
```

The view will contain a form similar to the delete page too.
```
[html]

@page
@model WaterLogger.Pages.UpdateModel
@{
}

<h4>Update Record</h4>
<hr />
<div class="row">
    <div class="col-md-4">
        <form method="post">
            <input type="hidden" asp-for="DrinkingWater.Id" />
            <div class="form-group">
                <label asp-for="DrinkingWater.Date" class="control-label"></label>
                <input type="date" asp-for="DrinkingWater.Date" class="form-control" />
                <span asp-validation-for="DrinkingWater.Date" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="DrinkingWater.Quantity" class="control-label"></label>
                <input type="number" asp-for="DrinkingWater.Quantity" class="form-control" />
                <span asp-validation-for="DrinkingWater.Quantity" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Update" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-page="./Index">Back to list</a>
</div>
```

A razor section for scripts is put at the bottom of the file, to ensure validation have been completed before updateing the object:
```
[html]


@section Scripts {
    @{
        await Html.RenderPartialAsync("_ValidationScriptsPartial");
    }
}
```

## Styling and others
The code for the application to run correctly is finished.<br>
But it doent's look great yet, a bit of styling can be done to imporve the user experience.
### Adding images
A new 'img' folder is added to the 'wwwroot' folder.<p>
Some bootstrap values are deleted from the navbar to make it transparent.<br>
The water drinking label next to the title is also deleted.<br>
A logo is then inserted instead of the title.
```
_layout.cshtml
[html]

<div class="container">
    <a class="navbar-brand" asp-area="" asp-page="/Index"><img class="logo" src="/img/water_logo.png" /></a>
...
<div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
    <ul class="navbar-nav flex-grow-1">
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-page="/Index">Water Drinking Logger</a>
        </li>
    </ul> 
</div>

...
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3">
        ...
```

The logo is there but is too big.<br>
Some css is added to the /wwwroot/css/site.css file that contain the global css styling for the app:
```
[CSS]


.logo{
    width: 80px;
}
```

### modifying the background
the background will be modifyed using a new div and attributing it a new class.<br>
The class will then be styled to set the background up.
```
_layout.cshtml
[html]

<body>
    <div class="background">
    ...
```
```
[CSS]


.background{
    position: relative;
    z-index: 1;
    height: 100vh;
}

    .background::before {
        content: "";
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        z-index: -1;
        background-image: url(https://images.pexels.com/photos/416528/pexels-photo-416528.jpeg?cs=srgb&dl=pexels-pixabay-416528.jpg&fm=jpg);
        opacity: .5;
        background-size: cover;
    }
```
The CSS ensure that the background is the same height of the veiwport (100vh),<br>
that it stays in place, and uses the linked image.
### Modifying the buttons
The button will be modified using generated palette (from coolors.co here).<br>
The palette will be generate from an image.<br>
The color values obtained with the palette will be used to overwrite some of the bootstrap class styling:
```
[CSS]

.btn-danger {
    background-color: #5c5c5c;
    border: none;
}

.btn-primary{
    background-color: white;
    border: none;
    color: #15446c;
}

.btn-success{
    background-color: #15446c;
    border: none;
}
```

Then icons are added to each button

```
Index.cshtml
[html]

<a button type="button" class="btn btn-success" asp-area="" asp-page="/Create">
    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-plus" viewBox="0 0 16 16">
        <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"></path>
    </svg>
    Add Record
</a>

...
<td>
    <a button type="button" class="btn btn-danger" asp-page="./Delete" asp-route-id="@record.Id">
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
            <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
        </svg>
    </a>

    <a button type="button" class="btn btn-primary" asp-page="./Update" asp-route-id="@record.Id">
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pen" viewBox="0 0 16 16">
            <path d="m13.498.795.149-.149a1.207 1.207 0 1 1 1.707 1.708l-.149.148a1.5 1.5 0 0 1-.059 2.059L4.854 14.854a.5.5 0 0 1-.233.131l-4 1a.5.5 0 0 1-.606-.606l1-4a.5.5 0 0 1 .131-.232l9.642-9.642a.5.5 0 0 0-.642.056L6.854 4.854a.5.5 0 1 1-.708-.708L9.44.854A1.5 1.5 0 0 1 11.5.796a1.5 1.5 0 0 1 1.998-.001zm-.644.766a.5.5 0 0 0-.707 0L1.95 11.756l-.764 3.057 3.057-.764L14.44 3.854a.5.5 0 0 0 0-.708l-1.585-1.585z" />
        </svg>
    </a>
</td>
```

SVG(Scalable Vector Graphics) are used to make the icon.<br>
It is basically like a pen following a given path and given colors to draw,in general, small icons.

## #Using Javascript with Razor
To see what can be done with javascript, a button will created using a calculate() method.
```
Index.cshtml
[html]

...
<button type="button"class="btn btn-success" onclick="calculate()">Calculate</button>

</tbody>
</table>

<div>@ViewData["Total"]</div>
<div id="result"></div>
```

The function is the declared in the /wwwroot/js/site.js file as following:
```
[JS]

function calculate() {
    var tbl = document.getElementById("records");
    var resultArea = document.getElementById("result");

    var result = 0;

    for (var i = 1; i < tbl.rows.length; i++) {
        result = result + +tbl.rows[i].cells[1].innerHTML;
    }

    resultArea.append(`${result}`)
}
```

### Validation
Data annotation can be used to specify validations for each data.<br>
Many options are avalaible such as range for numbers or regex.<br>
Those a made over the model properties/attributes:
```
[C#]

public class DrinkingWater
{
    public int Id { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd-MM-yy}",ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }
    [Range(0,Int32.MaxValue,ErrorMessage = "Value for {0} must be positive")]
    public int Quantity { get; set; }
}
```
The error validations and message will be displayed with the help of the validation scripts.

## Conclusion
Razor syntax make HTML accessible for C# developper and provide an way to adapt to its usage.<br>
Basic HTML, CSS and JS are still needed, but nothing too deep is necessary for this project.<br>
Bootstrap and libraries is a good way to not have to interact too much with CSS and JS code-wise<br>
and still provide their uses easily.<br>
Only Get and Post method are allowed, making each page not neing heavy on code and favorizing separate concerns<br>
for each of them.<br>
I made a document name 'KeyWords.md' where the methods and attributes used in this tutorial are listed to help me<br>
on the second part of the project.