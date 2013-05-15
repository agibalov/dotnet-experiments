aspnet-mvc-webapi-help-page-experiment
======================================

A lab aimed to understand if it's hard (and if _it is_, how hard it is) to implement a custom help page for ASP.NET MVC Web API. The original problem was that I really hate what happens when I add [Microsoft ASP.NET Web API Help Page 4.0.0](http://www.nuget.org/packages/microsoft.aspnet.webapi.helppage) via NuGet. It just generates a bunch of something weird, which I can't stand.

The requirements are:

1. Make a help page.
2. Page should list all methods available.
3. For every method, there should be a description, it's URL and request type.
4. For every method's parameter, there should also be a description and where it comes from (body vs. URI).
5. For every method's parameter, there should be a sample value (it's mostly useful for body parameters).
6. For method's return value, there should also be a sample.
7. I definitely don't want to use XML docs generation for description. I want to use `DescriptionAttribute`.

## Solution:

1. All reflection stuff is done via `IApiExplorer`. This service is available out of the box.
2. I use [AutoFixture](https://github.com/AutoFixture/AutoFixture) to generate sample objects (for parameters and returns).
3. I use [Json.NET](http://james.newtonking.com/projects/json-net.aspx) to render those objects as JSON.
4. I use [Bootstrap](http://twitter.github.io/bootstrap/) to layout the page.
5. There's custom `IDocumentationProvider` implemented - `IDocumentationProvider`. It checks `DescriptionAttribute` to provide descriptions for methods and their parameters.
