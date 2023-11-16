using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NullPaper.Modelo
{
   public class Leitura
    {

        public int Id {  get; set; }
        public string Computador { get; set; }
        public string Impressora { get; set; }
        public string Driver { get; set; }
        public int JobId { get; set; }
        public string Documento { get; set; }
        public string Usuario { get; set; }
        public string ColorMono { get; set; }
        public string TamanhoPapel { get; set; }
        public string DataImpressao { get; set; }
        public int TotalPages {  get; set; }
        public bool Enviado { get; set; }
        public string HashCode { get; set; }

        public string PortaImpressora { get; set; }

    }
}
