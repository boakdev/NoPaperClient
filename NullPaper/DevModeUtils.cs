using NullPaper.Banco;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NullPaper
{

    static class DevModeUtils
    {
        // Importa a função para obter o DEVMODE da impressora
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr DocumentProperties(
            IntPtr hwnd, IntPtr hPrinter, string pDeviceName,
            IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);

        // Constantes para o modo de operação do DocumentProperties
        private const int DM_OUT_BUFFER = 2;
        private const int DM_IN_BUFFER = 8;


        public static void GetPrinterConfig(string printerName)
        {
            //PrinterSettings printerSettings = new PrinterSettings();
            //string printerName = printerSettings.PrinterName;

            // Obtém o tamanho da estrutura DEVMODE
            int size = (Int32)DocumentProperties(IntPtr.Zero, IntPtr.Zero, printerName, IntPtr.Zero, IntPtr.Zero, 0);

            // Alocar memória para a estrutura DEVMODE
            IntPtr pDevMode = Marshal.AllocHGlobal(size);

            // Obtém os dados da impressora
            int result = (Int32)DocumentProperties(IntPtr.Zero, IntPtr.Zero, printerName, pDevMode, IntPtr.Zero, DM_OUT_BUFFER);
            
            // Verifica se a chamada foi bem-sucedida
            if (result >= 0)
            {

                // Converte o ponteiro DEVMODE para a estrutura DEVMODE
                DEVMODE devMode = (DEVMODE)Marshal.PtrToStructure(pDevMode, typeof(DEVMODE));

                // Agora você pode acessar as configurações da impressora em devMode

                Console.WriteLine("");
                Console.WriteLine("******* INÍCIO - DEVMODE *********** ");
                Console.WriteLine("DM-COPIES: " + devMode.dmCopies);
                Console.WriteLine("DM-DEVICE-NAME: " + devMode.dmDeviceName);
                Console.WriteLine("DM-FORM-NAME: " + devMode.dmFormName);
                 Console.WriteLine("******* FIM - DEVMODE *********** ");

                // Lembre-se de liberar a memória alocada
                Marshal.FreeHGlobal(pDevMode);
            }
            else
            {
                // Handle error
                Console.WriteLine("Erro ao obter os dados da impressora. Código de erro: " + result);
            }
        }


    }


}
