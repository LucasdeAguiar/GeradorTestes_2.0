using FluentValidation.Results;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorTestes.Infra.BancoDados.ModuloQuestao
{
    public class RepositorioQuestaoEmBancoDados : IRepositorioQuestao
    {
        private const string enderecoBanco =
            "Data Source=(LocalDB)\\MSSqlLocalDB;" +
            "Initial Catalog=GeradorTestesDb;" +
            "Integrated Security=True;" +
            "Pooling=False";


        #region SQL Queries
        private const string sqlInserirQuestao =
           @"INSERT INTO [TB_QUESTAO]
                   (
                        [ENUNCIADO],
                        [MATERIA_NUMERO],
                        [DISCIPLINA_NUMERO]
                        
                   )
                VALUES
                   (
                        @ENUNCIADO,
                        @MATERIA_NUMERO,
                        @DISCIPLINA_NUMERO
                    ); 
                SELECT SCOPE_IDENTITY()";



        private const string sqlInserirAlternativas =
            @"INSERT INTO [TB_ALTERNATIVA]
                (
                    DESCRICAO,
                    CORRETA,
                    QUESTAO_NUMERO
                )
                    VALUES
                (
                    @DESCRICAO,
                    @CORRETA,
                    @QUESTAO_NUMERO
                ); SELECT SCOPE_IDENTITY();";


        private const string sqlEditarQuestao =
             @"UPDATE TB_QUESTAO
                SET
                    ENUNCIADO = @ENUNCIADO,
                    MATERIA_NUMERO = @MATERIA_NUMERO,
                    DISCIPLINA_NUMERO = @DISCIPLINA_NUMERO
                WHERE
                    NUMERO = @NUMERO";

        private const string sqlEditarAlternativa =
            @"UPDATE TB_ALTERNATIVA
                SET
                    DESCRICAO = @DESCRICAO,
                    CORRETA = @CORRETA,
                    QUESTAO_NUMERO = @QUESTAO_NUMERO
                WHERE
                    NUMERO = @NUMERO";

        private const string sqlExcluirQuestao =
            @"DELETE FROM TB_QUESTAO
                WHERE
                    NUMERO = @NUMERO";

        private const string sqlExcluirAlternativas =
            @"DELETE FROM TB_ALTERNATIVA
                WHERE
                    QUESTAO_NUMERO = @NUMERO";

        private const string sqlSelecionarTodos =
            @"SELECT 
	                Q.NUMERO AS NUMERO,
	                Q.ENUNCIADO AS ENUNCIADO,
                    D.NUMERO AS DISCIPLINA_NUMERO,
	                D.NOME AS NOME_DISCIPLINA,
	                MT.NOME AS NOME_MATERIA,
                    MT.NUMERO AS MATERIA_NUMERO
                FROM
	                TB_QUESTAO AS Q
                INNER JOIN
	                TBDISCIPLINA AS D
                ON
	                D.NUMERO = Q.DISCIPLINA_NUMERO
                INNER JOIN
	                TBMATERIA AS MT
                ON
	                MT.NUMERO = Q.MATERIA_NUMERO";


        private const string sqlSelecionarPorNumero =
            @"SELECT 
	                Q.NUMERO AS NUMERO,
	                Q.ENUNCIADO AS ENUNCIADO,
                    D.NUMERO AS DISCIPLINA_NUMERO,
	                D.NOME AS NOME_DISCIPLINA,
	                MT.NOME AS NOME_MATERIA,
                    MT.NUMERO AS MATERIA_NUMERO
                FROM
	                TB_QUESTAO AS Q
                INNER JOIN
	                TBDISCIPLINA AS D
                ON
	                D.NUMERO = Q.DISCIPLINA_NUMERO
                INNER JOIN
	                TBMATERIA AS MT
                ON
	                MT.NUMERO = Q.MATERIA_NUMERO
                WHERE
                    Q.NUMERO = @NUMERO";



        #endregion


        public ValidationResult Inserir(Questao novaQuestao)
        {
            var validador = new ValidadorQuestao();

            var resultado = validador.Validate(novaQuestao);

            if (!resultado.IsValid)
                return resultado;

            SqlConnection conexaoComBanco = new(enderecoBanco);

            SqlCommand comandoInsercaoQuestao = new(sqlInserirQuestao, conexaoComBanco); 

            ConfigurarParametrosQuestao(novaQuestao, comandoInsercaoQuestao);

            conexaoComBanco.Open();

            var idQuestao = comandoInsercaoQuestao.ExecuteScalar();
            novaQuestao.Numero = Convert.ToInt32(idQuestao);

            SqlCommand comandoInsercaoAlternativa = new(sqlInserirAlternativas, conexaoComBanco);

            int i = 0;
            foreach (var alternativa in novaQuestao.Alternativas)
            {
                comandoInsercaoAlternativa.Parameters.Clear();
                ConfirugarParametrosAlternativas(alternativa, novaQuestao, comandoInsercaoAlternativa);
                var idAlternativa = comandoInsercaoAlternativa.ExecuteScalar(); 
                novaQuestao.Alternativas[i].Numero = Convert.ToInt32(idAlternativa);

                i++;
            }
            conexaoComBanco.Close();

            return resultado;
        }

        public ValidationResult Editar(Questao questao)
        {
            var validador = new ValidadorQuestao();

            var resultado = validador.Validate(questao);

            if (!resultado.IsValid)
                return resultado;

            SqlConnection conexaoComBanco = new(enderecoBanco);

            SqlCommand comandoEdicaoQuestao = new(sqlEditarQuestao, conexaoComBanco);

            SqlCommand comandoEdicaoAlternativa = new(sqlEditarAlternativa, conexaoComBanco);

            ConfigurarParametrosQuestao(questao, comandoEdicaoQuestao);

            conexaoComBanco.Open();

            foreach (var alternativa in questao.Alternativas)
            {
                comandoEdicaoAlternativa.Parameters.Clear();
                ConfirugarParametrosAlternativas(alternativa, questao, comandoEdicaoAlternativa);
                comandoEdicaoAlternativa.ExecuteNonQuery();
            }

           
            comandoEdicaoQuestao.ExecuteNonQuery(); // Edita aqui
            conexaoComBanco.Close();

            return resultado;
        }

        public ValidationResult Excluir(Questao questaoSelecionada)
        {
            
            SqlConnection conexaoComBanco = new(enderecoBanco);

            SqlCommand comandoExclusaoAlternativas = new(sqlExcluirAlternativas, conexaoComBanco);

            SqlCommand comandoExclusaoQuestao = new(sqlExcluirQuestao, conexaoComBanco);


            comandoExclusaoAlternativas.Parameters.AddWithValue("NUMERO", questaoSelecionada.Numero);

            comandoExclusaoQuestao.Parameters.AddWithValue("NUMERO", questaoSelecionada.Numero);


            conexaoComBanco.Open();

            

            var resultado = new ValidationResult();

           
                comandoExclusaoAlternativas.ExecuteNonQuery();

            int numeroRegistrosExcluidos = comandoExclusaoQuestao.ExecuteNonQuery(); // Exclui aqui
            comandoExclusaoAlternativas.ExecuteNonQuery();

            conexaoComBanco.Close();

            return resultado;
            

        }



        public Questao SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", numero);

            conexaoComBanco.Open();

            SqlDataReader leitor = comandoSelecao.ExecuteReader();

            Questao Questao = new();

            if (leitor.Read())
                Questao = ConverterParaQuestao(leitor);

            conexaoComBanco.Close();

            return Questao;
        }

        public List<Questao> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();

            SqlDataReader leitor = comandoSelecao.ExecuteReader(); 

            List<Questao> questoes = new List<Questao>();

            while (leitor.Read())
            {
                Questao questao = ConverterParaQuestao(leitor);
                questoes.Add(questao);
            }

            return questoes;
        }





        //Métodos privados



        private void ConfigurarParametrosQuestao(Questao questao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("NUMERO", questao.Numero);
            comando.Parameters.AddWithValue("ENUNCIADO", questao.Enunciado);
            comando.Parameters.AddWithValue("MATERIA_NUMERO", questao.Materia.Numero);
            comando.Parameters.AddWithValue("DISCIPLINA_NUMERO", questao.Disciplina.Numero);
        }


        private void ConfirugarParametrosAlternativas(Alternativa alternativa, Questao questao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("NUMERO", alternativa.Numero);
            comando.Parameters.AddWithValue("DESCRICAO", alternativa.Descricao);
            comando.Parameters.AddWithValue("CORRETA", alternativa.estaCorreta);
            comando.Parameters.AddWithValue("QUESTAO_NUMERO", questao.Numero);
        }






        private Questao ConverterParaQuestao(SqlDataReader leitorQuestao)
        {
           

            int numero = Convert.ToInt32(leitorQuestao["NUMERO"]); // Isso vem da área 'Select...' dos comando SQL Sel. Todos/Por número
            string enunciado = Convert.ToString(leitorQuestao["ENUNCIADO"]);

            Disciplina disciplina = new();
            disciplina.Numero = Convert.ToInt32(leitorQuestao["DISCIPLINA_NUMERO"]);
            disciplina.Nome = Convert.ToString(leitorQuestao["NOME_DISCIPLINA"]);

            Materia materia = new();
            materia.Numero = Convert.ToInt32(leitorQuestao["MATERIA_NUMERO"]);
            materia.Nome = Convert.ToString(leitorQuestao["NOME_MATERIA"]);

            return new Questao
            {
                Numero = numero,
                Enunciado = enunciado,
                Disciplina = disciplina,
                Materia = materia
            };
        }


        private Alternativa ConverterParaAlternativaQuestao(SqlDataReader leitorAlternativaQuestao)
        {
            var descricao = Convert.ToString(leitorAlternativaQuestao["DESCRICAO"]);
            var correta = Convert.ToBoolean(leitorAlternativaQuestao["CORRETA"]);

            var alternativaQuestao = new Alternativa
            {
                Descricao = descricao,
                estaCorreta = correta
            };

            return alternativaQuestao;
        }

        
    }
}
