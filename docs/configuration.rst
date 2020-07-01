Konfiguration
=============

Konfigurationsdatei anlegen
---------------------------

Die Anwendung muss leider händisch konfiguriert werden. Sie befindet sich unter ``C:\ProgramData\SchulIT\UntisExportService\settings.json``.

.. warning:: Bitte die Verzeichnisstruktur ggf. anlegen. Die Datei sollte in UTF-8 enkodiert sein.

Zunächst die folgende Vorlage für die JSON-Datei anlegen:

.. code-block:: json

    {
        "threshold": 5,
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
        "tuition_resolver": { },
        "exam_writers_resolver": null
    }

Der Wert ``threshold`` gibt die Anzahl an Sekunden an, die das Tool nach der letzten Dateiänderung warten soll, bis
der Importvorgang gestartet wird.

Unter ``inputs`` werden alle Untis-Bereiche konfiguriert.

Unter ``outputs`` werden alle Ausgaben (JSON oder ICC) konfiguriert.

Unter ``external`` werden externe Dienste konfiguriert (bspw. SchILD NRW), falls diese benötigt werden.

Unter ``tuition_resolver`` wird konfiguriert, wie Unterrichte aufgelöst werden.

Unter ``exam_writers_resolver`` wird konfiguriert, wie Klausurschreiber ermittelt werden.

Untis konfigurieren
-------------------

Vertretungen
############

GPU-Format
**********

Möchte/Muss man mit der ``GPU014.txt`` von Untis arbeiten, so fügt man in der angelegten
Konfigurationsdatei unter ``substitutions`` folgendes JSON ein und ersetzt damit das ``null``:

.. code-block:: json

    {
        "type": "gpu",
        "path": "C:\\Untis\\Export\\Substitutions",
        "delimiter": ",",
        "encoding": "utf-8"
    }

Erklärungen:

- ``path`` gibt den Ordner an, in den die GPU-Datei exportiert wird. **Wichtig:** doppelte Backslashes verwenden!
- ``delimiter`` gibt das Trennzeichen an, mit dem Untis die GPU-Datei exportiert.
- ``encoding`` gibt den Zeichensatz an, mit dem Untis die GPU-Datei exportiert.

.. warning:: Das GPU-Format exportiert keine Tagestexte oder Absenzen.

HTML-Format
***********

Das HTML-Format hat gegenüber dem GPU-Format den Vorteil, dass auch Tagestexte und Absenzen
exportiert werden können. Das HTML-Format setzt jedoch das Modul Infostundenplan voraus.

Möchte man das HTML-Format nutzen, so fügt man in der angelegten
Konfigurationsdatei unter ``substitutions`` folgendes JSON ein und ersetzt damit das ``null``:

.. code-block:: json

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

Erklärungen:

- ``path`` gibt den Ordner an, in den die HTML-Dateien exportiert werden. **Wichtig:** doppelte Backslashes verwenden!
- ``encoding`` gibt den Zeichensatz an, mit dem Untis die HTML-Dateien exportiert. Dies ist standardmäßig ``iso-8859-1``.
- ``type_replacements`` gibt eine Ersetzungstabelle an, um Vertretungsarten umzubenennen. Im folgenden Beispiel werden Vertretungsarten auf der linken Seite durch die rechte Seite ersetzt.

.. code-block:: json

    {
        "Sondereins.": "Sondereinsatz",
        "Raumvtr.": "Raumvertretung"
    }

- ``remove_types`` gibt Vertretungsarten an, bei denen eine Vertretung ignoriert werden soll. Standardmäßig werden alle Klausur-Vertretungen ignoriert.
- ``options`` spezifiziert weitere Optionen für den Import angegeben. Interessant ist die Option ``include_absentvalues``, welche festlegt, ob Absenzen als Infotext ausgegeben wird (``true``) oder nicht (``false``).
- ``columns`` gibt die Spaltenköpfe an, welche Untis im HTML-Export verwendet.

Klausuren
#########

GPU-Format
**********

Möchte/Muss man mit der ``GPU017.txt`` von Untis arbeiten, so fügt man in der angelegten
Konfigurationsdatei unter ``exams`` folgendes JSON ein und ersetzt damit das ``null``:

.. code-block:: json

    {
        "type": "gpu",
        "path": "C:\\Untis\\Export\\Exams",
        "delimiter": ",",
        "encoding": "utf-8"
    }

Erklärungen:

- ``path`` gibt den Ordner an, in den die GPU-Datei exportiert wird. **Wichtig:** doppelte Backslashes verwenden!
- ``delimiter`` gibt das Trennzeichen an, mit dem Untis die GPU-Datei exportiert.
- ``encoding`` gibt den Zeichensatz an, mit dem Untis die GPU-Datei exportiert.

