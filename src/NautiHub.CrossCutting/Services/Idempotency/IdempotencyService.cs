using Microsoft.EntityFrameworkCore;
using NautiHub.CrossCutting.Services.Idempotency;
using NautiHub.Infrastructure.DataContext;
using NautiHub.Infrastructure.Entities;

namespace NautiHub.CrossCutting.Services.Idempotency;

public class IdempotencyService : IIdempotencyService
{
    private readonly DatabaseContext _context;

    public IdempotencyService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> IsAlreadyProcessedAsync(Guid requestId) => await _context.ProcessCommand.AnyAsync(x => x.Id == requestId);

    public async Task MarkAsProcessedAsync(Guid requestId)
    {
        _context.ProcessCommand.Add(new IdempotentCommand { Id = requestId });
        await _context.SaveChangesAsync();
    }
}
