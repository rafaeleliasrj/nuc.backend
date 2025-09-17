using System.ComponentModel;

namespace Avvo.Domain.Enums;

public enum PaymentMethod
{
    [Description("Dinheiro")]
    Dinheiro = 1,

    [Description("Cheque")]
    Cheque = 2,

    [Description("Crédito")]
    CartaoCredito = 3,

    [Description("Débito")]
    CartaoDebito = 4,

    [Description("Crédito Loja")]
    CreditoLoja = 5,

    [Description("Vale Alimentação")]
    ValeAlimentacao = 10,

    [Description("Vale Refeição")]
    ValeRefeicao = 11,

    [Description("Vale Presente")]
    ValePresente = 12,

    [Description("Vale Combustível")]
    ValeCombustivel = 13,

    [Description("Duplicata Mercantil")]
    DuplicataMercantil = 14,

    [Description("Boleto Bancário")]
    Boleto = 15,

    [Description("Depósito Bancário")]
    Deposito = 16,

    [Description("PIX")]
    PIX = 17,

    [Description("Transferência Bancária, Carteira Digital")]
    Transferencia = 18,

    [Description("Programa de Fidelidade")]
    Fidelidade = 19,

    [Description("Sem Pagamento")]
    SemPagamento = 90,

    [Description("Outros")]
    Outros = 99
}
