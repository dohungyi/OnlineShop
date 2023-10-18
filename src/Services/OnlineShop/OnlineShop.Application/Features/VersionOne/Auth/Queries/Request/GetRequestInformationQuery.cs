using MediatR;
using SharedKernel.Application;
using SharedKernel.Domain;
using SharedKernel.Libraries;

namespace OnlineShop.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class GetRequestInformationQuery : BaseQuery<ApiResult>
{
    
}