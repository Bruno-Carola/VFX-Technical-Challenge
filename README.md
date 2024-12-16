# **Exchange Rate Manager**

## **Overview**
Exchange Rate Manager is a .NET Core Web API that provides CRUD operations for managing foreign exchange rates. The application is designed to fetch exchange rate data from the Alpha Vantage API when unavailable in the local database and leverages RabbitMQ to raise events when new rates are added.

This project demonstrates robust practices in API development, including integration with external services, database management, message queue systems, and unit testing.

---

## **Features**
- **CRUD Operations**: Manage exchange rates with bid and ask prices for currency pairs.
- **Data Fetching**: Automatically fetch rates from Alpha Vantage when not available locally.
- **Message Queue Integration**: Publish events to RabbitMQ whenever new rates are added.
- **Error Handling**: Comprehensive logging and error handling for reliability.
- **Unit Testing**: Fully tested handlers, controllers, and services.

---

## **Technologies Used**
- **.NET Core 9**
- **Entity Framework Core** for database access.
- **RabbitMQ** for message queuing.
- **Alpha Vantage API** for fetching real-time exchange rates.
- **MediatR** for implementing CQRS.
- **Microsoft SQL Server** for data storage.
- **xUnit** and **Moq** for unit testing.
- **Serilog** for structured logging.

---

## **Setup Instructions**

### **Prerequisites**
1. Install [.NET Core 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
2. Install and configure [RabbitMQ](https://www.rabbitmq.com/).
3. Set up a local or remote SQL Server instance.
4. Obtain an API key from [Alpha Vantage](https://www.alphavantage.co/support/#api-key).

### **Step 1: Clone the Repository**
1. Open Visual Studio 2022.
2. From the **Start Window**, click on **Clone a repository**.
3. Paste the repository URL:
   ```
   https://github.com/Bruno-Carola/VFX-Technical-Challenge.git
   ```
4. Select a folder to clone the repository into and click **Clone**.

### **Step 2: Configure the Project**
1. In Visual Studio 2022, open the `appsettings.json` file.
2. Update the following sections:
   - **Database Connection String**:
     ```json
     "ConnectionStrings": {
        "VFXFinancialDB": "Server=(localdb)\\MSSQLLocalDB;Connection Timeout=30;Command Timeout=30;Persist Security Info=False;TrustServerCertificate=True;Integrated Security=True;Initial Catalog=VFXFinancialDB"
      },
     ```
   - **RabbitMQ Configuration**:
     ```json
     "RabbitMQ": {
       "HostName": "localhost",
       "Port": 5672,
       "UserName": "guest",
       "Password": "guest"
     }
     ```
   - **Alpha Vantage Configuration**:
     ```json
     "AlphaVantage": {
       "ApiKey": "your_alpha_vantage_api_key"
     }
     ```

### **Step 3: Run Migrations**
1. Run Migrations to create localDB and apply all schema.

### **Step 4: Start the Application**
1. In Visual Studio 2022, set the startup project to `ExchangeRateManager`.
2. Press `F5` or click on **Start Debugging** to run the application.

### **Step 5: Test the Application**
The API will be available at `http://localhost:5000`.

---

## **Endpoints**
Here’s a summary of the available API endpoints:

### **Exchange Rate Endpoints**
| Method | Endpoint                          | Description                      |
|--------|-----------------------------------|----------------------------------|
| POST   | `/api/ExchangeRate`               | Create a new exchange rate.      |
| GET    | `/api/ExchangeRate/{from}/{to}`   | Get an exchange rate by currency pair. |
| PUT    | `/api/ExchangeRate/{id}`          | Update an exchange rate.         |
| DELETE | `/api/ExchangeRate/{id}`          | Delete an exchange rate.         |

### **Sample Request**
**Create a new exchange rate**:
```json
POST /api/ExchangeRate
{
  "FromCurrency": "USD",
  "ToCurrency": "EUR",
  "Bid": 1.10,
  "Ask": 1.12
}
```

**Response**:
- **201 Created**: Exchange rate successfully created.
- **500 Internal Server Error**: Error occurred.

---

## **Testing**
The solution includes comprehensive unit tests for handlers, controllers, and services.

### **Run Tests**
1. Open the **Test Explorer** in Visual Studio 2022.
2. Click on **Run All** to execute all unit tests.

---

## **Event System**
This project integrates with RabbitMQ to publish events when new exchange rates are created.

### **Event Details**
- **Event Name**: `ExchangeRateAdded`
- **Message**:
   ```json
   {
     "FromCurrency": "USD",
     "ToCurrency": "EUR",
     "Bid": 1.10,
     "Ask": 1.12,
     "Timestamp": "2024-12-15T12:00:00Z"
   }
   ```

---

## **Future Enhancements**
- Explore other database options.
- Implement authentication and authorization.
- Extend message queue integration for other operations like updates or deletions.
- Consume the published events from RabbitMQ for caching, notifications, analytics or purposes.
- Introduce caching for frequently requested exchange rates.

---

## **Contact**
For questions or suggestions, feel free to reach out:
- **Email**: bruno.carola17@gmail.com
- **GitHub**: [Bruno-Carola](https://github.com/Bruno-Carola)

