# Mango Store Build With Microservices Architecture
[![Mango Store - Microservices Repo Using .NET & CI (GitHub Actions) | ©2023.HongQuan](https://github.com/duong-hong-quan/mangostore_microservices/actions/workflows/dotnet.yml/badge.svg)](https://github.com/duong-hong-quan/mangostore_microservices/actions/workflows/dotnet.yml)
### Core Services
1. Product Service: Responsible for managing product-related information, such as product details, inventory, and pricing.
2. Order Service: Handles order processing, including order creation, payment verification, and order fulfillment.
3. Cart Service: Manages user shopping carts, allowing users to add, remove, and modify items in their carts.
4. Email Service: Sends transactional emails related to order confirmations, shipping updates, and other notifications.
5. Payment Service: Integrates with payment gateways to process payments securely.

## Implementation Details
1. Azure Service Bus (Main Branch)
- In the main branch of your project, you’ve chosen to use Azure Service Bus as the messaging system for communication between microservices. Here’s how it works:
- Event Publishing: When an event occurs (e.g., a new order is placed), the relevant service publishes an integration event to the Azure Service Bus topic.
- Event Subscriptions: Other services (such as the Email Service or Order Service) subscribe to specific topics. They receive notifications when relevant events occur.
- Reliable Communication: Azure Service Bus ensures reliable message delivery even in scenarios with high traffic or temporary service outages.

2. RabbitMQ (Develop Branch)
- In the develop branch, you’ve opted for RabbitMQ as the message broker for communication between services. Here’s how RabbitMQ fits into your architecture:
- Publish-Subscribe Model: RabbitMQ follows a publish-subscribe pattern. Services publish messages (events) to exchanges, which then route them to queues based on subscriptions.
- Queues and Exchanges: Each service has its queue where it listens for specific events. Exchanges route messages to the appropriate queues.
- Scalability and Flexibility: RabbitMQ allows you to scale horizontally by adding more instances or nodes. It’s well-suited for scenarios where you need fine-grained control over message routing.

## Contact Information
For any questions, feedback, or support, please feel free to contact us:
- Email: hongquan.contact@gmail.com

#### Copyright &#169; 2023 - Dương Hồng Quân
