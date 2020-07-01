SchILD NRW-Integration
======================

In Nordrhein-Westfalen wird größenteils SchILD NRW zur Verwaltung der Stammdaten einer Schule genutzt.
Dies schließt die Stammdaten zu Lerngruppen und Unterrichten ein. Da diese beim Export in das ICC benötigt
werden, wenn die Stammdaten mithilfe des `SchILD ICC Importers <https://github.com/schulit/schild-icc-importer>`_
importiert wurden.

Konfiguration
#############

Unter dem Schlüssel ``external`` muss dazu folgendes JSON-Objekt eingefügt werden:

.. code-block:: json

    {
      "type": "schild",
      "connection_string": "Server=SERVER\\INSTANZ;Database=DATENBANK;Integrated Security=True",
      "grades_with_coursename_as_subject": [
        "EF",
        "Q1",
        "Q2"
      ],
      "subject_conversation_rules": [ ]
    }

Erklärungen:

- ``connection_string`` enthält die Verbindungszeichenfolge zum Datenbankserver von SchILD. **Wichtig:** doppelte Backslashes verwenden!
- ``grades_with_coursename_as_subject`` enthält eine Liste von Klassen, bei denen die Kursbezeichnung in Untis als Fach geführt wird (dies ist in der Oberstufe häufig der Fall).
- ``subject_conversation_rules`` enthält alle Fächerumbenennungen.

Fächerumbenennungen
###################

Manchmal kommt es vor, dass Fächer in Untis und SchILD anders benannt sind. Man kann jedoch 
Übersetzungsregeln angeben, damit das Tool die Unterrichte dennoch findet. Mehrere Übersetzungsregeln
lassen sich mit Komma separiert angeben.

Beispiel:

.. code-block:: json

    {
        "untis_subject": "AGMedien",
        "schild_subject": "Medien-AG",
        "is_course": true,
        "grades": [
            "05*",
            "06*",
            "07*",
            "08*",
            "09*",
            "EF",
            "Q1",
            "Q2"
        ]
    },
    {
        "untis_subject": "IF-x",
        "schild_subject": "Informatikzusatzkurs",
        "is_course": true,
        "grades": [
            "05*",
            "06*"
        ]
    }

Erklärungen:

- ``untis_subject`` gibt an, wie das Fach es in Untis benannt ist.
- ``schild_subject`` gibt an, wie das Fach in SchILD benannt ist.
- ``is_course`` gibt an, ob das Fach als Kurs- oder Klassenunterricht in SchILD geführt wird (``true``) oder nicht (``false``).
- ``grades`` gibt an, in welchen Klassen das Fach unterrichtet wird. Das ``*`` ist als Platzhalter zulässig.