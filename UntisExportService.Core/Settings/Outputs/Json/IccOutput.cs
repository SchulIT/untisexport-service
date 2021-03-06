﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Outputs.Json
{
    public class IccOutput : OutputBase, IIccOutput
    {
        [JsonProperty("type")]
        public override string Type { get; } = "icc";

        [JsonProperty("endpoint")]
        public IEndpointSettings Endpoint { get; } = new EndpointSettings();

        [JsonProperty("timetable_periods")]
        public Dictionary<string, string> TimetablePeriodMapping { get; set; } = new Dictionary<string, string>();

        [JsonProperty("week_mapping")]
        public IWeekMapping WeekMapping { get; set; } = new WeekMapping();

        [JsonProperty("supervision_period")]
        public string SupervisionPeriod { get; set; } = null;

        [JsonProperty("name_as_id_pattern")]
        public string SetNameAsIdPattern { get; set; } = null;

        [JsonProperty("no_students_pattern")]
        public string SetNoStudentsPattern { get; set; } = null;

        [JsonProperty("student_subset_pattern")]
        public string StudentSubsetPattern { get; set; } = null;
    }
}
