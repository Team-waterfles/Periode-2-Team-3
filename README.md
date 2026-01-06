## Projectnaam
Typ.IO

## Omschrijving
een project om een typcursus aantrekkelijk te maken voor studenten door middel van muziek en ritme

## Teamleden
Bo Külman

Hazal Kalender

Abdi Abdulqadir

Michael van der Rhee

Rayen Khelifi

Sem Rusbach

## Werkwijze

In deze repository wordt gitflow gebruikt.

Maakt voor nieuwe features een nieuwe branch aan vanaf develop met een naam die begint met "feature/"
Als een feature af is dient een pull request naar develop aangemaakt te worden. Als alle tests slagen en de andere ontwikkelaars geen problemen vinden kan de merge voltooid worden

Als er een bug gevonden wordt die meteen opgelost moet worden kan gebruik gemaakt worden van de hotfix branch. Haal dan eerst de code van main naar de hotfix branch, programeer vervolgens de hitfix en maakt als het af is een pull request aan terug naar main. Dit verzoek wordt dan ook gecontroleerd op fouten door andere ontwikkelaars

Als alle features voor een sprint af zijn gerond kan de code van develop naar main worden gemerged. Hierna krijgt die commit van main een tag voor de release.

## Code Conventies

### Code

namen van classes, methods, enumerations, public fields, public properties, namespaces: PascalCase.

namen van local variables, parameters: camelCase.

namen van private, protected, internal en protected internal fields en properties: _camelCase.

Wat betreft hoofdlettergebruik is een "woord" alles wat geschreven is zonder interne spaties, inclusief acroniemen. Bijvoorbeeld MyRpc in plaats van MyRPC.

namen van interfaces starten met I, bijv. IInterface.

comments en variabelen in het nederlands declareren

### Files

Bestandsnamen en directorynamen zijn PascalCase, bijvoorbeeld MyFile.cs.

Waar mogelijk moet de bestandsnaam gelijk zijn aan de naam van de hoofdklasse in het bestand, bijvoorbeeld MyClass.cs.

Over het algemeen is het beter om één coreclass per bestand te gebruiken.

## Licensie
MIT License

Copyright (c) 2025 Abdi Abdulqadir, Bo Külman, Hazal Kalender, Michael van der Rhee, Rayen Khelifi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
