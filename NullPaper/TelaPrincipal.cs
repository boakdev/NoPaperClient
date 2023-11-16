using System;
using System.Windows.Forms;

namespace NullPaper
{
    public partial class TelaPrincipal : Form
    {
        public TelaPrincipal()
        {
            InitializeComponent();

            PrinterUtilsRefatorado printerUtilsRefatorado = new PrinterUtilsRefatorado();
            printerUtilsRefatorado.MonitoraAlteracaoStatus();



            tabelaLeitura.DataSource = Banco.LeituraDAO.pegarLeitura();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void alteraLabel(int total)
        {
           
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            tabelaLeitura.DataSource = Banco.LeituraDAO.pegarLeitura();
        }
    }
}
