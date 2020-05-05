using System;
using System.Collections.Generic;
using System.Text;

namespace UntisExportService.Core.Settings.Inputs.Substitutions
{
    public interface IHtmlSubstitutionOptions
    {
        bool FixBrokenPTags { get; }

        string DateTimeFormat { get; }

        bool InlcudeAbsentValues { get; }

        string[] EmptyValues { get; }

        
    }
}