HTML-Format
***********

Das HTML-Format hat gegenüber dem GPU-Format den Vorteil, dass die Jahrgangsstufen exportiert werden und man nicht
auf die GPU002.txt angewiesen ist (welche alte Unterrichte leider nicht exportiert).

Möchte man das HTML-Format nutzen, so fügt man in der angelegten
Konfigurationsdatei unter ``exams`` folgendes JSON ein und ersetzt damit das ``null``:

.. code-block:: json

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

Erklärungen:

- ``path`` gibt den Ordner an, in den die HTML-Dateien exportiert werden. **Wichtig:** doppelte Backslashes verwenden!
- ``encoding`` gibt den Zeichensatz an, mit dem Untis die HTML-Dateien exportiert. Dies ist standardmäßig ``iso-8859-1``.
- ``datetime_format`` gibt das Format an, in dem die Datumsangaben im Export gemacht werden.
- ``columns`` gibt die Spaltenköpfe an, welche Untis im HTML-Export verwendet.

Räume
-----

Räume können aus der ``GPU005.txt`` exportiert werden. Dazu das folgende JSON unter ``rooms`` einfügen
und somit das ``null`` ersetzen:

.. code-block:: json

    {
        "path": "C:\\Untis\\Export\\Rooms",
        "delimiter": ",",
        "encoding": "utf-8"
    }

Erklärungen:

- ``path`` gibt den Ordner an, in den die GPU-Datei exportiert wird. **Wichtig:** doppelte Backslashes verwenden!
- ``delimiter`` gibt das Trennzeichen an, mit dem Untis die GPU-Datei exportiert.
- ``encoding`` gibt den Zeichensatz an, mit dem Untis die GPU-Datei exportiert.

Unterrichte
-----------

Unterrichte können zwar eingelesen, aber aktuell nicht ins ICC exportiert werden. Sie werden
benötigt, wenn man Klausuren mit der ``GPU017.txt`` exportiert werden. Nur so können Jahrgangsstufen 
den Klausuren zugeordnet werden.

Um die GPU-Datei zu verwenden, muss unter ``tuitions`` das ``null`` durch folgendes JSON ersetzt werden:

.. code-block:: json

    {
        "path": "C:\\Untis\\Export\\Tuitions",
        "delimiter": ",",
        "encoding": "utf-8"
    }

- ``path`` gibt den Ordner an, in den die GPU-Datei exportiert wird. **Wichtig:** doppelte Backslashes verwenden!
- ``delimiter`` gibt das Trennzeichen an, mit dem Untis die GPU-Datei exportiert.
- ``encoding`` gibt den Zeichensatz an, mit dem Untis die GPU-Datei exportiert.

.. warning:: Unterrichte in der ``GPU002.txt`` sind leider nur jene Unterrichte, die der aktuell ausgewählten Periode zugeordnet sind. Somit können alte Klausuren nicht wieder importiert werden.

Stundenplan
-----------

Der Stundenplan kann lediglich über den HTML-Export eingelesen werden. Dazu unter ``timetable`` das ``null``
durch folgendes JSON ersetzen:

.. code-block:: json

    {
        "path": "C:\\Users\\Marcel\\Desktop\\Untis\\Stundenplan",
        "encoding": "iso-8859-1",
        "first_lesson": 1,
        "use_weeks": true,
        "grades": [
            "05*",
            "06*",
            "07*",
            "08*",
            "09*",
            "EF",
            "Q1",
            "Q2"
        ],
        "subjects": [
            "Bereit"
        ],
        "only_last_period": true
    }

Erklärungen:

- ``path`` gibt den Ordner an, in den die HTML-Dateien exportiert werden. **Wichtig:** doppelte Backslashes verwenden!
- ``encoding`` gibt den Zeichensatz an, mit dem Untis die HTML-Dateien exportiert. Dies ist standardmäßig ``iso-8859-1``.
- ``first_lesson`` gibt die erste Stunde an (ist in der Regel Stunde Nr. 1)
- ``use_weeks`` gibt an, ob man einen periodischen Stundenplan hat (``true``) oder nicht (``false``).
- ``grades`` gibt alle Klassen an, die exportiert werden. Dabei ist das ``*`` als Platzhalter erlaubt.
- ``subjects`` gibt alle Fächer an, die exportiert werden. Dies ist bei Stundenplaneinträgen wichtig, die keiner Klasse und keinem Unterricht zugeordnet werden (bspw. Bereitschaften).
- ``only_last_period`` gibt an, ob nur die letzte Periode exportiert werden soll (``true``) oder nicht (``false``).

