﻿using DispatchR.Benchmarks.Contracts;

namespace DispatchR.Benchmarks.Handlers;

public class CommandHandler : ICommandHandler<Command>
{
    public Task HandleAsync(Command request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}