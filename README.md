# Raspodijeljeni sustavi
Projekt kolegija Raspodijeljeni sustavi 2021.

# Projektne faze
- [x] - Opis projekta
- [x] - Funkcionalnosti projekta
- [x] - Faze izrade projeka
- [ ] - Prva verzija
- [ ] - Finalna verzija
- [ ] - Obrana projekta

# Opis projekta
## Snap! – online multiplayer card game
Implementirati online kartašku igru Snap za 2 – 3 igrača. 

Pravila igre: Igra se sa špilom od 52 karte. Sve karte iz špila se dijele jedna po jedna u krug igračima, tako da svaki igrač ispred sebe ima hrpu/špil karata okrenutih prema dolje. Igru započinje jedan igrač tako da okrene najgornju kartu iz svog špila i stavlja je na novu hrpu/špil ispred sebe (sada ima svoja 2 špila: okrenute i neokrenute karte), postupak ponavljaju svi igrači u krug ulijevo od prvog. Igrač koji prvi primijeti da je njegova karta ista kao i kod drugog igrača, te pozove „Snap!“, osvaja špil okrenutih karata drugog igrača. Pobijedio je igrač koji prvi skupi sve karte.

## Tehnologije
1. ASP.NET Core MVC
2. .NET Core Console App
3. Akka.NET

## Popis funkcionalnosti
1. igrač/klijent otvara igru u web pregledniku
2. igrač upisuje korisničko ime, igra ga spoji sa serverom, korisniku se prikažu svi trenutno online igrači
3. igrač može ili započeti novu igru i u nju pozvati ostale igrače koji su online ili može biti pozvan u igru
4. tijek igre: 
   1.	karte se dijele jedna po jedna svim igračima, dok svaki igrač ne bude imao ispred sebe svoj špil sa neokrenutim kartama
   2.	igrač koji je započeo igru igra prvi te okreće kartu na vrhu svog špila i stavlja je u novi špil tako da ima svoja 2 špila (okrenute i neokrenute karte), 
      tako rade i osatli igrači
   3. ako se igračima okrenu iste karte, igrač koji prvi klikne na gumb Snap!, dobije karte sa protivničkog špila
   4.	pobjednik je igrač koji prvi skupi sve karte u špilu
5. ukoliko netko od igrača tijekom igre napusti igru, igra završava i svi se igrači vraćaju na početnu stranicu gdje se trebaju priključiti novoj igri


# Faze izrade projekta
   Rok      | Zadatak
 ---------- | -------------
01.06.2021. | predati izvještaj o napretku projekta skupa sa trenutnom verzijom projekta
15.06.2021. | predati drugi izvještaj o napretku projekta skupa sa trenutnom verzijom projekta
25.07.2021. | predaja gotovog projekta









