namespace NautiHub.Core.Utils;

public static class DateUtils
{
    private static void ValidarDiaData(int ano, int mes, int dia)
    {
        if (ano <= 0)
        {
            throw new Exception("Ano inválido");
        }

        if (mes <= 0)
        {
            throw new Exception("Mês inválido");
        }

        if (mes > 12)
        {
            throw new Exception("Mês inválido");
        }

        if (dia <= 0)
        {
            throw new Exception("Dia inválido");
        }

        if (dia > 31)
        {
            throw new Exception("Dia inválido");
        }
    }

    public static DateTime CriarDateTimeUtc(int ano, int mes, int dia)
    {
        ValidarDiaData(ano, mes, dia);

        var maximoDiaDoMes = DateTime.DaysInMonth(ano, mes);
        if (dia > maximoDiaDoMes)
        {
            dia = maximoDiaDoMes;
        }

        return new DateTime(ano, mes, dia, 0, 0, 0, 0, 0, DateTimeKind.Utc);
    }

    public static DateTime CriarDateTimeUtc(
        int ano,
        int mes,
        int dia,
        int hora,
        int minuto,
        int segundo,
        int milisegundo,
        int microsegundo
    )
    {
        ValidarDiaData(ano, mes, dia);

        var maximoDiaDoMes = DateTime.DaysInMonth(ano, mes);
        if (dia > maximoDiaDoMes)
        {
            dia = maximoDiaDoMes;
        }

        return new DateTime(
            ano,
            mes,
            dia,
            hora,
            minuto,
            segundo,
            milisegundo,
            microsegundo,
            DateTimeKind.Utc
        );
    }

    public static DateOnly CriarDateOnlyUtc(int ano, int mes, int dia)
    {
        ValidarDiaData(ano, mes, dia);

        var maximoDiaDoMes = DateTime.DaysInMonth(ano, mes);
        if (dia > maximoDiaDoMes)
        {
            dia = maximoDiaDoMes;
        }

        return new DateOnly(ano, mes, dia);
    }

    public static DateOnly ObterDateOnlyAtual()
    {
        DateTime dataDoDia = DateTime.UtcNow;
        return new DateOnly(dataDoDia.Year, dataDoDia.Month, dataDoDia.Day);
    }

    public static DateOnly GetDateOnlyFirstDayOfMonth(int ano, int mes)
    {
        return new DateOnly(ano, mes, 1);
    }

    public static DateTime GetDateTimeFirstDayOfMonth(int ano, int mes)
    {
        return new DateTime(ano, mes, 1, 0, 0, 0, 0, 0, DateTimeKind.Utc);
    }

    public static DateOnly GetDateOnlyLastDayOfMonth(int ano, int mes)
    {
        return new DateOnly(ano, mes, DateTime.DaysInMonth(ano, mes));
    }

    public static DateTime GetDateTimeLastDayOfMonth(int ano, int mes)
    {
        return new DateTime(
            ano,
            mes,
            DateTime.DaysInMonth(ano, mes),
            23,
            59,
            59,
            999,
            999,
            DateTimeKind.Utc
        );
    }

    public static DateOnly ObterDateOnlyFinal(int ano, int mes)
    {
        DateOnly dataAtual = ObterDateOnlyAtual();
        DateOnly dataFinal = GetDateOnlyLastDayOfMonth(ano, mes);
        if (dataFinal > dataAtual)
        {
            dataFinal = dataAtual;
        }

        return dataFinal;
    }

    public static DateTime ObterDateTimeFinal(int ano, int mes)
    {
        var dataAtual = new DateTime(
            ano,
            mes,
            DateTime.UtcNow.Day,
            23,
            59,
            59,
            999,
            999,
            DateTimeKind.Utc
        );
        DateTime dataFinal = GetDateTimeLastDayOfMonth(ano, mes);
        if (dataFinal > dataAtual)
        {
            dataFinal = dataAtual;
        }

        return dataFinal;
    }
}
