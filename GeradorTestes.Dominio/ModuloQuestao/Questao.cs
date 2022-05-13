using GeradorTestes.Dominio.Compartilhado;
using GeradorTestes.Dominio.ModuloMateria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorTestes.Dominio.ModuloQuestao
{
    [Serializable]
    public class Questao : EntidadeBase<Questao>
    {
        public Materia Materia { get; set; }
        public string Enunciado { get; set; }

        public List<Alternativa> Alternativas { get; set; }

        public Questao()
        {
            this.Alternativas = new List<Alternativa>();
        }


        public override void atualizar(Questao questao)
        {
            this.Materia = questao.Materia;
            this.Enunciado = questao.Enunciado;
            this.Alternativas = questao.Alternativas;
        }

        public override string ToString()
        {
            return $"Número: {Numero} - Enunciado: {Enunciado} - Materia: {Materia.Nome}";
        }
    }
}
