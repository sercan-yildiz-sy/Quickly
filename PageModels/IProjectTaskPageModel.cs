using CommunityToolkit.Mvvm.Input;
using Quicky.Models;

namespace Quicky.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}