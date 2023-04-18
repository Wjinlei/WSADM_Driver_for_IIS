using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public static class BindingInformationExtensions
{
    public static Result Check(this IBindingInformation bindingInformation)
    {
        if (bindingInformation.IpAddr != "*"
            && !Mojito.Validator.IsIp(bindingInformation.IpAddr))
            return Result.Error(new
                ArgumentException(nameof(bindingInformation.IpAddr)));

        if (bindingInformation.Port < 1 ||
            bindingInformation.Port > 65535)
            return Result.Error(new
                ArgumentException(nameof(bindingInformation.Port)));

        return Result.Ok;
    }
}