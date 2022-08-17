## StockAnalyzer
### C#.NET Developer Project

Overview: Build a REST web service that return stock ticker prices for a given ticker symbol and date. 
The completed developer project should be delivered in a zip file.

Project Setup: Create a new a .NET Solution that contains three projects 
1) an ASP.NET MVC 5 or Web API 2 project that uses a dependency injection framework, 
2) a .NET Class Library that contains the business logic and data access layers, 
3) and a unit testing project. Please use NuGet Package Manager to manage any additional libraries needed for the projects.

Requirements: Three endpoints are required for the completion of the developer project. 
1) An endpoint that expects a stock ticker symbol, returns a stock model with the most recent price and most recent date. 
2) An endpoint that expects a stock ticker symbol and date, returns a stock model with the price for the given date. 
3) An endpoint that returns a distinct (ordered alphabetically) list of all stock ticker symbols that are available in the application.

Testing Requirements: Please create a unit test case for each of the following scenarios. 
1) When requesting an existing stock with no date, assert that the business logic layer returns a stock model with the expected name, symbol, most recent price, and most recent date. 
2) When requesting an existing stock with an existing date, assert that the business logic layer returns a stock model with the expected name, symbol, most recent price, and expected date. 
3) When requesting a non existing stock with no date, assert that your business logic layer throws an exception with a message telling the user that stock does not exist. 
4) When requesting an existing stock for a non existing date, assert that your business logic layer throws an exception with a message telling the user that date for the given stock does not exist. 
5) When requesting a list of all stock ticker symbols, assert that the count of stock ticker symbols matches what you would expect.

Tips: The .NET Class Library should follow modern design patterns and coding best practices. 
No database is required so please feel free to hardcode the stocks, prices, and dates in your .NET Class Library.

Data Models: The stock model must contain a name, ticker symbol, price, and date.

