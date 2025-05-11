# Conversion of number to string.

The application consists of two components.
- StringToNumber REST API
- Test page

**API**

The StringToNumber API is a .NET8, C#, minimal API project using dependancy injection and Mediator pattern to build a processing pipeline. The pipeline initially validates the request and then handles of the requests - processes it.

The request pipeline comes through the following steges
- Api controller => NumberToTextApi.Controllers.ConverterController.cs
- Request validation => NumberToTextApi.Application.Validators.Converter.ConvertNumberQueryValidator.cs
- Request processing => NumberToTextApi.Application.Handlers.Converter.ConvertNumberHandler.cs

The handler uses takes the query object containing the requested decimal value.
The value is then split into the before decimal separator and after decimal separator. Each of them is then converted into appropriate string and joined together as main number and fraction part.

The initial value could be provided as string or decimal. Each has its own benefits and drawbacks. The string value could contain bigger values but formatting would need to be carefully managed. Culture can impact how decimal section is separated and if thousands are delimited too.

Using decimal type ensures value is always held in the same way and processing can be done on actual numbers. Culture and therefore decimal and thousands separators are not an issue. This was considered from the begining of the implementation.

**Testing**

The .NET solution contains test projects which can be executed in VS. The _NumberToTextApi_.ApplicationTest project tests the handler and how the numerical values are converted to string.

**Runing the project**

The .NET API project can be opened in VS2022 and run specifying _NumberToTextApi_ as the startup solution. The API project has Swagger test API added to it. At runtime a Swagger test page opens which can be used to test the API endpoint.

The other way to test the solution is through the _NumberToText.htm_ page. Once the API project is started the _NumberToText.htm_ page can be opened and value submitted to the API through it. Its a basic page submitting values and showing response values or error messages.
