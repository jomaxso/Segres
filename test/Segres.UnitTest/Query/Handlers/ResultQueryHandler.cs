using Segres;
using Segres.Handlers;
using Xunit.Sdk;

namespace Segres.UnitTest.Query;

public class ResultQueryHandler : IQueryHandler<ResultQuery, string>
{
    public async Task<string> HandleAsync(ResultQuery query, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return query.Number switch
        {
            0 => "Zero",
            > 0 => throw new IndexOutOfRangeException(),
            < 0 => throw new NotEmptyException()
        };
    }
}