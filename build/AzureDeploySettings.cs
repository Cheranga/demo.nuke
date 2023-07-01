namespace DefaultNamespace;

public record AzureDeploySettings(
    string ClientId,
    string ClientSecret,
    string TenantId,
    string SubscriptionId
);
