# AisSagaStateMachine
This repo demonstrates the implementation of the Saga pattern using the MassTransit library.

## Student Registration
In this scenario, three services are going to be used to illustrate the process of performing the saga pattern and managing the consistency of data. The services are:
<b>StudentService:</b> Responsible for registering a student
<b>RegisterStudentService:</b> Responsible for generating a student
<b>EmailService:</b> Responsible for sending student information to the user; current implementation is logging to database; an actual email is not sent.

![SagaDiagram](./docs/overview.png)
As shown in the above image, a client will send a request to StudentService to register a student, then an AddStudentEvent will occur and send a message to RegisterStudentService and if the message contains valid values, it will send a message to EmailService to send a confirmation email that contains all information of student to the client. If one of the Services of RegisterStudentService or EmailService retrieves an invalid message the Cancel event will occur and incoming data will be removed from all Service's database.

## Prerequisites
- C# and .NET Core
- Visual Studio or VSCode
- Entity Framework core
- SQL server
- MassTransit
- RabbitMQ and/or Azure Service Bus
- Docker
- Postman / Thunderclient


## Launching the project

### SharedSettings.json (Shared folder)

Place a file called SharedSettings.json in the Shared folder in the root of the project. This file contains the connection strings and other settings that are shared between the services.

``` json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "BrokerType": "ASB",
  "SagaQueueName": "saga-queue",
  "ConnectionStrings": {
    "DbConnection": "Server=tcp:ais-db,1433;Initial Catalog=student_service;User ID=<SQL server user name>;Password=<SQL server password>;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;TrustServerCertificate=True;",
    "AzureServiceBus": "Endpoint=sb://vsaisdev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<key>",
    "RabbitMQ": "amqp://guest:guest@localhost:5672/"
  }
}

```

|Setting|Description|example|
|---|---|---|
|BrokerType|ASB for Azure Service Bus or RabbitMQ for RabbitMQ|ASB|
|SagaQueueName|Name of the saga queue|saga-queue|
|AllowedHosts|Allowed hosts|*|

|ConnectionStrings|Description|
|---|---|
|DbConnection|Connection string to the student-service database|
|AzureServiceBus|Azure Service Bus connection string|
|RabbitMQ|RabbitMQ connection string|


### VSCode

A Launch configuration is provided in the launch.json file (.vscode) with the necessary configurations to start any or all services for debugging:
- EmailService
- RegisterStudent
- SagaService
- StudentService
- Launch All

For each configuration is an equivalent with the extenstion "(EF Update)" that will update the database(s) prior to launching.


### Docker

To launch the services locally in docker containers, run the following command:for RabbitMQ

``` bash
bash start_containers.sh -s All -b RabbitMq
```

or for Azure Service Bus:

``` bash
bash start_containers.sh -s All -b ASB
```

### Testing

In the Test folder run the tests in StudentTests.http.

### 

# Deployment to Azure

## Prerequisites

- Azure account
- Azure CLI installed
- Docker installed
- Subscription with container registry access
- Basic understanding of containerization

## Deploying to Azure Container Instances
```	mermaid
sequenceDiagram
    participant Dev as Developer
    participant Script as Deploy Script
    participant ACR as Azure Container Registry
    participant ACI as Azure Container Instances
    
    Dev->>Script: Run deployment script
    Script->>ACR: Login to registry
    Script->>Script: Build containers
    Script->>ACR: Push containers
    Script->>ACI: Deploy container groups
```
``` mermaid
classDiagram
    class Student {
        +string StudentId
        +string Title
        +string Email
        +DateTime RequireDate
        +int Age
        +string Location
        +string StudentNumber
        +DateTime CreatedDate
    }
    
    class StudentServices {
        +Task<Student> AddStudent(Student student)
        +bool DeleteStudent(string StudentId)
    }
    
    class StudentController {
        +Post(AddStudentDTO addStudentDTO)
    }
    
    StudentController --> StudentServices
    StudentServices --> Student
```

In the Scripts folder run the following command:

``` bash
./deploy_container_instances.sh -r aisapis -g rg-ais-apis-student
```


