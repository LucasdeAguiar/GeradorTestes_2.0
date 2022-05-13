namespace GeradorTestes.WinApp.ModuloQuestao
{
    partial class TelaCadastroQuestaoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtNumero = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnunciado = new System.Windows.Forms.TextBox();
            this.checkMarcarMateria = new System.Windows.Forms.CheckBox();
            this.cmbMaterias = new System.Windows.Forms.ComboBox();
            this.btnGravar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gpAlternativas = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAlternativa = new System.Windows.Forms.TextBox();
            this.btnAdicionarAlternativa = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Número";
            // 
            // txtNumero
            // 
            this.txtNumero.Enabled = false;
            this.txtNumero.Location = new System.Drawing.Point(137, 23);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Size = new System.Drawing.Size(165, 23);
            this.txtNumero.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enunciado";
            // 
            // txtEnunciado
            // 
            this.txtEnunciado.Location = new System.Drawing.Point(137, 61);
            this.txtEnunciado.Name = "txtEnunciado";
            this.txtEnunciado.Size = new System.Drawing.Size(165, 23);
            this.txtEnunciado.TabIndex = 3;
            // 
            // checkMarcarMateria
            // 
            this.checkMarcarMateria.AutoSize = true;
            this.checkMarcarMateria.Location = new System.Drawing.Point(53, 103);
            this.checkMarcarMateria.Name = "checkMarcarMateria";
            this.checkMarcarMateria.Size = new System.Drawing.Size(173, 19);
            this.checkMarcarMateria.TabIndex = 4;
            this.checkMarcarMateria.Text = "Habilitar Seleção de Materia";
            this.checkMarcarMateria.UseVisualStyleBackColor = true;
            this.checkMarcarMateria.CheckedChanged += new System.EventHandler(this.checkMarcarMateria_CheckedChanged);
            // 
            // cmbMaterias
            // 
            this.cmbMaterias.FormattingEnabled = true;
            this.cmbMaterias.Location = new System.Drawing.Point(53, 140);
            this.cmbMaterias.Name = "cmbMaterias";
            this.cmbMaterias.Size = new System.Drawing.Size(230, 23);
            this.cmbMaterias.TabIndex = 5;
            // 
            // btnGravar
            // 
            this.btnGravar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnGravar.Location = new System.Drawing.Point(301, 440);
            this.btnGravar.Name = "btnGravar";
            this.btnGravar.Size = new System.Drawing.Size(83, 43);
            this.btnGravar.TabIndex = 6;
            this.btnGravar.Text = "Gravar";
            this.btnGravar.UseVisualStyleBackColor = true;
            this.btnGravar.Click += new System.EventHandler(this.btnGravar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(390, 440);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(83, 43);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // gpAlternativas
            // 
            this.gpAlternativas.Location = new System.Drawing.Point(53, 307);
            this.gpAlternativas.Name = "gpAlternativas";
            this.gpAlternativas.Size = new System.Drawing.Size(370, 127);
            this.gpAlternativas.TabIndex = 8;
            this.gpAlternativas.TabStop = false;
            this.gpAlternativas.Text = "Alternativas";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Resposta";
            // 
            // txtAlternativa
            // 
            this.txtAlternativa.Location = new System.Drawing.Point(126, 227);
            this.txtAlternativa.Name = "txtAlternativa";
            this.txtAlternativa.Size = new System.Drawing.Size(157, 23);
            this.txtAlternativa.TabIndex = 10;
            // 
            // btnAdicionarAlternativa
            // 
            this.btnAdicionarAlternativa.Location = new System.Drawing.Point(301, 227);
            this.btnAdicionarAlternativa.Name = "btnAdicionarAlternativa";
            this.btnAdicionarAlternativa.Size = new System.Drawing.Size(87, 24);
            this.btnAdicionarAlternativa.TabIndex = 11;
            this.btnAdicionarAlternativa.Text = "Adicionar";
            this.btnAdicionarAlternativa.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(126, 256);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 19);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Alternativa Correta";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // TelaCadastroQuestaoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 495);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnAdicionarAlternativa);
            this.Controls.Add(this.txtAlternativa);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gpAlternativas);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGravar);
            this.Controls.Add(this.cmbMaterias);
            this.Controls.Add(this.checkMarcarMateria);
            this.Controls.Add(this.txtEnunciado);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNumero);
            this.Controls.Add(this.label1);
            this.Name = "TelaCadastroQuestaoForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cadastro de Questões";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNumero;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEnunciado;
        private System.Windows.Forms.CheckBox checkMarcarMateria;
        private System.Windows.Forms.ComboBox cmbMaterias;
        private System.Windows.Forms.Button btnGravar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.GroupBox gpAlternativas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAlternativa;
        private System.Windows.Forms.Button btnAdicionarAlternativa;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}