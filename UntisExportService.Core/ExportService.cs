using Autofac.Features.Indexed;
using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using UntisExportService.Core.Inputs;
using UntisExportService.Core.Inputs.Exams;
using UntisExportService.Core.Inputs.Rooms;
using UntisExportService.Core.Inputs.Substitutions;
using UntisExportService.Core.Inputs.Supervisions;
using UntisExportService.Core.Inputs.Timetable;
using UntisExportService.Core.Inputs.Tuitions;
using UntisExportService.Core.Outputs;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Settings.Inputs.Exams;
using UntisExportService.Core.Settings.Inputs.Rooms;
using UntisExportService.Core.Settings.Inputs.Substitutions;
using UntisExportService.Core.Settings.Inputs.Supervisions;
using UntisExportService.Core.Settings.Inputs.Timetable;
using UntisExportService.Core.Settings.Inputs.Tuitions;
using UntisExportService.Core.Settings.Outputs;

namespace UntisExportService.Core
{
    public class ExportService : IExportService
    {
        private readonly IWatcher<IExamInput> examWatcher;
        private readonly IWatcher<IRoomInput> roomWatcher;
        private readonly IWatcher<ISubstitutionInput> substitutionWatcher;
        private readonly IWatcher<ISupervisionInput> supervisionWatcher;
        private readonly IWatcher<ITimetableInput> timetableWatcher;
        private readonly IWatcher<ITuitionInput> tuitionWatcher;

        private readonly IIndex<string, IOutputHandler> outputHandlers;

        private readonly IEventBus eventBus;
        private readonly ISettingsService settingsService;
        private readonly ILogger<ExportService> logger;

        public ExportService(IEventBus eventBus, ISettingsService settingsService, IWatcher<IExamInput> examWatcher, IWatcher<IRoomInput> roomWatcher, 
            IWatcher<ISubstitutionInput> substitutionWatcher, IWatcher<ISupervisionInput> supervisionWatcher, 
            IWatcher<ITimetableInput> timetableWatcher, IWatcher<ITuitionInput> tuitionWatcher, IIndex<string, IOutputHandler> outputHandlers, ILogger<ExportService> logger)
        {
            this.eventBus = eventBus;
            this.settingsService = settingsService;

            this.examWatcher = examWatcher;
            this.roomWatcher = roomWatcher;
            this.substitutionWatcher = substitutionWatcher;
            this.supervisionWatcher = supervisionWatcher;
            this.timetableWatcher = timetableWatcher;
            this.tuitionWatcher = tuitionWatcher;
            this.outputHandlers = outputHandlers;

            this.logger = logger;
        }

        public void Stop()
        {
            examWatcher.Stop();
            roomWatcher.Stop();
            substitutionWatcher.Stop();
            supervisionWatcher.Stop();
            timetableWatcher.Stop();
            tuitionWatcher.Stop();

            logger.LogInformation("Export service stopped.");
        }

        public void Start()
        {
            Start(InputType.Exams, InputType.Rooms, InputType.Substitutions, InputType.Supervisions, InputType.Timetable, InputType.Tuitions);
        }

        public void Start(params InputType[] inputs)
        {
            var settings = settingsService.Settings;

            logger.LogDebug("Start watchers...");

            if (inputs.Contains(InputType.Exams))
            {
                examWatcher.Start();
            }

            if (inputs.Contains(InputType.Rooms))
            {
                roomWatcher.Start();
            }

            if (inputs.Contains(InputType.Substitutions))
            {
                substitutionWatcher.Start();
            }

            if (inputs.Contains(InputType.Supervisions))
            {
                supervisionWatcher.Start();
            }

            if (inputs.Contains(InputType.Timetable))
            {
                timetableWatcher.Start();
            }

            if (inputs.Contains(InputType.Tuitions))
            {
                tuitionWatcher.Start();
            }

            logger.LogInformation("Export service started.");
        }

        public void Configure()
        {
            logger.LogDebug("Configuring...");
            var settings = settingsService.Settings;

            examWatcher.SyncThresholdInSeconds = settings.SyncThresholdInSeconds;
            examWatcher.Configure(settings.Inputs.Exams);

            roomWatcher.SyncThresholdInSeconds = settings.SyncThresholdInSeconds;
            roomWatcher.Configure(settings.Inputs.Rooms);

            substitutionWatcher.SyncThresholdInSeconds = settings.SyncThresholdInSeconds;
            substitutionWatcher.Configure(settings.Inputs.Substitutions);

            supervisionWatcher.SyncThresholdInSeconds = settings.SyncThresholdInSeconds;
            supervisionWatcher.Configure(settings.Inputs.Supervisions);

            timetableWatcher.SyncThresholdInSeconds = settings.SyncThresholdInSeconds;
            timetableWatcher.Configure(settings.Inputs.Timetable);

            tuitionWatcher.SyncThresholdInSeconds = settings.SyncThresholdInSeconds;
            tuitionWatcher.Configure(settings.Inputs.Tuitions);
            logger.LogDebug("Configuration completed.");

            logger.LogDebug("Register events...");
            RegisterEvents(settings);
            logger.LogDebug("Events regsistration completed.");
        }

