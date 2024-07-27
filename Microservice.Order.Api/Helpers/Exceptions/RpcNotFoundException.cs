using Grpc.Core;

namespace Microservice.Order.Api.Helpers.Exceptions;

public class RpcNotFoundException : RpcException
{
    public RpcNotFoundException(Status status) : base(status)
    {
    }
}

