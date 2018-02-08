using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Algorithms
{
    // ALD notes - TODO refactor
    class Rekurencja
    {
        // Funkcja która w rezultacie wywołuje samą siebie z innymi
        // parametrami. Algorytm reukurencyjny to taki ktory odwołuje
        // sie do samego siebie.

        // 1. Problem:
        //    Dysponujemy tablicą n liczb całkowitych tab[n] = tab[0], tab[1],..., tab[n–1].
        //    Czy w tablicy tab występuje liczba x (podana jako parametr)?
        // *  Algorytm:
        //    - wziąć pierwszy niezbadany element tablicy n-elementowej;
        //    - jeśli aktualnie analizowany element tablicy jest równy x, to:
        //          ogłoś „Sukces” i zakończ;
        //      w przeciwnym przypadku:
        //          zbadaj pozostałą część tablicy.    

        public class Problem_1_Rozwiazanie_1
        {
            const int n = 10;           // l. el. w tab
            int[] tab = new int[n] { 1, 2, 3, 2, -7, 44, 5, 1, 0, -3 };

            public void Main()
            {
                Szukaj(tab, 0, n - 1, 7);  // tablica, poczatkowy el. ostatni el, szulana wartosc
                Szukaj(tab, 0, n - 1, 5);
            }

            private void Szukaj(int[] table, int left, int right, int x)
            {
                if (left > right)
                {
                    Console.WriteLine("Element {0} nie zostal odnaleziony", x);
                }
                else
                {
                    if (tab[left] == x)
                    {
                        Console.WriteLine("Znalazlem szukany element {0}", x);
                    }
                    else
                    {
                        this.Szukaj(table, left + 1, right, x);
                    }
                }
            }
        }

        // 0! 1,
        // n! = n*(n-1)!, gdzie n nalezy do N, n >= 1

        public class Problem_2_Silnia
        {
            private long Silnia(int x)
            {
                if (x == 0)
                {
                    return 1;
                }
                else
                {
                    return x * this.Silnia(x - 1);
                }

            }

            public void Main(int x = 5)
            {

                Console.WriteLine("silnia({0}) = {1}", x, this.Silnia(5));
            }

        }

        // fib(0) = 0
        // fib(1) = 1
        // fib(n) = fib(n-1) + fib(n-2), gdzie n >= 2
        // 
        // załózmy taki problem:
        // Hodujemy króliki i mamy gwarantowany wzrost populacji według reguł:
        // * Zaczynamy od jednej pary.
        // * Każda samica królika wydaje na świat potomstwo w miesiąc po kopulacji
        //   — jednego samca i jedną samicę.
        // * W miesiąc po urodzeniu królik może przystąpić do reprodukcji.
        public class Problem_3_Fibonacci
        {
            private long Fib(int x)
            {
                if (x < 2)
                {
                    return x;
                }
                else
                {
                    return Fib(x - 1) + Fib(x - 2);
                }
            }

            public void Main(int x = 10)
            {
                Console.WriteLine("Ciag fibanacigego dla {0} el. ma wartosc {1}", x, this.Fib(x));
            }
        }


    }
}