        public Task TriggerAsync(InputType type)
        {
            switch(type)
            {
                case InputType.Exams:
                    return examWatcher.TriggerAsync();

                case InputType.Rooms:
                    return roomWatcher.TriggerAsync();

                case InputType.Substitutions:
                    return substitutionWatcher.TriggerAsync();

                case InputType.Supervisions:
                    return supervisionWatcher.TriggerAsync();

                case InputType.Timetable:
                    return timetableWatcher.TriggerAsync();

                case InputType.Tuitions:
                    return tuitionWatcher.TriggerAsync();
            }

            return Task.CompletedTask;
        }

        private void RegisterEvents(ISettings settings)
        {
            eventBus.Subscribe<EventBase>(HandleAllEvents);

            /*eventBus.Subscribe<ExamEvent>(HandleExamEvent);
            eventBus.Subscribe<RoomEvent>(HandleRoomEvent);
            eventBus.Subscribe<SubstitutionEvent>(HandleSubstitutionEvent);
            eventBus.Subscribe<AbsenceEvent>(HandleAbsenceEvent);
            eventBus.Subscribe<InfotextEvent>(HandleInfotextEvent);
            eventBus.Subscribe<SupervisionEvent>(HandleSupervisionEvent);
            eventBus.Subscribe<TimetableEvent>(HandleTimetableEvent);
            eventBus.Subscribe<TuitionEvent>(HandleTuitionEvent);*/
        }

        private void HandleAllEvents(EventBase obj)
        {
            if (obj is ExamEvent)
            {
                HandleExamEvent(obj as ExamEvent);
            }
            else if (obj is RoomEvent)
            {
                HandleRoomEvent(obj as RoomEvent);
            }
            else if (obj is SubstitutionEvent)
            {
                HandleSubstitutionEvent(obj as SubstitutionEvent);
            }
            else if (obj is AbsenceEvent)
            {
                HandleAbsenceEvent(obj as AbsenceEvent);
            }
            else if (obj is InfotextEvent)
            {
                HandleInfotextEvent(obj as InfotextEvent);
            }
            else if (obj is SupervisionEvent)
            {
                HandleSupervisionEvent(obj as SupervisionEvent);
            }
            else if (obj is TimetableEvent)
            {
                HandleTimetableEvent(obj as TimetableEvent);
            }
            else if (obj is TuitionEvent)
            {
                HandleTuitionEvent(obj as TuitionEvent);
            }
            else if (obj is FreeLessonEvent)
            {
                HandleFreeLessonEvent(obj as FreeLessonEvent);
            }
        }

        private void Handle(string entity, Action<IOutputHandler, IOutput> handle)
        {
            foreach (var output in settingsService.Settings.Outputs)
            {
                if (output.Entities.Contains(entity))
                {
                    if (outputHandlers.TryGetValue(output.Type, out var handler))
                    {
                        if (handler.CanHandleInfotexts)
                        {
                            logger.LogDebug($"Using output handler of type {output.Type}.");
                            handle(handler, output);
                        }
                        else
                        {
                            logger.LogError($"Output handler {output.Type} does not support entity '{entity}'.");
                        }
                    }
                    else
                    {
                        logger.LogError($"Output handler {output.Type} not supported.");
                    }
                }
            }
        }

        private void HandleInfotextEvent(InfotextEvent obj)
        {
            Handle("infotext", (handler, settings) => handler.HandleInfotextEvent(obj, settings));
        }

        private void HandleAbsenceEvent(AbsenceEvent obj)
        {
            Handle("absence", (handler, settings) => handler.HandleAbsenceEvent(obj, settings));
        }

        private void HandleSupervisionEvent(SupervisionEvent obj)
        {
            Handle("supervision", (handler, settings) => handler.HandleSupervisionEvent(obj, settings));
        }

        private void HandleTimetableEvent(TimetableEvent obj)
        {
            Handle("timetable", (handler, settings) => handler.HandleTimetableEvent(obj, settings));
        }

        private void HandleTuitionEvent(TuitionEvent obj)
        {
            Handle("tuition", (handler, settings) => handler.HandleTuitionEvent(obj, settings));
        }

        private void HandleRoomEvent(RoomEvent obj)
        {
            Handle("room", (handler, settings) => handler.HandleRoomEvent(obj, settings));
        }

        private void HandleSubstitutionEvent(SubstitutionEvent obj)
        {
            Handle("substitution", (handler, settings) => handler.HandleSubstitutionEvent(obj, settings));
        }

        private void HandleExamEvent(ExamEvent obj)
        {
            Handle("exam", (handler, settings) => handler.HandleExamEvent(obj, settings));
        }

        private void HandleFreeLessonEvent(FreeLessonEvent obj)
        {
            Handle("free_lesson", (handler, settings) => handler.HandleFreeLessonEvent(obj, settings));
        }
    }
}
