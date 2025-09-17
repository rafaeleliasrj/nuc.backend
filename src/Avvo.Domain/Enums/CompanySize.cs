using System.ComponentModel;

namespace Avvo.Domain.Enums;

public enum CompanySize
{
    [Description("MEI – Microempreendedor Individual")]
    MEI = 1,

    [Description("Microempresa (ME)")]
    Microempresa = 2,

    [Description("Empresa de Pequeno Porte (EPP)")]
    EmpresaPequenoPorte = 3,

    [Description("Média Empresa")]
    MediaEmpresa = 4,

    [Description("Grande Empresa")]
    GrandeEmpresa = 5,

    [Description("Demais")]
    Demais = 6,
}
