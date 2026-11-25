using MedoraClinic.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace MedoraClinic.API.Extensions;

public static class ApiResponseExtensions
{
    public static IActionResult ToActionResult<T>(this ControllerBase controller,ApiResponse<T> response,int? successStatusCode = null)
    {
        var statusCode = response.Succeeded
            ? successStatusCode ?? response.StatusCode
            : response.StatusCode;

        return controller.StatusCode(statusCode, response);
    }
}
