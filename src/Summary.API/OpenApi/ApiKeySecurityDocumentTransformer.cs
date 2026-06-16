using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Summary.API.Authentication;

namespace Summary.API.OpenApi;

internal sealed class ApiKeySecurityDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes[ApiKeyAuthenticationOptions.SchemeName] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Name = ApiKeyAuthenticationOptions.HeaderName,
            Description = $"API key passed in the '{ApiKeyAuthenticationOptions.HeaderName}' header."
        };

        document.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference(ApiKeyAuthenticationOptions.SchemeName, document),
                []
            }
        });

        return Task.CompletedTask;
    }
}
