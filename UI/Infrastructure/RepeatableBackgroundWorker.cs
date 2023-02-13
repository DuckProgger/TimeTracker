using System;
using System.ComponentModel;
using Timer = System.Threading.Timer;

namespace UI.Infrastructure;

internal class RepeatableBackgroundWorker : BackgroundWorker
{
    private readonly TimeSpan dueTime;
    private readonly TimeSpan period;
    private Timer? timer;

    public RepeatableBackgroundWorker(TimeSpan dueTime, TimeSpan period)
    {
        this.dueTime = dueTime;
        this.period = period;
    }

    public new void RunWorkerAsync()
    {
        timer = new Timer(_ =>
        {
            if(!IsBusy)
                base.RunWorkerAsync();
        }, null, dueTime, period);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        timer?.Dispose();
    }
}