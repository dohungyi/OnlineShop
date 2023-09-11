using SharedKernel.Application;

namespace SharedKernel.Libraries;

[AttributeUsage(AttributeTargets.Class)]
public class AuthorizationRequestAttribute : Attribute
{
    public ActionExponent[] Exponents { get; } = new ActionExponent[] { ActionExponent.View };

    public AuthorizationRequestAttribute(ActionExponent[] exponents)
    {
        Exponents = Exponents.Concat(exponents).ToArray();
    }

    public AuthorizationRequestAttribute()
    {
    }
}