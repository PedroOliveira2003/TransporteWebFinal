using System.Collections.Generic;

namespace TransporteWeb.Models
{
    public class MotoristaAgendamentoViewModel
    {
        public IEnumerable<Agendamento> Agendamentos { get; set; }
        public IEnumerable<ConfirmacaoPresenca> Confirmacoes { get; set; }
    }
}