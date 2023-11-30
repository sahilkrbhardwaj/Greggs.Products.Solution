# Approach to User Stories

## User Story 1: Retrieve Latest Menu

1. **Integration of Data Access Layer:**
    - Implement integration of the existing data access layer (`IDataAccess<Product>`) into the API.
    - Modify the endpoint to utilize the data access implementation to fetch the latest menu of products.
    
2. **Controller Modification:**
    - Update the controller action to make use of the data access service (`IProductAccessService`) instead of the static list.
    - Ensure that the API endpoint returns the latest available products fetched from the data access layer.

## User Story 2: Retrieve Prices in Euros

1. **Currency Conversion Logic:**
    - Introduce a mechanism to handle currency conversion within the API.
    - Implement logic to convert product prices from GBP to Euros based on the specified exchange rate (1GBP to 1.11EUR).

2. **Controller Modification:**
    - Adjust the controller action to incorporate the currency conversion logic when retrieving the list of products.
    - Ensure that the API endpoint returns products with prices converted to Euros based on the exchange rate.

# Code Structure and Approach

1. **Dependency Injection:**
    - Utilize dependency injection to inject services/interfaces where required (e.g., `IDataAccess<Product>`, `ICurrencyExchangeAccess`) following SOLID principles.
  
2. **Service Layer Implementation:**
    - Leverage service layers (`ProductAccessService`, `CurrencyExchangeAccess`) for business logic and data access, ensuring separation of concerns.
  
3. **Middleware Configuration:**
    - Configure middleware for Swagger documentation (`AddSwaggerGen`, `UseSwagger`, `UseSwaggerUI`) to facilitate API documentation.
  
4. **Memory Caching:**
    - Utilize the in-memory cache (`IMemoryCache`) for storing and retrieving currency exchange data.

5. **Error Handling and Logging:**
    - Implement error handling mechanisms for edge cases and exceptions, utilizing the provided `ILogger` for logging critical events.
  
6. **Unit Testing:**
    - While assuming the data access functionality is fully tested, additional unit tests should be written for new functionalities, especially currency conversion and endpoint modifications.
  
7. **Readability and Documentation:**
    - Maintain code readability by following coding standards, commenting appropriately, and providing clear documentation for complex logic or functionalities.

# Working Style

- **Agile Methodology:** Utilizing an iterative approach, breaking down tasks into manageable units, and delivering incremental updates.

- **Continuous Integration:** Embracing CI/CD practices to ensure frequent integration and testing of changes.
