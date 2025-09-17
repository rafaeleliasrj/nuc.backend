using System.ComponentModel;

namespace Avvo.Domain.Enums;

public enum SimplesNationalTaxCalculationRegime
{
    [Description("Regime de apuração dos tributos federais e municipais pelo SN")]
    FederalMunicipal = 1,

    [Description("Regime de apuração dos tributos federais e municipal pela NFS-e conforme respectivas legislações federal e municipal de cada tributo")]
    FederalMunicipalNfse = 2,

    [Description("Regime de apuração dos tributos federais pelo SN e o ISSQN pela NFS-e conforme respectiva legislação municipal do tributo")]
    FederalISSQNNfse = 3
}
