using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feur_MaColoc_Metier
{
    class ControlViewModelScheduler
    {

        public ObservableCollection<SchedulerAppointment> SchedulerEvents { get; set; }
        public ControlViewModelScheduler()
        {
            this.SchedulerEvents = new ObservableCollection<SchedulerAppointment>
            {
                new SchedulerAppointment
                {
                    StartTime = new DateTime(2023, 10, 05, 17, 0, 0),
                    EndTime = new DateTime(2023, 10, 05, 19, 0, 0),
                    Subject = "Test",
                }
            };
        }
    }
}
