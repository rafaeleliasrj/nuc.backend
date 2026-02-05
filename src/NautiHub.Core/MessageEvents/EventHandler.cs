using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Data;
using NautiHub.Core.Extensions;

namespace NautiHub.Core.MessageEvents;

public abstract class EventHandler<TEventBasic>(IUnitOfWork unitOfWork)
    where TEventBasic : Event
{
    protected internal readonly IUnitOfWork _unitOfWork = unitOfWork;

    public abstract Task OnExecuteConsume(TEventBasic context);

    protected async Task<bool> SaveChanges(
        bool tratarSimultaneidade = false,
        bool ativarSoftDelete = true
    )
    {
        if (!tratarSimultaneidade)
        {
            await _unitOfWork.CommitAsync(ativarSoftDelete);
            return true;
        }

        try
        {
            await _unitOfWork.CommitAsync(ativarSoftDelete);
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!tratarSimultaneidade)
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
