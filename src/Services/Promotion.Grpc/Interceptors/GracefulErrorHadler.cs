using Common.Exceptions;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Grpc.Core.Interceptors;

using grpc_core = Grpc.Core;
namespace Promotion.Grpc.Interceptors;

public class GracefulErrorHadler(ILogger<GracefulErrorHadler> logger, IHostEnvironment env) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException exc)
        {
            var errorDescriptions = exc.Errors
                .Select(err =>
                    new BadRequest.Types.FieldViolation
                    {
                        Field = err.PropertyName,
                        Description = err.ErrorMessage
                    }
                );
            Google.Rpc.Status richError = new()
            {
                Code = (int)Code.InvalidArgument,
                Message = exc.Message,
                Details =
                {
                    Any.Pack(new BadRequest
                    {
                        FieldViolations = { errorDescriptions }
                    })
                }
            };
            throw richError.ToRpcException();
        }
        catch (NotFoundException exc)
        {
            throw new RpcException(new (StatusCode.NotFound, exc.Message));
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "An error occured while processing gRPC request {request}", request);
            throw new RpcException(
                new grpc_core.Status(
                    StatusCode.Unknown, 
                    env.IsDevelopment()? exc.Message : string.Empty
                )
            );
        }
    }
}