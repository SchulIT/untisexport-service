Klausurschreiber aus SchILD NRW ermitteln
=========================================

Das Auflösen der Klausurschreiber aus SchILD hat den Vorteil,
dass stets die aktuell dort hinterlegten Daten verwendet werden.
Damit die Auflösung funktioniert, muss SchILD als externer
Dienst konfiguriert sein.

Konfiguration
#############

Unter ``exam_writers_resolver`` muss ``null`` durch folgendes JSON ersetzt werden:

.. code-block:: json

    {
        "type": "schild",
        "sections": [
            {
                "year": 2019,
                "section": 1,
                "start": "2019-08-20T00:00:00Z",
                "end": "2020-01-31T00:00:00Z"
            },
            {
                "year": 2019,
                "section": 2,
                "start": "2020-02-01T00:00:00Z",
                "end": "2020-06-30T00:00:00Z"
            }
        ],
        "rules": [
            {
                "grades": [
                    "EF",
                    "Q1"
                ],
                "sections": [
                    1,
                    2
                ],
                "types": [
                    "GKS",
                    "LK1",
                    "LK2",
                    "AB3"
                ]
            },
            {
                "grades": [ "Q2" ],
                "sections": [ 1 ],
                "types": [
                    "GKS",
                    "LK1",
                    "LK2",
                    "AB3"
                ]
            },
            {
                "grades": [ "Q2" ],
                "sections": [ 2 ],
                "types": [
                    "LK1",
                    "LK2",
                    "AB3"
                ]
            }
        ]
    }

Erklärungen:

- unter ``sections`` werden alle Abschnitte definiert, die SchILD kennt. Das können die beiden Schulhalbjahre oder vier Perioden sein.
- unter ``rules`` werden Regeln angegeben, welche einen Schüler oder eine Schülerin als Klausurschreiber definiert

Regel-Defintion
***************

Eine Regel besteht aus den Informationen Klassen/Jgst. (``grades``), 
den Abschnitten (``sections``) und den Kursbelegarten (``types``).

Beispiel 1:

.. code-block:: json

    {
        "grades": [
            "EF",
            "Q1"
        ],
        "sections": [
            1,
            2
        ],
        "types": [
            "GKS",
            "LK1",
            "LK2",
            "AB3"
        ]
    }

Erklärung: Kursteilnehmerinnen und -teilnehmer in EF und Q1, die in den Abschnitten 1 und 2
die Kursart GKS, LK1, LK2 oder AB3 belegen, gelten als Klausurschreiber.
In diesem Fall wäre Abschnitt gleichgesetzt mit Schulhalbjahr (man beachte die Angaben unter ``sections``).

Beispiel 2:

.. code-block:: json

    {
        "grades": [ "Q2" ],
        "sections": [ 2 ],
        "types": [
            "LK1",
            "LK2",
            "AB3"
        ]
    }

Erklärung: Kursteilnehmerinnen und -teilnehmer in der Q2, die im Abschnitt 2 die Kursart
LK1, LK2 oder AB3 belegen, gelten als Klausurschreiber.

