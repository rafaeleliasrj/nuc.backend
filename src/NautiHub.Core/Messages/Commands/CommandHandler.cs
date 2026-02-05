using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;

namespace NautiHub.Core.Messages.Commands;

public class CommandHandler(IUnitOfWork unitOfWork)
{
    protected internal readonly IUnitOfWork _unitOfWork = unitOfWork;

    protected ValidationResult ValidationResult = new();

    protected void AddError(string message) => ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));

    protected bool HasError() => !ValidationResult.IsValid;

    protected async Task<bool> SaveChanges(
        bool treatSimultaneity = false,
        bool activateSoftDelete = true
    )
    {
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
        catch (Exception)
        {
            throw;
        }
    }
}
