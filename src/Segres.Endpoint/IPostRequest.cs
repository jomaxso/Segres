using Microsoft.AspNetCore.Http;
using Segres.Contracts;

namespace Segres.Endpoint;

public interface IPostRequest : ICommand<IResult>
{
}