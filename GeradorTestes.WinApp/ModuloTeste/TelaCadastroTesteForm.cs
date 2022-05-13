using FluentValidation.Results;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloTeste;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloTeste
{
    public partial class TelaCadastroTesteForm : Form
    {
        private Teste teste;
        public TelaCadastroTesteForm(List<Disciplina> disciplinas , List<Materia> materias)
        {
            InitializeComponent();
            CarregarMaterias(materias);
            CarregarDisciplinas(disciplinas);
        }
        private void CarregarDisciplinas(List<Disciplina> disciplinas)
        {
            cmbDisciplinas.Items.Clear();

            foreach (var item in disciplinas)
            {
                cmbDisciplinas.Items.Add(item);
            }
        }

        private void CarregarMaterias(List<Materia> materias)
        {
            cmbMaterias.Items.Clear();

            foreach (var item in materias)
            {
                cmbMaterias.Items.Add(item);
            }
        }

        public Func<Teste, ValidationResult> GravarRegistro { get; set; }

        public Teste Teste
        {
            get { return teste; }
            set
            {
                teste = value;

                txtNumero.Text = teste.Numero.ToString();
                txtTitulo.Text = teste.Titulo;
                txtData.Value = teste.Data;

                cmbDisciplinas.Enabled = teste.Disciplina != null;

                checkMarcarDisciplina.Checked = teste.Disciplina != null;

                cmbDisciplinas.SelectedItem = teste.Disciplina;


                cmbMaterias.Enabled = teste.Materia != null;

                checkMarcarMateria.Checked = teste.Materia != null;

                cmbMaterias.SelectedItem = teste.Materia;


            }
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            teste.Titulo = txtTitulo.Text;
            teste.Data = txtData.Value;
            teste.Materia = (Materia)cmbMaterias.SelectedItem;
            teste.Disciplina = (Disciplina)cmbDisciplinas.SelectedItem;
            teste.qtdQuestoes = Convert.ToInt32(txtQtdQuestoes);

            var resultadoValidacao = GravarRegistro(teste);


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
            TelaPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void checkMarcarMateria_CheckedChanged(object sender, EventArgs e)
        {
            cmbMaterias.Enabled = checkMarcarMateria.Checked;
            cmbMaterias.SelectedIndex = -1;
        }

        private void checkMarcarDisciplina_CheckedChanged(object sender, EventArgs e)
        {
            cmbDisciplinas.Enabled = checkMarcarDisciplina.Checked;
            cmbDisciplinas.SelectedIndex = -1;
        }
    }
}
