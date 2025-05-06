using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace Homework4;

public class HybridViewModel : ObservableObject, IReactiveObject
{
    public void RaisePropertyChanging(PropertyChangingEventArgs args) => OnPropertyChanging(args);
    public void RaisePropertyChanged(PropertyChangedEventArgs args) => OnPropertyChanged(args);
}
