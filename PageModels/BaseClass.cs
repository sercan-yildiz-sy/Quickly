using CommunityToolkit.Mvvm.Input;
using Quickly.Models;

namespace Quickly.PageModels
{
    /// <summary>
    /// Defines a base interface for view models that support busy state tracking
    /// </summary>
    public interface IBaseClass
    {
        // Gets a value indicating whether the view model is currently busy
        bool IsBusy { get; }
    }
}