using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //for (int i = 0; i < Cep.faixas1.Length; i++)
            //    Console.WriteLine(Correios.Fac.CepNet($"{Cep.faixas1[i]}",1));

            string cif = Correios.Fac.Cif(72, 09142177, 12345, 13081, 040618, "01000-000", true);
            string cif2 = Correios.Fac.Cif(72, 09142177, 12345, 13081, 040618, "01000-000", false);
            string cif3 = Correios.Fac.Cif(72, 09142177, 12345, 13081, 040618, "01000-000",true);
            string dtmtrx = Correios.Fac.DataMatrix("01000-000", 0, "05099-999", 0, int.Parse(Correios.Fac.CepNet("01000-000", 1)),
                                     1, cif, 0, 82015, 0, 1813099);


            Console.WriteLine($"cif1 ------ {cif}");
            Console.WriteLine($"cif2 ------ {cif2}");
            Console.WriteLine($"cif3 ------ {cif3}");
            Console.WriteLine($"DtMatrix-- {dtmtrx}");
            Console.ReadKey();
        }
    }
}
