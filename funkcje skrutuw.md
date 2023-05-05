wybieramy 3 dowolne funkcje skrutu
MD5
SHA3
SHA1

Badamy jak szybko utworzymy skrot dla 3 plikow o roznych rozmiarach.
porownac to na wykresie
wnioski

sprawdzamy kryterium ssak. czyli kryterium lawinowosci zmian. jesli zmienimi pojedynczy bit w wiadomosci w skrocie powinno sie zmienic polowa bitow. mozna to sprawdzic za pomoca xor na przyklad.
zrobic to dla dowolnej albo 2 funkcjach.

sprubujemy znalezc kolizje. sprawdzamy na ilu x pierwszych bitach mozna znalezc skrot. np. na piewszych 12,32, 64 bitach. dla jednej wybranej funkcji skrotu i wnioski..

badanie trybówąś
Tryby pracy: bierzemy 3 tryby pracy, CTR CBC ECB.
1. szybkosc
2. badamy propagacje bledów.
propagacja do przodu. i do tyłu, ile wiadomosci da się odzyskać
3. własna implementacja trybu CBC z wykorzystanei ECB