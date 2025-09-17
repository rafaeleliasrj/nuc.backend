using System.ComponentModel;

namespace Avvo.Domain.Enums;

public enum SimplesNationalOption
{
    [Description("Não Optante")]
    NaoOptante = 0,

    [Description("Optante pelo Simples Nacional")]
    Optante = 1,

    [Description("Excluído do Simples Nacional")]
    Excluido = 2,

    [Description("Excesso de Sublimite da Receita Bruta")]
    ExcessoSublimite = 3
}
