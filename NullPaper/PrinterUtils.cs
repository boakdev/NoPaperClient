using NullPaper.Banco;
using NullPaper.Modelo;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace NullPaper

    
{

    
    static class PrinterUtils
    {

       
        //static int idLeitura = 0;
        //static List<int> listaTotalPage = new List<int>();


        public static void MonitoraImpressao()
        {

            foreach (string printname in PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine($">>> Impressora: '{printname}'");
            }

 

            // Consulta para monitorar os eventos de impressão
            WqlEventQuery query = new WqlEventQuery("__InstanceCreationEvent",
                new TimeSpan(0, 0, 1),
                "TargetInstance ISA 'Win32_PrintJob'");

            // Criar o watcher de eventos
            ManagementEventWatcher watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += new EventArrivedEventHandler(PrintJobEventArrived);


            // Iniciar o watcher
            watcher.Start();


            MonitoraAlteracaoStatus();

            //Console.WriteLine("Aguardando eventos de impressão. Pressione Enter para sair.");
            //Console.ReadLine();

            // Parar o watcher quando o usuário pressionar Enter
            //watcher.Stop();
        }

        static public void MonitoraAlteracaoStatus()
        {

            // verifica apenas uma vez
            //WqlEventQuery query = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1),
            //    "TargetInstance ISA 'Win32_PrintJob'");


            // verifica alteracoes de status
            string queryClassName = "__InstanceOperationEvent";
            string queryCond = "TargetInstance ISA 'Win32_PrintJob'";
            TimeSpan queryTimeSpan = new TimeSpan(1);

            try
            {
                WqlEventQuery eventQuery = new WqlEventQuery(queryClassName, queryTimeSpan, queryCond);
                //ManagementEventWatcher watcher1 = new ManagementEventWatcher(query);
                ManagementEventWatcher watcher2 = new ManagementEventWatcher(eventQuery);

                //watcher1.EventArrived += new EventArrivedEventHandler(PrintJobEventArrived);
                watcher2.EventArrived += PrintJobChange;
               
                //watcher1.Start();
                watcher2.Start();
               

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error monitoring print jobs. Exception {0} Trace {1}.",
                ex.Message, ex.StackTrace);
                Console.WriteLine(msg, "Error");
            }
        }


        static void PrintJobEventArrived(object sender, EventArrivedEventArgs e)
        {
            //totalNumberPages = 0;
            //idLeitura = 0;
            //listaTotalPage.Clear();


            ManagementBaseObject printJob = (ManagementBaseObject)e.NewEvent["TargetInstance"];

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

            if (LeituraDAO.gravarLeitura(leitura))
            {
                //Console.WriteLine($"Salvou com sucesso! HASH-CODE>>> " + printJob.GetHashCode().ToString());
                Console.WriteLine("");
                Console.WriteLine("******* INÍCIO - GRAVAÇÃO *********** ");
                Leitura leituraPersist;
                leituraPersist = LeituraDAO.recuperarLeituraPersist(leitura);
                if (leituraPersist != null)
                {
                    Console.WriteLine("ID_LEITURA >>> " + leituraPersist.Id);
                    Console.WriteLine("TOTAL-PAGES >>> " + leituraPersist.TotalPages);
                    Console.WriteLine("[JOB-ID] >>> " + leituraPersist.JobId);
                    Console.WriteLine("[IMPRESSORA] >>> " + leituraPersist.Impressora);
                    Console.WriteLine("******* FIM - GRAVAÇÃO *********** ");
                    Console.WriteLine("");
                    //idLeitura = leituraPersist.Id;
                }


                

            }
            else
            {
                Console.WriteLine($"ERRO ao salvar no banco!");
            }

            //PropertyDataCollection properties = printJob.Properties;
            //foreach (PropertyData property in properties)
            //{
            //    string propertyName = property.Name;
            //    object propertyValue = property.Value;
            //    Console.WriteLine($"Propriedade: {propertyName}, Valor: {propertyValue}");

            //}

        }


        static public void PrintJobChange(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject printJob = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;

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

            Leitura leituraPersist;
            leituraPersist = LeituraDAO.recuperarLeituraPersist(leitura);


            int totalPages = Int32.Parse(printJob["TotalPages"].ToString());
            Console.WriteLine("[TOTAL-PAGES] >>> " + totalPages);


            if (leituraPersist != null && leituraPersist.TotalPages < totalPages)
                {
                    Console.WriteLine("");
                    Console.WriteLine("******* INÍCIO - ATUALIZAÇÃO *********** ");
                    LeituraDAO.atualizaTotalPages(leituraPersist.Id, totalPages);
                    Console.WriteLine("[ID-LEITURA]: " + leituraPersist.Id + "| Total Pages Atual: " + totalPages);
                    Console.WriteLine("******* FIM - ATUALIZAÇÃO *********** ");
                }
            

            //string jobStatus = (string)objProps["JobStatus"];
            //uint totalPages = ((uint)objProps["TotalPages"]);



            //Console.WriteLine("Status: " + jobStatus + ", Total Pages: " + totalPages);

        }

    }
}
