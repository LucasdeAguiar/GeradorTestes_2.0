using FluentValidation.Results;
using GeradorTestes.Dominio.Compartilhado;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Dominio.ModuloTeste;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorTestes.Infra.BancoDados.ModuloTeste
{
    public class RepositorioTesteEmBancoDados : IRepositorioTeste
    {
        private const string enderecoBanco =
            "Data Source=(LocalDB)\\MSSqlLocalDB;" +
            "Initial Catalog=GeradorTestesDb;" +
            "Integrated Security=True;" +
            "Pooling=False";


        #region SQL Queries Teste

        private const string sqlInserirTeste =
           @"INSERT INTO [TB_TESTE]
                 (
				    [TITULO],
				    [DISCIPLINA_NUMERO],
				    [MATERIA_NUMERO],
                    [DATA]
			    )
            VALUES
                (
				    @TITULO,
				    @DISCIPLINA_NUMERO,
				    @MATERIA_NUMERO,
                    @DATA
			); SELECT SCOPE_IDENTITY();";

        private const string sqlEditarTeste =
             @"UPDATE TB_TESTE
                SET
                    TITULO = @TITULO,
                    DISCIPLINA_NUMERO = @DISCIPLINA_NUMERO,
                    MATERIA_NUMERO = @MATERIA_NUMERO,
                    DATA = @DATA
                WHERE
                    NUMERO = @NUMERO";

        private const string sqlExcluirTeste =
            @"DELETE FROM [TB_TESTE]
		        WHERE
			        [NUMERO] = @NUMERO";

        private const string sqlSelecionarTodos =
            @"SELECT
                T.[NUMERO],
	            T.[TITULO],
	            T.[DATA],
	            D.[NUMERO] AS DISCIPLINA_NUMERO,
                D.[NOME] AS DISCIPLINA_NOME,
	            MT.[NUMERO] AS MATERIA_NUMERO,
	            MT.[NOME] AS MATERIA_NOME,
	            MT.[SERIE] AS MATERIA_SERIE
            FROM
                TB_TESTE AS T LEFT JOIN TBDISCIPLINA AS D
            ON
                T.DISCIPLINA_NUMERO = D.NUMERO
                LEFT JOIN TBMATERIA AS MT
            ON
                T.MATERIA_NUMERO = MT.NUMERO";

        private const string sqlSelecionarPorNumero =
            @"SELECT
                T.[NUMERO],
	            T.[TITULO],
	            T.[DATA],
	            D.[NUMERO] AS DISCIPLINA_NUMERO,
                D.[NOME] AS DISCIPLINA_NOME,
	            MT.[NUMERO] AS MATERIA_NUMERO,
	            MT.[NOME] AS MATERIA_NOME,
	            MT.[SERIE] AS MATERIA_SERIE
            FROM
                TB_TESTE AS T LEFT JOIN TBDISCIPLINA AS D
            ON
                T.DISCIPLINA_NUMERO = D.NUMERO
                LEFT JOIN TBMATERIA AS MT
            ON
                T.MATERIA_NUMERO = MT.NUMERO
            WHERE
                T.NUMERO = @NUMERO";


        private const string sqlSelecionarAlternativasDaQuestao =
            @"SELECT 
                [NUMERO],
                [DESCRICAO],
                [CORRETA],
                [QUESTAO_NUMERO]
            FROM
                [TB_ALTERNATIVA]
            
            WHERE 
                [QUESTAO_NUMERO] = @QUESTAO_NUMERO";


        #endregion

        #region SQL Queries Teste_Questao

        private const string sqlAdicionarQuestaoNoTeste =
            @"INSERT INTO[TB_TESTE_TB_QUESTAO]
                (
                    [TESTE_NUMERO],
                    [QUESTAO_NUMERO]
                )
                VALUES
                (
                    @TESTE_NUMERO,
                    @QUESTAO_NUMERO
                )";

        private const string sqlSelecionarQuestoesDoTeste =
            @"SELECT 
	                Q.[NUMERO], 
	                Q.[ENUNCIADO],
	                Q.[MATERIA_NUMERO],
	                Q.[DISCIPLINA_NUMERO],
	                D.[NOME] AS DISCIPLINA_NOME,
                    
                    MT.[NUMERO] AS MATERIA_NUMERO,
                    MT.[NOME] AS MATERIA_NOME
            FROM 
	                TB_QUESTAO AS Q INNER JOIN TB_TESTE_TB_QUESTAO AS TQ 
                ON 
	                Q.NUMERO = TQ.QUESTAO_NUMERO
                    INNER JOIN TBDISCIPLINA AS D
                ON
                    Q.[DISCIPLINA_NUMERO] = D.NUMERO
                    INNER JOIN TBMATERIA AS MT
                ON
                    MT.[NUMERO] = Q.MATERIA_NUMERO
                WHERE 
	                TQ.TESTE_NUMERO = @NUMERO";

        private const string sqlRemoverQuestaoDoTeste =
            @" DELETE FROM
                TB_TESTE_TB_QUESTAO
            WHERE
                [TESTE_NUMERO] = @TESTE_NUMERO";
        #endregion


        public ValidationResult Inserir(Teste novoRegistro)
        {
            var validador = new ValidadorTeste();

            var resultadoValidacao = validador.Validate(novoRegistro);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserirTeste, conexaoComBanco);

            ConfigurarParametrosTeste(novoRegistro, comandoInsercao);

            conexaoComBanco.Open();

            var id = comandoInsercao.ExecuteScalar();

            novoRegistro.Numero = Convert.ToInt32(id);

            foreach (var questao in novoRegistro.questoes)
                AdicionarQuestao(novoRegistro, questao);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Teste teste)
        {
            var validador = new ValidadorTeste();

            var resultado = validador.Validate(teste);

            if (!resultado.IsValid)
                return resultado;

            SqlConnection conexaoComBanco = new(enderecoBanco);

            SqlCommand comandoEdicaoQuestao = new(sqlEditarTeste, conexaoComBanco);

            

            ConfigurarParametrosTeste(teste, comandoEdicaoQuestao);

            conexaoComBanco.Open();

            foreach (var alternativa in teste.questoes)
            {
                comandoEdicaoQuestao.Parameters.Clear();
              //  ConfirugarParametrosQuestoes(alternativa, questao, comandoEdicaoQuestao);
                comandoEdicaoQuestao.ExecuteNonQuery();
            }


            comandoEdicaoQuestao.ExecuteNonQuery(); 
            conexaoComBanco.Close();

            return resultado;
        }

        public ValidationResult Excluir(Teste registro)
        {
            RemoverQuestao(registro);

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluirTeste, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("NUMERO", registro.Numero);

            conexaoComBanco.Open();

            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o Registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public Teste SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", numero);

            conexaoComBanco.Open();
            SqlDataReader leitorTeste = comandoSelecao.ExecuteReader();

            Teste teste = null;

            if (leitorTeste.Read())
                teste = ConverterParaTeste(leitorTeste);

            conexaoComBanco.Close();

            CarregarQuestoesTeste(teste);

            return teste;
        }


        public List<Teste> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();

            SqlDataReader leitorTeste = comandoSelecao.ExecuteReader();

            List<Teste> testes = new List<Teste>();

            while (leitorTeste.Read())
            {
                Teste teste = ConverterParaTeste(leitorTeste);

                CarregarQuestoesTeste(teste);

                testes.Add(teste);
            }

            conexaoComBanco.Close();

            return testes;
        }


        


        //Métodos Privados

        private void ConfigurarParametrosTeste(Teste teste, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("TITULO", teste.Titulo);
            comando.Parameters.AddWithValue("DISCIPLINA_NUMERO", teste.Disciplina.Numero);
            comando.Parameters.AddWithValue("MATERIA_NUMERO", teste.Materia != null ? teste.Materia.Numero : DBNull.Value);
            comando.Parameters.AddWithValue("DATA", teste.Data);
        }


        private Teste ConverterParaTeste(SqlDataReader leitorTeste)
        {
            var numeroTeste = Convert.ToInt32(leitorTeste["NUMERO"]);
            var titulo = Convert.ToString(leitorTeste["TITULO"]);
            var data = Convert.ToDateTime(leitorTeste["DATA"]);

            var numeroDisciplina = Convert.ToInt32(leitorTeste["DISCIPLINA_NUMERO"]);
            var nomeDisciplina = Convert.ToString(leitorTeste["DISCIPLINA_NOME"]);

            Disciplina disciplina = new Disciplina();
            disciplina.Numero = numeroDisciplina;
            disciplina.Nome = nomeDisciplina;

            var teste = new Teste
            {
                Numero = numeroTeste,
                Titulo = titulo,
                Data = data,
                Disciplina = disciplina
            };

            if (leitorTeste["MATERIA_NUMERO"] != DBNull.Value)
            {
                teste.Materia = new Materia();
                teste.Materia.Numero = Convert.ToInt32(leitorTeste["MATERIA_NUMERO"]);
                teste.Materia.Nome = Convert.ToString(leitorTeste["MATERIA_NOME"]);
                teste.Materia.Disciplina = disciplina;
                teste.Materia.Serie = Convert.ToString(leitorTeste["MATERIA_SERIE"]);
            }

            return teste;
        }




        #region bloco questão


        private void AdicionarQuestao(Teste teste, Questao questao)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlAdicionarQuestaoNoTeste, conexaoComBanco);

            comandoInsercao.Parameters.AddWithValue("TESTE_NUMERO", teste.Numero);
            comandoInsercao.Parameters.AddWithValue("QUESTAO_NUMERO", questao.Numero);

            conexaoComBanco.Open();
            comandoInsercao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        private void RemoverQuestao(Teste teste)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlRemoverQuestaoDoTeste, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("TESTE_NUMERO", teste.Numero);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        private void CarregarQuestoesTeste(Teste teste)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarQuestoesDoTeste, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", teste.Numero);

            conexaoComBanco.Open();

            SqlDataReader leitorQuestao = comandoSelecao.ExecuteReader();

            while (leitorQuestao.Read())
            {
                Questao questao = ConverterParaQuestao(leitorQuestao);

                CarregarAlternativas(questao);

                teste.questoes.Add(questao);
            }

            conexaoComBanco.Close();
        }

        

        private Questao ConverterParaQuestao(SqlDataReader leitorQuestao)
        {
            var numero = Convert.ToInt32(leitorQuestao["NUMERO"]);
            var enunciado = Convert.ToString(leitorQuestao["ENUNCIADO"]);

            var numeroDisciplina = Convert.ToInt32(leitorQuestao["DISCIPLINA_NUMERO"]);
            var nomeDisciplina = Convert.ToString(leitorQuestao["DISCIPLINA_NOME"]);

            var numeroMateria = Convert.ToInt32(leitorQuestao["MATERIA_NUMERO"]);
            var nomeMateria = Convert.ToString(leitorQuestao["MATERIA_NOME"]);


            Disciplina disciplina = new Disciplina();
            disciplina.Numero = numeroDisciplina;
            disciplina.Nome = nomeDisciplina;

            Materia materia = new Materia();
            materia.Numero = numeroMateria;
            materia.Nome = nomeMateria;
            materia.Disciplina = disciplina;

            var questao = new Questao
            {
                Numero = numero,
                Enunciado = enunciado,
                Materia = materia
            };


            return questao;
        }

        #endregion


        #region bloco alternativa

        private void CarregarAlternativas(Questao questao)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarAlternativasDaQuestao, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("QUESTAO_NUMERO", questao.Numero);

            conexaoComBanco.Open();

            SqlDataReader leitorAlternativa = comandoSelecao.ExecuteReader();

            while (leitorAlternativa.Read())
            {
                Alternativa alternativa = ConverterParaAlternativa(leitorAlternativa);

                questao.AdicionarAlternativa(alternativa);
            }

            conexaoComBanco.Close();
        }

        private Alternativa ConverterParaAlternativa(SqlDataReader leitorAlternativa)
        {
            var numero = Convert.ToInt32(leitorAlternativa["NUMERO"]);
            var descricao = Convert.ToString(leitorAlternativa["DESCRICAO"]);
            var correta = Convert.ToBoolean(leitorAlternativa["CORRETA"]);

            var alternativa = new Alternativa
            {
                Numero = numero,
                Descricao = descricao,
                estaCorreta = correta,
            
            };

            return alternativa;
        }
        #endregion



    }
}
