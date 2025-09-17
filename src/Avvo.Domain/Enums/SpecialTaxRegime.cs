using System.ComponentModel;

namespace Avvo.Domain.Enums;

public enum SpecialTaxRegime
{
    [Description("Nenhum")]
    Nenhum = 0,

    [Description("Micro Empresa Municipal")]
    MicroEmpresaMunicipal = 1,

    [Description("Estimativa")]
    Estimativa = 2,

    [Description("Sociedade de Profissionais")]
    SociedadeProfissionais = 3,

    [Description("Cooperativa")]
    Cooperativa = 4,

    [Description("Microempreendedor Individual - MEI")]
    MicroempreendedorIndividual = 5,

    [Description("Microempresa ou Empresa de Pequeno Porte - ME/EPP")]
    MicroempresaOuEpp = 6,

    [Description("Lucro Real")]
    LucroReal = 7,

    [Description("Lucro Presumido")]
    LucroPresumido = 8,

    [Description("Tributação Normal")]
    TributacaoNormal = 9,

    [Description("Simples Nacional com excesso do sublimite")]
    SimplesNacionalExcessoSublimite = 10,

    [Description("Empresa de Responsabilidade Limitada")]
    EmpresaResponsabilidadeLimitada = 11
}
