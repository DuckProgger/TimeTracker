using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using UI.Infrastructure;

namespace UI.ViewModel;

internal abstract class ViewModelBase : INotifyPropertyChanged, INavigationAware
{
    protected readonly IEventAggregator eventAggregator;

    protected ViewModelBase() { }

    protected ViewModelBase(IEventAggregator eventAggregator) {
        this.eventAggregator = eventAggregator;
    }

    public Notifier Notifier { get; set; } = new();

    public string Title { get; set; } = string.Empty;


    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext) { }

    public virtual bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }
}

internal class ViewModelBase<T> : ViewModelBase
{
    protected ViewModelBase(IEventAggregator eventAggregator) : base(eventAggregator) {
        collectionView = (CollectionView)CollectionViewSource.GetDefaultView(Collection);
    }
    public ObservableCollection<T> Collection { get; set; } = new();

    private readonly ICollectionView collectionView;

    protected void SetFilter(Predicate<T> predicate) {
        collectionView.Filter = obj => predicate((T)obj);
    }

    protected void RefreshFilter() {
        collectionView.Refresh();
    }

    protected ViewModelBase() : this(null) {
    }
}