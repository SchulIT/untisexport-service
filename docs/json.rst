JSON Export
===========

Das Tool unterstützt den Datenexport als JSON.

Konfiguration
#############

Unter ``outputs`` muss folgendes JSON in die eckigen Klammern eingetragen werden:

.. code-block:: json

    {
        "type": "file",
        "entities": [
            "infotext",
            "absence",
            "supervision",
            "timetable",
            "room",
            "substitution",
            "exam"
        ],
        "path": "C:\\Untis\\Export\\JSON"
    }

Erklärungen:

- ``entities`` gibt an, welche Daten exportiert werden sollen. Eine mögliche Auswahl ist im Beispiel angegeben.
- ``path`` gibt den Pfad an, in denen die Dateien erstellt werden sollen. **Wichtig:** doppelte Backslashes verwenden!
