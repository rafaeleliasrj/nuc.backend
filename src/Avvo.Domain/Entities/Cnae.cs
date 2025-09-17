using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Domain.Entities;

public class Cnae : BaseEntity, IAggregateRoot
{
    private Cnae() { }

    public Cnae(string code, string description, DateTime? startDate, DateTime? endDate)
    {
        UpdateCnae(code, description, startDate, endDate);
    }

    public string Code { get; private set; }
    public string Description { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public virtual ICollection<Business> Businesses { get; private set; }

    public void UpdateCnae(
        string code,
        string description,
        DateTime? startDate,
        DateTime? endDate)
    {
        Code = code;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }
}
