# UntisExportService

[![Build Status](https://dev.azure.com/schulit/UntisExportService/_apis/build/status/SchulIT.untisexport-service?branchName=master)](https://dev.azure.com/schulit/UntisExportService/_build/latest?definitionId=3&branchName=master)

This service utilises the [UntisExport library](https://github.com/SchulIT/untisexport) to parse new substitutions and infotexts in order to push them to the [ICC](SchulIT/portal). The service watches a directory in which the HTML output is saved to enable pushing substitutions and infotexts to the ICC automatically.

## Installation

Use the MSI from the releases tab to install the latest version of the service. Upgrading using the MSI is supported as well.

## Configuration

Currently, configuration is only supported using the `settings.json` file which can be found inside the `C:\ProgramData\SchulIT\UntisExportService` directory. Note: the configuration file is generated after the first startup. 

### Default configuration

```json
{
  "debug": false,
  "enabled": true,
  "html_path": null,
  "endpoint": {
    "substitutions": null,
    "infotexts": null,
    "api_key": null,
    "new_version": true
  },
  "untis": {
    "remove_exams": false,
    "fix_ptags": true,
    "datetime_format": "d.M.yyyy",
    "empty_values": [
      "---",
      "???"
    ],
    "include_absentvalues": false,
    "columns": {
      "id": "Vtr-Nr.",
      "date": "Datum",
      "lesson": "Stunde",
      "grades": "(Klasse(n))",
      "replacement_grades": "Klasse(n)",
      "teachers": "(Lehrer)",
      "replacement_teachers": "Vertreter",
      "subject": "(Fach)",
      "replacement_subject": "Fach",
      "room": "(Raum)",
      "replacement_room": "Raum",
      "type": "Art",
      "remark": "Vertretungs-Text"
    }
  },
  "study_groups_file": "C:\\ProgramData\\SchulIT\\UntisExportService\\your-study-groups.json"
}
```

Explanations:

* if `debug` is set to `true`, all responses are logged (inside the `log` directory in the app directory)
* if `enabled` is set to `false`, the service will ignore file system changes (thus it won't upload any data)
* `html_path` specifies the output directory of the Untis HTML files
* the `endpoint` section defines where to upload the substitutions/infotexts to:
  * `substitutions` specifies the full URL to the API endpoint to push substitutions to
  * `infotexts` specifies the full URL to the API endpoint to push infotexts to
  * `api_key` specifies the API key which is added to the request in order to authorise the client against the ICC
  * `new_version` should be set to `true`. It is a legacy settings which will be removed in further versions as it is only used for our proprietery version of ICC. 
* the `untis` sections defines settings for the Untis library
  * if `remove_exams` is set to true, all exams (substitutions with ID `0` are concidered exams) are removed before upload. **Important:** This options should be set to `true` if substitutions are pushed to ICC as the ICC expects IDs to be unique.
  * `fix_ptags` should be set to `true` to make the HTML output valid before parsing
  * `datetime_format` is used to parse the date from the Untis HTML output (internally, `DateTime.Parse` is used), see [Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings)
  * all strings in `empty_values` are used to concider a value (e.g. the room) empty making its value `null`
  * if `include_absentvalues` is set to `false`, values inside brakets (such as `(05A)`) are concidered empty and thus making its value `null`. If set to `true`, the values are parsed without brakets
  * the `columns` settings hold the information to determine the corresponding columns
* `your-study-groups.json` has has the following format: In the `study_groups` property you need to specify the grades as keys. Inside this dictionary, the keys are the subjects (which come from the Untis program) and its value is basically and study group (identifier):

```json
{
  "study_groups": {
    "05A": {
      "M": "M-05A"
    },
    "08A": {
      "IF": "IF-8ac"
    },
    "08B": {
      "IF": "IF-8ac"
    },
    "08C": {
      "IF": "IF-8ac"
    }
  }
}
```

**Note:** You can change the configuration during runtime. The service watches for changes of the `settings.json` file and applies new settings immediately.

## Tools

### Console

The console is used to run the service in a command window. Even though the Console application is written in .NET Core, the installer currently supports Windows environments only.

### Service

This is the actual Windows Service which can be used to synchronise substitutions and infotexts in the background.

**Note:** By default, the service does not start automatically on Windows startup. You need to set the startup action manually.

### Configuration utilty (coming soon)

In the future, there will be a configuration utility which can be used to configure the settings mentioned above.

## License

[MIT](license.md)
