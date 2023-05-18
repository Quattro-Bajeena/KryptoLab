# Funkcje Skrótów 

Autor: *Mateusz Oleszek*, nr. 144608

Wybrane funkcje do testowania: MD5, SHA-1, SHA-3

## Skrót za pomocą różnych algorytmów

![](F:\Programowanie\Studia\KryptoLab\screenshots\Screenshot_6151_WindowsTerminal-2023_05_05-15_23.png)

## Szybkość działania

<img src="F:\Programowanie\Studia\KryptoLab\screenshots\Screenshot_6149_EXCEL-2023_05_05-13_40.png" style="zoom:50%;" />

Najszybsza jest funkcja SHA-1, potem MD5, a na końcu SHA-3. 

## Funkcja MD5 dla krótkiego słowa

Skrót MD5 dla słowa wejściowego "ares": 1769d06df18cb4c2b01931d7f83f3c9a

Wyniki po wyszukaniu tego skrótu w google

<img src="F:\Programowanie\Studia\KryptoLab\screenshots\Screenshot_6146_firefox-2023_05_05-13_18.png" style="zoom:33%;" />



<img src="F:\Programowanie\Studia\KryptoLab\screenshots\Screenshot_6147_firefox-2023_05_05-13_18.png" style="zoom:33%;" />



## Czy funkcja skrótu nie jest bezpieczna

Funkcja MD5 jest powszechnie uznawana za złamaną kryptograficznie i nienadającą się do użytkowania od kilkunastu lat. Konsumenckie karty graficzne potrafią generować miliony skrótów na sekundę.

Zostały też znalezione ciągi które posiadają ten sam skrót, a co więcej nawet algorytm pozwalający stworzyć 2 pliki o dowolnej długości które będą miały ten sam skrót.



## Znajdowanie kolizji na pierwszych bitach

Dla funkcji skrótów wygenerowanych z **miliona** losowych ciągów znaków kolizje były znalezione na pierwszych **40 bitach**. Im więcej skrótów było generowanych tym można było znaleźć kolizję dla większej liczby bitów, np. dla tysiąca było to tylko pierwsze 19 bitów. Tak więc dla większych zbiorów skrótów będzie można znaleźć coraz bardziej podobne skróty z większym prawdopodobieństwem.



## Kryterium SAC

Zostały przetestowane milion losowych ciągów znaków, w których został zmieniony jeden bit a potem porównane bity skrótów oryginalnej i zmienionej wartości. Zaobserwowane procenty zmienionych bitów pomiędzy nimi to:

- Min: ~29%
- Max: ~72%
- Średnia: 50.001%

Patrząc na średnią wartość będącą prawie dokładnie 50% można uznać, że ten algorytm spełnia kryterium SAC.
