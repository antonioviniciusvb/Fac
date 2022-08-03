using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Correios
{
    public static class Fac
    {
        /// <summary>
        /// Este método retorna o código CepNet
        /// retorno 0 = retorna o CepNet completo
        /// retorno 1 = retorna apenas o dígito do CepNet
        /// </summary>
        /// <param name="cep"></param>
        /// <param name="retorno"></param>
        /// <returns></returns>
        public static string CepNet(string cep, int retorno)
        {
            string auxCep = string.Empty, cepNet = string.Empty;
            int digControle = 0;

            if (cep.Length != 8)
                for (int i = cep.Length - 1; i < 7; i++)
                    auxCep += "0";

            auxCep += cep;
            double SomaCep = (char.GetNumericValue(auxCep[0]) + char.GetNumericValue(auxCep[1]) + char.GetNumericValue(auxCep[2]) + char.GetNumericValue(auxCep[3]) +
                               char.GetNumericValue(auxCep[4]) + char.GetNumericValue(auxCep[5]) + char.GetNumericValue(auxCep[6]) + char.GetNumericValue(auxCep[7]));

            int resto = ((int)SomaCep % 10);

            digControle = resto == 0 ? 0 : 10 - resto;


            if (retorno == 0)
                cepNet = $"{auxCep}{digControle}";
            else if (retorno == 1)
                cepNet = $"{digControle}";
            else
                throw new Exception("Retorno deve ser 0 ou 1");

            return cepNet;

        }

        /// <summary>
        /// Este método retorna o código cif formatado, com base nos parametros recebidos
        /// </summary>
        /// <param name="dr">Diretoria Regional</param>
        /// <param name="codAdm">Código Administrativo</param>
        /// <param name="lote">Número do Lote</param>
        /// <param name="seq">Número de sequencia</param>
        /// <param name="dtPostagem">Data de Postagem</param>
        /// <param name="cepDestino">Cep de destino</param>
        /// <param name="formatado">true = retorna cif formatado  false = retorna cif sem formatação</param>
        /// <returns></returns>
        public static string Cif(int dr, int codAdm, int lote, int seq, int dtPostagem, string cepDestino, bool formatado)
        {
            string codCif = string.Empty;

            //dr
            codCif += string.Format("{0:00}", dr);

            //Código Administrativo
            codCif += string.Format("{0:00000000}", codAdm);

            codCif += " ";

            //Número do lote
            if (lote > 0)
                codCif += string.Format("{0:00000}", lote);
            else
                throw new Exception("Número de lote deve ser superior a 0");

            codCif += " ";

            //Número de seq lote
            if (seq > 0)
                codCif += string.Format("{0:00000000000}", seq);
            else
                throw new Exception("Número de Sequencia do Objeto deve ser superior a 0");

            codCif += " ";

            #region Código Destino 1 = local, 2 = Estadual, 3 = Nacional

            string auxCep = limpaCep(cepDestino);

            if (Regex.IsMatch(auxCep, Util.Regex.CepLocal))
                codCif += "1";
            else
                if (Regex.IsMatch(auxCep, Util.Regex.CepEstadual))
                codCif += "2";
            else
                if (Regex.IsMatch(auxCep, Util.Regex.CepNacional))
                codCif += "3";
            #endregion

            //Código Reserva -- sempre 0
            codCif += "0";

            codCif += " ";

            //Data Postagem
            codCif += string.Format("{0:000000}", dtPostagem);

            codCif = formatado ? codCif : Regex.Replace(codCif, " ", "");

            return codCif;
        }

        /// <summary>
        /// Este méotodo retorna o código DataMatrix 2D dos Correios, com base nos parametros recebidos
        /// </summary>
        /// <param name="cepDestino"></param>
        /// <param name="numeroDestino"></param>
        /// <param name="cepRemetente"></param>
        /// <param name="numeroRemetente"></param>
        /// <param name="dgtControleCepDestino"></param>
        /// <param name="IDV"></param>
        /// <param name="cif"></param>
        /// <param name="svAdicionais"></param>
        /// <param name="svPrincipal"></param>
        /// <param name="reserva"></param>
        /// <param name="CNAE"></param>
        /// <returns></returns>
        public static string DataMatrix(string cepDestino, int numeroDestino, string cepRemetente, int numeroRemetente, 
                                         int dgtControleCepDestino, int IDV, string cif, int svAdicionais, int svPrincipal,
                                         int reserva, int CNAE)
        {
            string dtMatrix = string.Empty;

            //Limpa CEP DESTINO E REMETENTE
            string cepDestinoLimpo = limpaCep(cepDestino);
            string cepRemetenteLimpo = limpaCep(cepRemetente);

            //CEP Destino
            dtMatrix += cepDestinoLimpo;

            //Complemento CEP destino
            dtMatrix += string.Format("{0:00000}", numeroDestino);

            //CEP Remetente
            dtMatrix += cepRemetenteLimpo;

            //Complemento Remetente
            dtMatrix += string.Format("{0:00000}", numeroRemetente);

            //Digito CEP DESTINO
            dtMatrix += CepNet(cepDestinoLimpo, 1);

            //IDV
            dtMatrix += string.Format("{0:00}", IDV);

            //Cif
            dtMatrix += cif;

            //Serviço adiconal, FAC simples -- prencher com  10 zeros
            dtMatrix += string.Format("{0:0000000000}", svAdicionais);

            //Serviço Principal
            dtMatrix += string.Format("{0:00000}", svPrincipal);

            //Campo Reserva
            dtMatrix += string.Format("{0:000000000000000}", reserva);

            //CNAE
            dtMatrix += string.Format("{0:000000000}", CNAE);

            return dtMatrix;
        }


        /// <summary>
        /// Método que limpa o CEP, deixando apenas digitos
        /// </summary>
        /// <param name="cep"></param>
        /// <returns></returns>
        public static string limpaCep(string cep)
        {
            string CepLimpo = Regex.Replace(cep, Util.Regex.limpaCep, "");

            string auxCep = string.Empty;

            if (CepLimpo.Length > 0)
                auxCep = string.Format("{0:00000000}",int.Parse(CepLimpo));
            else
                auxCep = "00000000";
            return auxCep;
        }

        public static string limpaNumero(string numero)
        {
            string numeroLimpo = Regex.Replace(numero, Util.Regex.limpaCep, "");

            string auxNum = string.Empty;

            if (numeroLimpo.Length > 0)
                auxNum = string.Format("{0:00000}", int.Parse(numeroLimpo));
            else
                auxNum = "00000";

            return auxNum;
        }

        public static string servico(string Cep)
        {
            string cepLimpo = limpaCep(Cep);
            string servico = string.Empty;

            if (Regex.IsMatch(cepLimpo, Util.Regex.CepLocal))
                servico = "82015";
            else
              if (Regex.IsMatch(cepLimpo, Util.Regex.CepEstadual))
                servico = "82023";
            else
              if (Regex.IsMatch(cepLimpo, Util.Regex.CepNacional))
                servico = "82031";
            else
                servico = "00000";

            return servico;
        }
    }
    
}
