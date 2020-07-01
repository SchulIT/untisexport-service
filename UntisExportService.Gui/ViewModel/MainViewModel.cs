using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using UntisExportService.Core;
using UntisExportService.Gui.Message;

namespace UntisExportService.Gui.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                Set(() => IsBusy, ref isBusy, value);
                TriggerCommand?.RaiseCanExecuteChanged();
                StartWatchersCommand?.RaiseCanExecuteChanged();
                StopWatchersCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool isWatching;

        public bool IsWatching
        {
            get { return isWatching; }
            set
            {
                Set(() => IsWatching, ref isWatching, value);
                TriggerCommand?.RaiseCanExecuteChanged();
                StartWatchersCommand?.RaiseCanExecuteChanged();
                StopWatchersCommand?.RaiseCanExecuteChanged();
            }
        }


        private bool syncExams;

        public bool SyncExams
        {
            get { return syncExams; }
            set
            {
                Set(() => SyncExams, ref syncExams, value);
                TriggerCommand?.RaiseCanExecuteChanged();
                StartWatchersCommand?.RaiseCanExecuteChanged();
                StopWatchersCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool syncRooms;

        public bool SyncRooms
        {
            get { return syncRooms; }
            set
            {
                Set(() => SyncRooms, ref syncRooms, value);
                TriggerCommand?.RaiseCanExecuteChanged();
                StartWatchersCommand?.RaiseCanExecuteChanged();
                StopWatchersCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool syncSubstitutions;

        public bool SyncSubstitutions
        {
            get { return syncSubstitutions; }
            set
            {
                Set(() => SyncSubstitutions, ref syncSubstitutions, value);
                TriggerCommand?.RaiseCanExecuteChanged();
                StartWatchersCommand?.RaiseCanExecuteChanged();
                StopWatchersCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool syncSupervisions;

        public bool SyncSupervisions
        {
            get { return syncSupervisions; }
            set
            {
                Set(() => SyncSupervisions, ref syncSupervisions, value);
                TriggerCommand?.RaiseCanExecuteChanged();
                StartWatchersCommand?.RaiseCanExecuteChanged();
                StopWatchersCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool syncTimetable;

        public bool SyncTimetable
        {
            get { return syncTimetable; }
            set
            {
                Set(() => SyncTimetable, ref syncTimetable, value);
                TriggerCommand?.RaiseCanExecuteChanged();
                StartWatchersCommand?.RaiseCanExecuteChanged();
                StopWatchersCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool syncTuitions;

        public bool SyncTuitions
        {
            get { return syncTuitions; }
            set
            {
                Set(() => SyncTuitions, ref syncTuitions, value);
                TriggerCommand?.RaiseCanExecuteChanged();
                StartWatchersCommand?.RaiseCanExecuteChanged();
                StopWatchersCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand SelectAllCommand { get; private set; }

        public RelayCommand UnselectAllCommand { get; private set; }

        public RelayCommand TriggerCommand { get; private set; }

        public RelayCommand StartWatchersCommand { get; private set; }

        public RelayCommand StopWatchersCommand { get; private set; }

        #endregion

        #region Services

        public IMessenger Messenger { get { return base.MessengerInstance; } }

        private readonly IExportService exportService;

        #endregion

        public MainViewModel(IExportService exportService, IMessenger messenger)
            : base(messenger)
        {
            this.exportService = exportService;

            SelectAllCommand = new RelayCommand(SelectAll);
            UnselectAllCommand = new RelayCommand(UnselectAll);
            TriggerCommand = new RelayCommand(Trigger, CanTrigger);

            StartWatchersCommand = new RelayCommand(StartWatchers, CanStartWatchers);
            StopWatchersCommand = new RelayCommand(StopWatchers, CanStopWatchers);
        }

        private void StartWatchers()
        {
            var types = new List<InputType>();

            if (SyncExams)
            {
                types.Add(InputType.Exams);
            }
            if (SyncRooms)
            {
                types.Add(InputType.Rooms);
            }
            if (SyncSubstitutions)
            {
                types.Add(InputType.Substitutions);
            }
            if (SyncSupervisions)
            {
                types.Add(InputType.Supervisions);
            }
            if (SyncTimetable)
            {
                types.Add(InputType.Timetable);
            }
            if (SyncTuitions)
            {
                types.Add(InputType.Tuitions);
            }

            exportService.Start(types.ToArray());
            IsWatching = true;
        }

        private bool CanStartWatchers()
        {
            return !IsWatching && !IsBusy && (SyncExams || SyncRooms || SyncSubstitutions || SyncSupervisions || SyncTimetable || SyncTuitions);
        }

        private void StopWatchers()
        {
            exportService.Stop();
            IsWatching = false;
        }

        private bool CanStopWatchers()
        {
            return IsWatching;
        }

        private async void Trigger()
        {
            IsBusy = true;

            try
            {
                if(SyncExams)
                {
                    await exportService.TriggerAsync(InputType.Exams);
                }
                if (SyncRooms)
                {
                    await exportService.TriggerAsync(InputType.Rooms);
                }
                if (SyncSubstitutions)
                {
                    await exportService.TriggerAsync(InputType.Substitutions);
                }
                if (SyncSupervisions)
                {
                    await exportService.TriggerAsync(InputType.Supervisions);
                }
                if(SyncTimetable)
                {
                    await exportService.TriggerAsync(InputType.Timetable);
                }
                if (SyncTuitions)
                {
                    await exportService.TriggerAsync(InputType.Tuitions);
                }
            }
            catch (Exception e)
            {
                Messenger.Send(new ErrorDialogMessage { Exception = e, Header = "Fehler", Title = "Fehler beim Import", Text = "Beim Import ist ein Fehler aufgetreten." });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanTrigger()
        {
            return !IsBusy && !IsWatching && (SyncExams || SyncRooms || SyncSubstitutions || SyncSupervisions || SyncTimetable || SyncTuitions);
        }

        private void SelectAll()
        {
            SyncExams = SyncRooms = SyncSubstitutions = SyncSupervisions = SyncTimetable = SyncTuitions = true;
        }

        private void UnselectAll()
        {
            SyncExams = SyncRooms = SyncSubstitutions = SyncSupervisions = SyncTimetable = SyncTuitions = false;
        }
    }
}
