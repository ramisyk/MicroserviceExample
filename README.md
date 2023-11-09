# Microservice Example
This repository is developed for Microservice Training.
Microservice Example includes 4 diffenent projects and all projects include several things like below:
- Shared: The project named Shared includes Events and Messages. Events include properties what will happen when it happen... Messages refer to transformed data in events like Data Transfer Objects or View Models.
    - Events:
        * Order Created Event
        * Stock Reserved Event
        * Stock Not Reserved Event
        * Payment Completed Event
        * Payment Failed Event
    - Messages: 
        * OrderItemMessage
- Order.API: Order API throw events to Stock API when order created and listen Stock API and Payment API with consumers.
    - Functions:
        * Create new order function in Orders Controller.
    - Consumers:
        * Stock Not Reserved Event Consumer
        * Payment Completed Event Consumer
        * Payment Failed Event Consumer
- Stock.API: Stock API listen Order API with consumer.
    - Consumers:
        * Order Created Event Consumer
- Payment.API: Payment API listen Stock API with consumer.
    - Consumers:
        * Stock Reserved Event Consumer