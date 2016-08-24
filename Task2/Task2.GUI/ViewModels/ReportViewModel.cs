using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Task2.Common.Models;

namespace Task2.GUI.ViewModels
{
    public sealed class ReportViewModel : INotifyPropertyChanged
    {

        public ReportViewModel()
        {
            _reports = new List<SpReportModel>();
        }

        private IEnumerable<SpReportModel> _reports;

        public IEnumerable<SpReportModel> Reports
        {
            get { return _reports; }
            set
            {
                _reports = value;

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
