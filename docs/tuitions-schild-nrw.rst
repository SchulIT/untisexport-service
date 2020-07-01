Unterrichte aus SchILD NRW ermitteln
====================================

Nutzt man den Schild ICC Importer, so muss der UntisExportService Unterrichte
aus SchILD ermitteln, damit diese Online wiedererkannt werden.

Konfiguration
#############

Unter ``tuition_resolver`` muss ``null`` durch folgendes JSON ersetzt werden:

.. code-block:: json

    {
        "type": "schild"
    }

Das war es auch schon.