using AspNetCoreProblemDetailsDemo.Handlers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Adds services for using Problem Details format
//builder.Services.AddProblemDetails();


builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});



//builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Enable built-in Problem Details middleware that automatically
// converts unhandled exceptions into Problem Details responses
app.UseExceptionHandler();


// Enable centralized exception handling
//app.UseExceptionHandler("/error");

// Returns the Problem Details response for (empty) non-successful responses
app.UseStatusCodePages();


// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

// Map the error route
//app.Map("/error", (HttpContext httpContext) =>
//{
//    var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
//    var exception = exceptionHandlerFeature?.Error;

//    var problemDetails = new ProblemDetails
//    {
//        Status = httpContext.Response.StatusCode,
//        Title = "An error occurred while processing your request.",
//        Detail = exception?.Message,
//        Instance = httpContext.Request.Path
//    };

//    return Results.Problem(
//        detail: problemDetails.Detail,
//        instance: problemDetails.Instance,
//        statusCode: problemDetails.Status,
//        title: problemDetails.Title);
//});

app.Run();
