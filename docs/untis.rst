Untis konfigurieren
===================

Damit Untis mit den HTML-Exporten umgehen kann, muss ein bestimmtes Format
importiert werden. 

Schritt 1: Export-Format importieren
####################################

Zunächst im Menü unter "Import/Export" die Kategorie "Untis" auswählen und dort die Schnittstelle "Formate/Fenstergruppen/Ribbon" anklicken.

.. image:: images/untis-import-1.png

Es öffnet sich ein Dialog. Dort in den Tab "Eingabeformat Import" (Vertretungsplan) oder "Stundenplanformat Import" (Stundenplan) wechseln und auf "Durchsuchen" klicken.

.. image:: images/untis-import-2.png

.. image:: images/untis-import-2-stundenplan.png

Es öffnet sich der "Datei öffnen"-Dialog. Dort in den Ordner wechseln, in dem die Formate-Datei liegt und anschließend die Datei auswählen und auf "Öffnen" klicken.

**Wichtig:** Um die Datei sehen zu können, muss unten rechts das "Format Dateien (*.gpf)" ausgewählt werden.

.. image:: images/untis-import-3.png

Abschließend auf "Importieren" klicken. Es öffnet sich nun ein letzter Dialog, welchen man mit "Ok" bestätigen kann.

.. image:: images/untis-import-4.png
.. image:: images/untis-import-4-stundenplan.png

**Fertig!** Dieses Prozedere für alle gewünschten Formate durchführen.

Schritt 2: Export-Format anpassen (Absenzen im Vertretungsplan)
###############################################################

Aktuell gibt es nicht allzu viele Möglichkeiten, den Export anzupassen. Aktuell ist es nur möglich, Absenzen zu exportieren oder nicht. Dazu in der Druck-Ansicht auf das "Seite einrichten"-Icon klicken.

.. image:: images/untis-anpassen-1.png

Im Dialog hat man dann die Möglichkeit, den Absenzkopf anzupassen. Hier kann man die rot markierten Checkboxen aktivieren oder deaktivieren:

.. image:: images/untis-anpassen-2.png

Schritt 3: Untis Info-Stundenplan einrichten (Vertretungen)
###########################################################

Im Ribbon klickt man oben auf Info-Stundenplan. Es öffnet sich der folgende Dialog:

.. image:: images/untis-export-1.png

Dort wählt man die Einstellung "Standard" (alternativ lässt sich auch eine andere nutzen) und klickt auf den Button "..." neben dem Einstellungsnamen.

Im sich öffnenden Dialog müssen nun die folgenden Dinge angepasst werden:

* Das Vertretungsplanformat muss auf "HTML-Vertr" geändert werden. Dies ist das Format, welches in Schritt 1 importiert wurde.
* Die Anzahl der Tage muss auf einen gewünschten Wert geändert werden. Der Wert von 7 spuckt später die Vertretungen der nächsten sieben Tage aus.
* Der Wert "Max. Zeilen pro Seite" muss auf den höchsten Wert von 9999 geändert werden. Anderenfalls werden nicht alle Vertretungen auf eine HTML-Seite geschrieben (die Bibliothek unterstützt dies noch nicht).
* Die Option "Texte zum Tag auf jeder Seite" kann optional aktiviert werden. Dann ist es auch möglich, die Tagestexte zu exportieren. Anderenfalls können diese nicht exportiert werden.
* Das Exportverzeichnis muss auf ein beliebiges Verzeichnis geändert werden.

.. image:: images/untis-export-2.png

Anschließend bestätigt man mit "OK.

Im Info-Stundenplan-Dialog abschließend noch das Häckchen "aktiv" setzen. Es empfiehlt sich, das Häckchen "Ausgabeverzeichnis löschen" zu setzen - das ist jedoch kein Muss.

Nun kann man mittels "Exportieren" den HTML-Vertretungsplan erzeugen.

Schritt 4: Untis Info-Stundenplan einrichten (Stundenplan)
###############################################

Im Ribbon klickt man oben auf Info-Stundenplan. Es öffnet sich der folgende Dialog:

.. image:: images/untis-export-stundenplan-1.png

Dort wählt man die Einstellung "Standard" (alternativ lässt sich auch eine andere nutzen) und klickt auf den Button "..." neben dem Einstellungsnamen.

Im sich öffnenden Dialog müssen nun die folgenden Dinge angepasst werden:

* Option "Ein Stundenplan pro Periode" aktivieren (nicht zwangsläufig notwendig, aber hilfreich -- wer möchte schon Wochen-Stundenpläne haben?!)
* Maximale Wochenanzahl auswählen (hat man die Perioden-Option gewählt, sind hiermit die Anzahl der Perioden gemeint)
* "Wochenverzeichnisse in der Struktur oben" bewirkt, dass man folgende Verzeichnisstruktur erhält:
    * Option aktiv: `/P1/c/*.htm` und `/P1/f/*.htm`
    * Option nicht aktiv: `/c/P1/*.htm` und `/f/P1/*.htm`

.. image:: images/untis-export-stundenplan-2.png

Im Reiter "Stundenplan" muss man noch die Formate einstellen:

.. image:: images/untis-export-stundenplan-3.png

Anschließend kann man den Stundenplan exportieren und die HTML-Dateien mit der Bibliothek auslesen.