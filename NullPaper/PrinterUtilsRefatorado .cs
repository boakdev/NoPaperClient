using NullPaper.Banco;
using NullPaper.Modelo;
using System;
using System.Drawing.Printing;
using System.Management;
using System.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NullPaper


{


    class PrinterUtilsRefatorado
    {

        //[DllImport("winspool.drv")]
        //private static extern IntPtr OpenPrinter(string printerName, ref IntPtr pDefaultPrinter, IntPtr printerDefaults);

        //[DllImport("winspool.drv")]
        //private static extern bool GetPrinter(IntPtr hPrinter, int dwFlags, ref PRINTER_INFO_2 pPrinterInfo2);

        //[DllImport("winspool.drv")]
        //private static extern bool ClosePrinter(IntPtr hPrinter);

        ManagementScope oManagementScope = null;

        public void MonitoraAlteracaoStatus()
        {

            // verifica alteracoes de status
            string queryClassName = "__InstanceOperationEvent";
            string queryCond = "TargetInstance ISA 'Win32_PrintJob'";
            TimeSpan queryTimeSpan = new TimeSpan(1);

            try
            {
                WqlEventQuery eventQuery = new WqlEventQuery(queryClassName, queryTimeSpan, queryCond);
                ManagementEventWatcher watcher2 = new ManagementEventWatcher(eventQuery);

                watcher2.EventArrived += PrintJobChange;
                watcher2.Start();

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error monitoring print jobs. Exception {0} Trace {1}.",
                ex.Message, ex.StackTrace);
                Console.WriteLine(msg, "Error");
            }
        }



        private void PrintJobChange(object sender, EventArrivedEventArgs e)
        {



            LeituraDTO leituraDTO = new LeituraDTO();

            ManagementBaseObject printJob = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            int totalPages = Int32.Parse(printJob["TotalPages"].ToString());

            DevModeUtils.GetPrinterConfig(printJob["Name"].ToString().Split(',')[0]);

            //Início printer info
            oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
            oManagementScope.Connect();

            SelectQuery oSelectQuery = new SelectQuery();
            oSelectQuery.QueryString = @"SELECT * FROM Win32_Printer 
                     WHERE Name = '" + printJob["Name"].ToString().Split(',')[0].Replace("\\", "\\\\") + "'";

            //GetTotalPages(printJob["Name"].ToString().Split(',')[0]);

            ManagementObjectSearcher oObjectSearcher =
               new ManagementObjectSearcher(oManagementScope, @oSelectQuery);
            ManagementObjectCollection oObjectCollection = oObjectSearcher.Get();
            //fim printer info


            Leitura leitura = new Leitura();
            leitura.Computador = printJob["HostPrintQueue"].ToString();
            leitura.Impressora = printJob["Name"].ToString();
            leitura.Driver = printJob["DriverName"].ToString();
            leitura.JobId = Int32.Parse(printJob["JobId"].ToString());
            leitura.Documento = printJob["Document"].ToString();
            leitura.Usuario = printJob["Owner"].ToString();
            leitura.ColorMono = printJob["Color"].ToString();
            leitura.TamanhoPapel = printJob["PaperSize"].ToString();
            leitura.DataImpressao = printJob["TimeSubmitted"].ToString();
            leitura.TotalPages = Int32.Parse(printJob["TotalPages"].ToString());
            leitura.HashCode = printJob.GetHashCode().ToString();

            foreach (ManagementObject oItem in oObjectCollection)
            {
                leitura.PortaImpressora = oItem["PortName"].ToString();

            }

            Leitura leituraPersist = LeituraDAO.recuperarLeituraPersist(leitura);

            if (leituraPersist == null)
            {
                if (LeituraDAO.gravarLeitura(leitura))
                {
                    Console.WriteLine("");
                    Console.WriteLine("******* PRIMEIRA - GRAVAÇÃO *********** ");
                    Console.WriteLine("");




                }
            }
            else
            {

                if (leituraPersist.TotalPages < totalPages)
                {
                    Console.WriteLine("");
                    Console.WriteLine("******* INÍCIO - ATUALIZAÇÃO *********** ");
                    LeituraDAO.atualizaTotalPages(leituraPersist.Id, totalPages);
                    Console.WriteLine("JOB-STATUS: " + printJob["JobStatus"].ToString());
                    Console.WriteLine("STATUS: " + printJob["Status"].ToString());
                    Console.WriteLine("ATUALIZANDO... id-leitura: " + leituraPersist.Id + "| Total Pages Atual: " + totalPages);
                    Console.WriteLine("******* FIM - ATUALIZAÇÃO *********** ");

                }
            }
        }



        //public int GetTotalPages(string printerName)
        //{
        //    int totalPages = 0;

        //    // Get the handle to the spool window.
        //    IntPtr hPrinter = IntPtr.Zero;
        //    IntPtr pDefaultPrinter = IntPtr.Zero;
        //    IntPtr printerDefaults = IntPtr.Zero;
        //    try
        //    {
        //        hPrinter = OpenPrinter(printerName, ref pDefaultPrinter, printerDefaults);
        //        if (hPrinter == IntPtr.Zero)
        //        {
        //            throw new Exception("Failed to open printer.");
        //        }

        //        // Get the number of pages in the spool window.
        //        PRINTER_INFO_2 printerInfo2 = new PRINTER_INFO_2();
        //        bool success = GetPrinter(hPrinter, 2, ref printerInfo2);
        //        if (!success)
        //        {
        //            throw new Exception("Failed to get printer information.");
        //        }

        //        totalPages = (Int32)printerInfo2.cJobs;
        //        Console.WriteLine("GET-TOTAL >>> " + totalPages);
        //    }
        //    finally
        //    {
        //        // Detach the spool window.
        //        if (hPrinter != IntPtr.Zero)
        //        {
        //            ClosePrinter(hPrinter);
        //        }
        //    }

        //    return totalPages;
        //}
    }




}

