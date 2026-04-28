using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PropertyManagement.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.", ex.Message);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message, string? detail = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        if (detail == null)
        {
            await context.Response.WriteAsJsonAsync(new { error = message });
            return;
        }

        await context.Response.WriteAsJsonAsync(new { error = message, detail });
    }
}
