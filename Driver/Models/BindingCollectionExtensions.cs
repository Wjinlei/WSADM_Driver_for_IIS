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

        // Check that the binding information already exists at the current site
        if (bindingInformationCollection.Contains(bindingInformation))
            return Result.Error(new ArgumentException(
                $"Binding information [{bindingInformation}] already exists"));

        // Check if the binding information already exists on another site
        foreach (var site in sites)
        {
            if (site.Bindings.Contains(bindingInformation))
                return Result.Error(new ArgumentException(
                    $"Binding information [{bindingInformation}] already exists"));
        }

        return Result.Ok;
    }
}
