using GeradorTestes.Dominio.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorTestes.Dominio.ModuloQuestao
{
    [Serializable]
    public class Alternativa 
    {
        public string Descricao { get; set; }
        public bool estaCorreta{ get; set; }

        public Alternativa(string descricao)
        {
            Descricao = descricao;
            this.estaCorreta = false;
        }

        public override string ToString()
        {
            string status = estaCorreta == true ? "Correta" : "Falsa";
            return $"Alternativa: {Descricao} | {status}";
        }

        public void atualizarAlternativa(Alternativa alternativa)
        {
            this.Descricao = alternativa.Descricao;
            this.estaCorreta = alternativa.estaCorreta;
        }
    }
}
