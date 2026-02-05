using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;

namespace NautiHub.Core.Messages.Features;

public class FeatureHandler
{
    protected readonly IUnitOfWork? _unitOfWork;
    protected ValidationResult ValidationResult;

    public FeatureHandler() => ValidationResult = new();

    public FeatureHandler(IUnitOfWork unitOfWork)
        : this()
    {
        _unitOfWork = unitOfWork;
    }

    protected void AddError(string message) => ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));

    protected bool HasError() => !ValidationResult.IsValid;

    protected async Task<bool> SaveChanges(
        bool treatSimultaneity = false,
        bool activateSoftDelete = true
    )
    {
        if (_unitOfWork == null)
        {
            throw new InvalidOperationException(
                "PersistirDados não pode ser chamado sem um IUnitOfWork."
            );
        }

        if (!treatSimultaneity)
        {
            await _unitOfWork.CommitAsync(activateSoftDelete);
            return true;
        }

        try
        {
            await _unitOfWork.CommitAsync(activateSoftDelete);
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!treatSimultaneity)
            {
                throw;
            }

            ex.UndoChangesToEntities();
            return false;
        }
        catch
        {
            throw;
        }
    }
}
