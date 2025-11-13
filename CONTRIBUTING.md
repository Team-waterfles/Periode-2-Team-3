# Branching strategie

In deze repository wordt gitflow gebruikt.

Maakt voor nieuwe features een nieuwe branch aan vanaf develop met een naam die begint met "feature/"
Als een feature af is dient een pull request naar develop aangemaakt te worden. Als alle tests slagen en de andere ontwikkelaars geen problemen vinden kan de merge voltooid worden

Als er een bug gevonden wordt die meteen opgelost moet worden kan gebruik gemaakt worden van de hotfix branch. Haal dan eerst de code van main naar de hotfix branch, programeer vervolgens de hitfix en maakt als het af is een pull request aan terug naar main. Dit verzoek wordt dan ook gecontroleerd op fouten door andere ontwikkelaars

Als alle features voor een sprint af zijn gerond kan de code van develop naar main worden gemerged. Hierna krijgt die commit van main een tag voor de release.