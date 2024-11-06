namespace MessageBrokers
{
    public class ASBConfig
    {
        // To ensure that MassTransit has sufficient permissions to perform queue 
        // management as well as messaging operations. 
        // Your identity & managed identity will need to have the correct role assignments within Azure.
        // Assigning the role Azure Service Bus Data Owner will provide sufficient permissions for 
        // Mass Transit to function on the namespace.
        // For more information: https://masstransit.io/documentation/configuration/transports/azure-service-bus#using-azure-managed-identity
        public const string ASBURL = "sb://<namespace>.servicebus.windows.net";
    }
}
