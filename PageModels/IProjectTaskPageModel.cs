using CommunityToolkit.Mvvm.Input;
using Quickly.Models;

namespace Quickly.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}