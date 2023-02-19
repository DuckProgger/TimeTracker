using System;
using System.Collections.ObjectModel;
using System.Linq;
using Entity;

namespace UI.Model;

internal class WorkdayModel : ModelBase
{
    private ObservableCollection<WorkModel> works = new();

    public ObservableCollection<WorkModel> Works
    {
        get => works;
        set
        {
            works = value;
            OnPropertyChanged(nameof(TotalWorkload));
        }
    }

    public TimeSpan TotalWorkload { get; set; }

    public static WorkdayModel Map(Workday workday)
    {
        return new WorkdayModel()
        {
            TotalWorkload = workday.TotalWorkload,
            Works = new ObservableCollection<WorkModel>(workday.Works.Select(WorkModel.Map))
        };
    }
}