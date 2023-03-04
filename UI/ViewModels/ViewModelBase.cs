using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
using UI.Infrastructure;
using UI.Validation;

namespace UI.ViewModels;

internal abstract class ViewModelBase : INotifyPropertyChanged, INavigationAware, IDataErrorInfo
{
    protected readonly IEventAggregator eventAggregator;
    private Validator? validator;

    protected ViewModelBase() { }

    protected ViewModelBase(IEventAggregator eventAggregator) {
        this.eventAggregator = eventAggregator;
    }

    public Notifier Notifier { get; set; } = new();

    public string Title { get; set; } = string.Empty;

    public bool ValidationSuccess => Validate();

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Добавить валидацию для свойства.
    /// </summary>
    /// <param name="nameExpression">Выражение, указывающее на свойство.</param>
    /// <param name="validationRule">Добавляемое правило проверки.</param>
    protected void AddValidator(
        Expression<Func<object>> nameExpression,
        ValidationRule validationRule)
    {
        validator ??= new Validator();
        validator.Add(nameExpression, validationRule);
    }

    /// <summary>
    /// Произвести все валидации.
    /// </summary>
    /// <returns>true, если проверка прошла успешно; иначе false.</returns>
    protected bool Validate()
    {
        return validator?.ValidateAll() ?? true;
    }

    public void OnPropertyChanged([CallerMemberName] string prop = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        if(validator?.HasPropertyName(prop) ?? false)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationSuccess)));
    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext) { }

    public virtual bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

    public string Error { get; }

    public string this[string columnName] => validator?.Validate(columnName) ?? string.Empty;
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