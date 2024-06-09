Tetris

Inicializace a načtení všech obrázků, které hra používá.

NextGamePhase()
- připraví herní pole, načte nejvyšší skóre
- po zmáčknutí start vykreslí herní pole, posune do herní fáze - začne hru
- po skončení hry vykreslí finální skóre a nabídne hrát znovu

InitializeCanvas()
- předpřipraví prázdnou herní mřížku a prázdné statistiky bloků

SetBlockStatisticsImages()
- vykreslí obrázky ke statistikám

Jak fungují jednotlivé blocky?
- každý block má svou mřížku (uchovává souřadnice jednotlivých kostiček, ze kterých se block skládá), která se překresluje do herní mřížky - herního pole
- když se potom block má otočit, jenom si načte další mřížku, ve které jsou jiné souřadnice jednotlivých kostek
- každý block má potom své určené číslo - pro zjednodušení vytváření nových blocků (abecední pořadí - block I = 1, block J = 2, ...)

DrawBlock()
- tato metoda nejdříve zkontroluje, jestli náhodou pokud mají herní mřížka i mřížka blocku souřadnice levé horní kostky 0,0 se celý block může vykreslit, pokud ne - hra skončila, protože už nejde vytvořit nový block
- když je jasné, že nový block lze vykreslit, projde mřížku blocku a překreslí obrázky v herní mřížce z prázdných kostek na kostky blocku
- zároveň si ukládá, které kostky v herní mřížce jsou obsazené, aby se pak dalo zjistit, jestli se na to místo může vykreslit jiný block

RemoveOneBack()
- stejně jako DrawBlock() projde herní mřížku a mřížku blocku, ale překresluje na prázdnou kostku, stejně tak si ukládá prázdné místo
- tato metoda se musí spustit vždy před metodou DrawBlock()

MakeNewBlock(int[] coords, int makeThisBlock = 0)
- přijímá souřadnice levé horní kostky herní mřížky, kde se bude nacházet levá horní kostka mřížky blocku
- pokud je potřeba vytvořit nějaký specifický block, třeba když se bere block z holdu, pošle se i číslo blocku, který se má vytvořit
- pokud se nevytváří specifický block, vytvoří náhodné číslo a podle toho nový block, stejně tak si hlídá i block, který má následovat
- přepíše statistiky a změní obrázek kostky, která se bude vykreslovat

HoldBlock()
- pokud v tomhle kole už nebyl použit hold, smaže vykrslení tohohle blocku v mřížece, prohodí z blockem, který je momentálně v holdu (nastaví číslo, které odpovídá tomu blocku), pokud tam žádný není vezme další block

MoveBlockLeft(), MoveBlockRight(), MoveBlockDown()
- zkontroluje kostku po kostce, jestli je prázdné (nebo existuje) políčko směrem, kam se má posunout
- pokud není - nic nedělá
- pokud je možné posunout, překreslí block s jinými souřadnicemi

IsOccupied(int x, int y)
- v herní mřízce používám enumeraci na jednotlivé kostky - abych mohla vykreslit správnou barvu, až smažu řádek
- proto používám jednoduchou metodu, která se jen kouká, jestli je volná kostka v mřížce nebo ne

CheckAndRemoveRows(), RemoveRow(int rowNumber)
- projde mřížku, jestli není plný některý řádek
- pokud je řádek plný vezme všechny řádky, které jsou nad ním a posune je o 1 dolů (přes ten plný řádek)
- překreslí mřížku na základě enumerace kostek, které jsou na jednotlivých políčkách mřížky
- GetTileImage(int rowNumber, int y) - předává obrázky na základě enumerace

CheckHighestScore()
- pokud bylo dosaženo vyššího skóre než bylo momentálně načtené jako nejvyšší, zapíše do souboru

LoadHighestScore()
- pokud existuje soubor s nejvyšším skórem, vezme si z něj číslo nejvyššího skóre
