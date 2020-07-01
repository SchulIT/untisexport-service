ICC Export
==========

Eine zentrale Aufgabe des Tools ist der Datenexport ins ICC. 

Konfiguration
#############

Zur Basiskonfiguration muss das folgende JSON in die eckigen Klammern
von ``outputs`` kopiert werden. Falls dort bereits ein Eintrag notiert
ist, bitte mit einem Komma separieren.

.. code-block:: json

    {
        "type": "icc",
        "entities": [
            "infotext",
            "absence",
            "supervision",
            "timetable",
            "room",
            "substitution",
            "exam"
        ],
        "endpoint": {
            "url": "https://icc.example.com/",
            "token": "abc"
        },
        "timetable_periods": {
            "1. Periode": "periode-1",
            "2. Periode": "periode-2"
        },
        "week_mapping": {
            "weeks": {
                "0": "B",
                "1": "A"
            },
            "use_week_modulo": true
        },
        "supervision_period": "periode-6",
        "name_as_id_pattern": null,
        "no_students_pattern": null,
        "student_subset_pattern": "SG:([A-ZÄÖÜ]*)-([A-ZÄÖÜ]*)"
    }

Erklärungen:

- ``entities`` gibt an, welche Daten exportiert werden sollen. Eine mögliche Auswahl ist im Beispiel angegeben.
- ``endpoint`` konfiguriert den ICC Endpunkt. Das Token ist jenes, welches in der Konfiguration des ICC als ``IMPORT_PSK`` angegeben wurde.
- ``timetable_periods`` übersetzt die Namen der Perioden aus Untis in den Perioden-Schlüssel im ICC.
- ``week_mapping`` übersetzt den die Wochennummer in Wochennamen (wenn man periodische Wochen nutzt).
- ``use_week_modulo`` gibt an, ob bei der Übersetzung der Wochennummern auf der linken Seite der Modulo-Operator genutzt werden soll. Dies erleichtert bei periodischen A-/B-Wochen die Konfiguration.
- ``supervision_period`` gibt den Periodenschlüssel aus dem ICC an, für den die Aufsichten importiert werden sollen. **Wichtig:** Dieser Schlüssel muss angepasst werden, sobald eine neue Periode ins ICC hochgeladen werden soll.
- ``name_as_id_pattern`` gibt einen regulären Ausdruck an, mit dessen Hilfe der Name der Klausur überprüft wird. Falls der reguläre Ausdruck zutrifft, wird der Klausurname als ID verwendet. Dies ist für Nachschreibeklausuren 
  sinnvoll, bei denen auf dem ICC Nachschreiber hinzugefügt werden können. Damit die Klausur vom ICC wiedererkannt wird, wird dann der Name als ID verwendet. **Wichtig:** Der Name der Klausur darf dann nur einmalig vorkommen.
- ``no_students_pattern`` gibt einen regulären Ausdruck an, mit dessen Hilfe der Name der Klausur überprüft wird. Falls der reguläre Ausdruck zutrifft, wird der Klausur kein Klausurschreiber zugeordnet. Dies kann für Nachschreibeklausuren genutzt werden.
- ``student_subset_pattern`` gibt einen regulären Ausdruck an, der benötigt wird, falls nicht alle Klausurschreiber einer Klausur Teil dieses Klausurtermins sind. Dies ist sinnvoll, wenn
  die Klausurschreiber auf mehrere Räume aufgeteilt werden. Beispiel: Klausurname ``EF-M-GK1-HT1-SG:A-C`` beinhaltet alle Klausurschreiber mit Nachnamen A bis C (einschließlich). **Hinweis:** Die Bereichsangabe darf auch mehrfach vorkommen, bspw. ``EF-M-GK1-HT1-SG:A-C-SG:K-M`` (A bis C und K bis M)
