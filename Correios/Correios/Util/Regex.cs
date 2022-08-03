using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Correios.Util
{
    public static class Regex
    {
        public static string validaFormatoCep = @"^\d{2}\.?\d{3}[.-]?\d{3}";
        public static string CepLocal = @"^0[1-9]\d{6}";
        public static string CepEstadual = @"^1[1-9]\d{6}";
        public static string CepNacional = @"^[2-9]\d{7}";

        public static string limpaCep = @"\D";
    }
}
