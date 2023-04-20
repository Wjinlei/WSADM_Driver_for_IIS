using WSADM;
using WSADM.Interfaces;

namespace Driver.Models;

public static class BindingCollectionExtensions
{
    public static Result Check(
        this IBindingInformationCollection bindingInformationCollection,
        IBindingInformation bindingInformation,
        ISiteCollection<ISite> sites)
    {
        var result = bindingInformation.Check();
        if (!result.Success)
            return result;

        if (bindingInformationCollection.Contains(bindingInformation))
            return Result.Error(new ArgumentException("This binding information already exists"));

        foreach (var site in sites)
        {
            if (site.Bindings.Contains(bindingInformation))
                return Result.Error(new ArgumentException("This binding information already exists"));
        }

        return Result.Ok;
    }
}
