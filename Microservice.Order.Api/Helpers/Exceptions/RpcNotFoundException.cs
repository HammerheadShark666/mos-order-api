using Grpc.Core;

namespace Microservice.Order.Api.Helpers.Exceptions;

public class RpcNotFoundException(Status status) : RpcException(status)
{
}

