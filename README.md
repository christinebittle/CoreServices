# .NET Core Services & Relational CRUD
This example factors our previous API functionality into a service, linked through an interface. It also extends CRUD to Products and Customers.

## To run this project
- Tools > NuGet Package Manager > Package Manager Console
- update-database
- Tools > SQL Server Object Explorer > Database
- Add Customer, Order records
- Interact with Ordered Items, Products, Categories through API requests

## Index of Examples
1. [Core Entity Framework](https://github.com/christinebittle/CoreEntityFramework)
2. [Core API](https://github.com/christinebittle/CoreAPI)
3. [Core Services](https://github.com/christinebittle/CoreServices)

## Test Your Understanding!
- Implement data access methods in Services/CustomerService.cs (defined in ICustomerService.cs)
- Associate the CustomerService implementation with the ICustomerService interface in Program.cs
- Create an API CustomerController.cs
- Use dependency injection in the CustomerController.cs API to receive the ICustomerService interface
- Use API endpoints + ICustomerService definitions to Create, Read, Update, Delete customers on each request
- Create an interface Interfaces/IOrderService.cs
- Create data access methods in Services/OrderService.cs
- Use dependency injection to receive the database context in OrderService.cs
- Repeat the same steps you did for customers!
- IOrderService.cs will need an additional definition: ListOrdersForCustomer
