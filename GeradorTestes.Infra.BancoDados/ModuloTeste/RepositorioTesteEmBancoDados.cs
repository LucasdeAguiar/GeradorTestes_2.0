using FluentValidation.Results;
using GeradorTestes.Dominio.ModuloQuestao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorTestes.Infra.BancoDados.ModuloTeste
{
    public class RepositorioTesteEmBancoDados : IRepositorioQuestao
    {
        private const string enderecoBanco =
            "Data Source=(LocalDB)\\MSSqlLocalDB;" +
            "Initial Catalog=GeradorTestesDb;" +
            "Integrated Security=True;" +
            "Pooling=False";


        #region SQL Queries


        #endregion

        public ValidationResult Inserir(Questao novoRegistro)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Editar(Questao registro)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Excluir(Questao registro)
        {
            throw new NotImplementedException();
        }

        public Questao SelecionarPorNumero(int numero)
        {
            throw new NotImplementedException();
        }

        public List<Questao> SelecionarTodos()
        {
            throw new NotImplementedException();
        }


        //Métodos Privados
    }
}
