namespace Presentation.Attributes;

/// <summary>
/// Hide the endpoint from the OpenAPI documentation
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class HideFromScalarAttribute : Attribute, IApiDescriptionVisibilityProvider
{
    public bool IgnoreApi => true;
}
