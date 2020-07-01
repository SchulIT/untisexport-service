# UntisExportService

[![Build Status](https://dev.azure.com/schulit/UntisExportService/_apis/build/status/SchulIT.untisexport-service?branchName=master)](https://dev.azure.com/schulit/UntisExportService/_build/latest?definitionId=3&branchName=master)
![GitHub](https://img.shields.io/github/license/schulit/untisexport-service?style=flat-square)
![.NET Core 3.1](https://img.shields.io/badge/.NET%20Core-3.1-brightgreen?style=flat-square)

Dieser Dienst verwendet die [UntisExport-Bibliothek](https://github.com/schulit/untisexport), um Daten aus Untis (Vertretungen, Klausuren, Räume, Stundenplan, Aufsichten und Unterrichte) zu parsen und anschließend externe Systeme (bspw. das [ICC](https://github.com/schulit/icc)) mit diesen Daten zu speisen.

Beim Import für das ICC ist dabei wichtig, dass dieser Dienst mit externen Diensten wie SchILD NRW spricht, um Lerngruppen und Unterrichte auflösen zu können bevor sie ins ICC hochgeladen werden.

## Installation

Installationspakete gibt es auf [GitHub](https://github.com/schulit/untisexport-service/releases) als MSI.

## Konfiguration

Die Konfiguration muss aktuell händisch angelegt werden. Siehe [Handbuch](https://untisexport-service.readthedocs.org).

## Anwendungen

Das Programm besteht aus drei Teilen: Konsole, Windows-Dienst und grafischer Oberfläche.

## Konsole

Die Konsolenanwendung überwacht im Hintergrund die konfigurierten Exportverzeichnisse von Untis auf Änderungen und startet bei Änderungen einen Importvorgang.

## Windows Dienst

Wie die Konsolenanwendung überwacht der Windows Dienst die konfigurierten Exportverzeichnisse von Untis auf Änderungen und startet bei Änderungen einen Importvorgang. Im Gegensatz zur Konsolenanwendung läuft der Windows Dienst im Hintergrund.

## Grafische Oberfläche

Die grafische Oberfläche importiert die gewünschten Untis Daten auf Knopfdruck in die in der Konfigurationsdatei hinterlegten Systeme.

## Anmerkungen

Aktuell unterstützt dieser Dienst den Export als JSON und ins [ICC](https://github.com/schulit/icc). Zur Auflösung der Unterrichte und Lerngruppen wird dabei SchILD NRW vorausgesetzt. Sollte ein anderes Programm verwendet werden, muss eine Erweiterung des Dienstes programmiert werden.

## Lizenz

Der Quelltext steht (abgesehen von den [Icons](ICONS_LICENSE.md)) unter der [MIT License](LICENSE.md).