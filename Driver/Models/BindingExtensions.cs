using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public static class BindingExtensions
{
    public static Result Check(this IBindingInformation bindingInformation)
    {
        if (bindingInformation.Address != "0.0.0.0"
            && !Mojito.Validator.IsIp(bindingInformation.Address))
            return Result.Error(new ArgumentException(nameof(bindingInformation.Address)));

        if (bindingInformation.Port < 1 ||
            bindingInformation.Port > 65535)
            return Result.Error(new ArgumentException(nameof(bindingInformation.Port)));

        return Result.Ok;
    }
}