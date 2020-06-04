# Configuration

To configure the service or console, a `settings.json` file needs to be created.

## Basic template

```json
{
    "debug": false,
    "threshold": 2,
    "inputs": {
        "substitutions": null,
        "exams": null,
        "rooms": null,
        "supervisions": null,
        "timetable": null,
        "tuitions": null
    },
    "outputs": [ ],
    "external": [ ],
    "tuition_resolver": { }
}
```

* if `debug` is `true` then log level will be set to DEBUG
* `threshold` is the number of seconds the watcher will wait for Untis to create all export files before triggering upload

## Inputs

### Substitutions

Substitutions can be imported from either a GPU file or using HTML files. Replace `null` with the corresponding configurion which is explained below.

#### GPU

```json
{
    "type": "gpu",
    "path": "C:\\Untis\\Export\\Substitutions",
    "delimiter": ",",
    "encoding": "utf-8"
}
```

* `type` must be `gpu` in order to recognize the type of file
* `path` is the directory where the GPU-file is exported to by Untis - use double back-slashes!
* `delimiter` is the delimiter Untis uses (can be specified when exporting)
* `encoding` sets the encoding of the GPU file (can be specified when exporting) - see [this list of supported encodings](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getencodings?view=netcore-3.1)

#### HTML

```json
{
    "type": "html",
    "path": "C:\\Untis\\Export\\Substitutions",
    "encoding": "iso-8859-1",
    "type_replacements": { },
    "remove_types": [ "Klausur" ],
    "options": {
        "fix_ptags": true,
        "datetime_format": "d.M.yyyy",
        "empty_values": [
            "---",
            "???"
        ],
        "include_absentvalues": false
    },
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
}
```

* `type` must be `html` in order to recognize the type of file
* `path` is the directory where the HTML files are exported to by Untis - use double back-slashes!
* `encoding` sets the encoding of the GPU file (can be specified when exporting) - see [this list of supported encodings](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getencodings?view=netcore-3.1)
* consult the `UntisExport` manual for the `options` and `column` settings

### Exams

#### GPU

```json
{
    "type": "gpu",
    "path": "C:\\Untis\\Export\\Exams",
    "delimiter": ",",
    "encoding": "utf-8"
}
```

* `type` must be `gpu` in order to recognize the type of file
* `path` is the directory where the GPU-file is exported to by Untis - use double back-slashes!
* `delimiter` is the delimiter Untis uses (can be specified when exporting)
* `encoding` sets the encoding of the GPU file (can be specified when exporting) - see [this list of supported encodings](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getencodings?view=netcore-3.1)

#### HTML

```json
{
    "type": "html",
    "path": "C:\\Untis\\Export\\Exams",
    "encoding": "iso-8859-1",
    "datetime_format": "d.M.yyyy",
    "columns": {
        "date": "Datum",
        "lesson_start": "Von",
        "lesson_end": "Bis",
        "grades": "Klassen",
        "grades_separator": ",",
        "courses": "Kurs",
        "courses_separator": ",",
        "teachers": "Lehrer",
        "teachers_separator": "-",
        "rooms": "Räume",
        "rooms_separator": "-",
        "name": "Name",
        "remark": "Text"
    }
}
```

### Rooms

Rooms are only supported by the GPU file:

```json
{
    "path": "C:\\Untis\\Export\\Rooms",
    "delimiter": ",",
    "encoding": "utf-8"
}
```

* `path` is the directory where the GPU-file is exported to by Untis - use double back-slashes!
* `delimiter` is the delimiter Untis uses (can be specified when exporting)
* `encoding` sets the encoding of the GPU file (can be specified when exporting) - see [this list of supported encodings](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getencodings?view=netcore-3.1)

### Tuitions

Tuitions are only supported by the GPU file. This input is needed when importing exams using a GPU file:

```json
{
    "path": "C:\\Untis\\Export\\Tuitions",
    "delimiter": ",",
    "encoding": "utf-8"
}
```

* `path` is the directory where the GPU-file is exported to by Untis - use double back-slashes!
* `delimiter` is the delimiter Untis uses (can be specified when exporting)
* `encoding` sets the encoding of the GPU file (can be specified when exporting) - see [this list of supported encodings](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getencodings?view=netcore-3.1)

### Supervisions

Supervisions are only supported by the GPU file:

```json
{
    "path": "C:\\Untis\\Export\\Supervisions",
    "delimiter": ",",
    "encoding": "utf-8"
}
```

* `path` is the directory where the GPU-file is exported to by Untis - use double back-slashes!
* `delimiter` is the delimiter Untis uses (can be specified when exporting)
* `encoding` sets the encoding of the GPU file (can be specified when exporting) - see [this list of supported encodings](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getencodings?view=netcore-3.1)

### Timetable

The timetable can only be imported using HTML files:

```json
{
    "path": "C:\\Untis\\Export\\Timetable",
    "encoding": "iso-8859-1",
    "first_lesson": 1,
    "use_weeks": true
}
```

* `path` is the directory where the GPU-file is exported to by Untis - use double back-slashes!
* `encoding` sets the encoding of the GPU file (can be specified when exporting) - see [this list of supported encodings](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getencodings?view=netcore-3.1)
* `first_lesson` specifies the first lesson set in Untis
* `use_weeks` must be true for timetables with alternating weeks

## Outputs

You may specify as many outputs as you want by adding them to the `outputs` array.

### JSON file

The simplest output type is a JSON file which contains all entities which are imported from Untis.

```json
{
    "type": "file",
    "entities": [ ],
    "path": "C:\\UntisExporter\\Output"
}
```

### ICC

You can also import the entities to the [ICC](https://github.com/schulit/icc):

```json
{
    "type": "icc",
    "entities": [ ],
    "endpoint": {
        "url": "https://icc.schulit.de/",
        "token": "YourSecretToken",
        "response_path": "C:\\UntisExporter\\Responses"
    },
    "timetable_periods": { },
    "week_mapping": {
        "weeks": {
            "1": "A",
            "0": "B"
        },
        "use_week_modulo": true
    }
}
```

* `timetable_periods` contains a dictionary which resolves the period names specified in Untis to the `external ID` property of the corresponding ICC period.
* `week_mapping` contains information about the week mapping. The keys in the `weeks` dictionary are the week numbers and the values are the ICC week keys. If weeks are only computed by the week number, set the option `use_week_modulo` to `true`. Otherwise you need to specify all weeks and their corresponding week type in the ICC. 

## External services

Sometimes you need to use external services such as a school management program which holds information about tuitions and students. In most cases, a library or service is used to retrieve data from those programs. To be able to use this program data in multiple resolving situtations, you need to configure the service in the `external` section. 

### SchILD NRW

In order to use SchILD NRW, you must configure it as follows:

```json
{
    "type": "schild",
    "connection_string": "your_connection_string"
}
```

* `type` must be `schild`
* `connection_string` must be a valid connection string used by the [underlying library](https://github.com/schulit/schildexport) to connect to SchILD

## Tuition Resolver

A tuition resolver is needed in order to match Untis study groups and tuitions with the ones online (e.g. ICC). 

### SchILD NRW

The SchILD NRW resolver is used in case tuitions come from this program.

**Important:** You need a SchILD service in the `external` section (see above).

```json
{
    "type": "schild",
    "grades_with_course_names_as_subject": [ "EF", "Q1", "Q2" ]
}
```

* `type` must be `schild`
* `grades_with_course_names_as_subject` is used to specify grades which use course names as subjects in Untis (e.g. M-LK1 is a subject in Untis, but the actual subject is M)