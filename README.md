# .NET Core API example
A .NET Core API example which interacts with Order Items through LINQ

This example is adapted from the template API code scaffolding. See ProductsController.cs for an example. Included are the following improvements:
- Clearer documentation (Controllers/OrderItems.cs)
- Use of a Data Transfer Object (OrderItem.cs -> OrderItemDto)
- Flattened representation of fields (Order Item -> Product, Order Item -> Order, Order Item -> Order -> Customer)

## To run this project
- Tools > NuGet Package Manager > Package Manager Console
- update-database
- Tools > SQL Server Object Explorer > Database
- Add Customer, Order, Product records
- Interact with Ordered Items through API requests

## Index of Examples
1. [Code First Migrations](https://github.com/christinebittle/CoreEntityFramework)
2. [Core API](https://github.com/christinebittle/CoreAPI)

## Test Your Understanding!
- Modify ProductsController.cs to use the same routing convention as OrderItemsController
- Create a ProductDTO
- In the ProductDto, omit the price field
- In ProductsController.cs, change ListProducts and FindProduct to return List\<ProductDto\> and ProductDto
- Document all methods in ProductsController.cs with descriptive summary blocks
- Plan for future method ListProductsForCategory(int CategoryId)
