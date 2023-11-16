namespace NullPaper
{
    partial class TelaPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.tabelaLeitura = new System.Windows.Forms.DataGridView();
            this.lblResposta = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tabelaLeitura)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Location = new System.Drawing.Point(30, 418);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(151, 42);
            this.btnAtualizar.TabIndex = 0;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // tabelaLeitura
            // 
            this.tabelaLeitura.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabelaLeitura.Location = new System.Drawing.Point(12, 13);
            this.tabelaLeitura.Name = "tabelaLeitura";
            this.tabelaLeitura.RowHeadersWidth = 51;
            this.tabelaLeitura.RowTemplate.Height = 24;
            this.tabelaLeitura.Size = new System.Drawing.Size(1900, 381);
            this.tabelaLeitura.TabIndex = 1;
            // 
            // lblResposta
            // 
            this.lblResposta.AutoSize = true;
            this.lblResposta.Location = new System.Drawing.Point(554, 431);
            this.lblResposta.Name = "lblResposta";
            this.lblResposta.Size = new System.Drawing.Size(44, 16);
            this.lblResposta.TabIndex = 2;
            this.lblResposta.Text = "label1";
            // 
            // TelaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 486);
            this.Controls.Add(this.lblResposta);
            this.Controls.Add(this.tabelaLeitura);
            this.Controls.Add(this.btnAtualizar);
            this.Name = "TelaPrincipal";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabelaLeitura)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.DataGridView tabelaLeitura;
        private System.Windows.Forms.Label lblResposta;
    }
}