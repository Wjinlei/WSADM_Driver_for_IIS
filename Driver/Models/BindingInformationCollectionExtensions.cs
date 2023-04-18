using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public static class BindingInformationCollectionExtensions
{
    public static Result Check(
        this IBindingInformationCollection bindingInformationCollection,
        IBindingInformation bindingInformation)
    {
        var result = bindingInformation.Check();
        if (result.Success)
            return result;

        if (bindingInformationCollection.Contains(bindingInformation))
            return Result.Error(new ArgumentException("This binding information already exists"));

        return Result.Ok;
    }
}
