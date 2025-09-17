using System.ComponentModel;

namespace Avvo.Domain.Enums;

public enum TaxRegimeCode
{
    [Description("Simples Nacional")]
    SimplesNacional = 1,

    [Description("Simples Nacional - Excesso de Sublimite da Receita Bruta")]
    SimplesNacionalExcessoSublimite = 2,

    [Description("Regime Normal")]
    RegimeNormal = 3,

    [Description("Simples Nacional - Microempreendedor Individual - MEI")]
    MicroempreendedorIndividual = 4
}
