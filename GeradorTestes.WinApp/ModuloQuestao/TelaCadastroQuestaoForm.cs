using FluentValidation.Results;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloQuestao
{
    public partial class TelaCadastroQuestaoForm : Form
    {
        private Questao questao;
        public TelaCadastroQuestaoForm(List<Materia> materias)
        {
            InitializeComponent();

            CarregarMaterias(materias);
            
        }

        private void CarregarMaterias(List<Materia> materias)
        {
            cmbMaterias.Items.Clear();

            foreach (var item in materias)
            {
                cmbMaterias.Items.Add(item);
            }
        }

        public Func<Questao, ValidationResult> GravarRegistro { get; set; }

        public Questao Questao
        {
            get { return questao; }
            set
            {
                questao = value;

                txtNumero.Text = questao.Numero.ToString();
                txtEnunciado.Text = questao.Enunciado;

                cmbMaterias.Enabled = questao.Materia != null;

                checkMarcarMateria.Checked = questao.Materia != null;

                cmbMaterias.SelectedItem = questao.Materia;

         
            }
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            questao.Enunciado = txtEnunciado.Text;
            questao.Materia = (Materia)cmbMaterias.SelectedItem;

            var resultadoValidacao = GravarRegistro(questao);


            if (resultadoValidacao.IsValid == false)
            {
                string erro = resultadoValidacao.Errors[0].ErrorMessage;

                TelaPrincipalForm.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }
        }

        private void TelaCadastroMateriaForm_Load(object sender, EventArgs e)
        {
            TelaPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void TelaCadastroMateriaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TelaPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void checkMarcarMateria_CheckedChanged(object sender, EventArgs e)
        {
            cmbMaterias.Enabled = checkMarcarMateria.Checked;
            cmbMaterias.SelectedIndex = -1;
        }
    }
}
