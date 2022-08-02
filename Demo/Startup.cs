// using Demo.Endpoints.Authentication.User;
// using Demo.Endpoints.PrintToConsole;
// using MicrolisR;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Demo;
//
// /// <summary>
// /// </summary>
// public class HttpContextRequestResolver_PrintToConsoleHandler : IHttpContextRequestResolver<PrintRequest, PrintResult>
// {
//     private readonly bool _shouldValidate;
//     public IRequestHandler<PrintRequest, PrintResult> RequestHandler { get; }
//
//     public HttpContextRequestResolver_PrintToConsoleHandler(IRequestHandler<PrintRequest, PrintResult> requestHandler)
//     {
//         RequestHandler = requestHandler;
//         _shouldValidate = ((IHttpContextRequestResolver) this).Validate;
//     }
//
//     public Delegate EndpointDelegate => ([FromServices] IMediator mediator, [FromRoute] int value, CancellationToken cancellationToken) =>
//         mediator.SendAsync(new PrintRequest() {Value = value}, _shouldValidate, cancellationToken);
// }
//
// public class HttpContextRequestResolver_GetUserAuthenticationHandler : IHttpContextRequestResolver<GetUserAuthenticationRequest, UserAuthenticationResponse>
// {
//     private readonly bool _shouldValidate;
//     public IRequestHandler<GetUserAuthenticationRequest, UserAuthenticationResponse> RequestHandler { get; }
//
//     public HttpContextRequestResolver_GetUserAuthenticationHandler(IRequestHandler<GetUserAuthenticationRequest, UserAuthenticationResponse> requestHandler)
//     {
//         RequestHandler = requestHandler;
//         _shouldValidate = ((IHttpContextRequestResolver) this).Validate;
//     }
//
//     public Delegate EndpointDelegate =>
//         ([FromServices] IMediator mediator, [FromRoute] int value, CancellationToken cancellationToken) =>
//         mediator.SendAsync(new GetUserAuthenticationRequest() {Value = value}, _shouldValidate, cancellationToken);
// }
//
// public class HttpContextRequestResolver_GetAllAuthenticationsHandler : IHttpContextRequestResolver<GetAllAuthenticationsRequest, List<UserAuthenticationResponse>>
// {
//     private readonly bool _shouldValidate;
//     public IRequestHandler<GetAllAuthenticationsRequest, List<UserAuthenticationResponse>> RequestHandler { get; }
//
//     public HttpContextRequestResolver_GetAllAuthenticationsHandler(IRequestHandler<GetAllAuthenticationsRequest, List<UserAuthenticationResponse>> requestHandler)
//     {
//         RequestHandler = requestHandler;
//         _shouldValidate = ((IHttpContextRequestResolver) this).Validate;
//     }
//
//     public Delegate EndpointDelegate => ([FromServices] IMediator mediator, CancellationToken cancellationToken) =>
//         mediator.SendAsync(new GetAllAuthenticationsRequest(), _shouldValidate, cancellationToken);
// }
//
// // public class HttpContextRequestResolver_GetAllAuthenticationsHandler : IHttpContextRequestResolver<GetAllAuthenticationsRequest, List<UserAuthenticationResponse>>
// // {
// //     public Delegate EndpointDelegate => ([FromServices] ISender sender, CancellationToken cancellationToken) => sender.SendAsync(new GetAllAuthenticationsRequest(), cancellationToken);
// // }