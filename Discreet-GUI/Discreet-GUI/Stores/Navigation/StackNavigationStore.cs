using System.Collections.Generic;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Stores.Navigation
{
    /// <summary>
    /// This store holds previous viewmodels, used for layouts to implement a back button, without knowing what the previous viewmodel is
    /// </summary>
    public class StackNavigationStore
    {
        private Stack<ViewModelBase> _previousViewModels = new Stack<ViewModelBase>();

        public void PushViewModelBase(ViewModelBase viewModelBase) => _previousViewModels.Push(viewModelBase);
        public ViewModelBase GetPreviousViewModelBase() => _previousViewModels.Pop();
    }
}
