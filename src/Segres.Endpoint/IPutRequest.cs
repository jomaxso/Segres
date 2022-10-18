using Microsoft.AspNetCore.Http;
using Segres.Contracts;

namespace Segres.Endpoint;

public interface IPutRequest: ICommand<IResult>
{
}