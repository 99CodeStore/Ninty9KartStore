﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NsdcTraingPartnerHub.Service.Models;
using Serilog;

namespace NsdcTraingPartnerHub.Service.Configuration
{
    public static class ServiceExtension
    {
        public static void ConfigureDefaultIdentity(this IServiceCollection services)
        {

        }

        public static void ConfigureIdentity(this IServiceCollection serviceCollection)
        {
        
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseExceptionHandler(
                e =>
                {
                    e.Run(async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = "application/json";
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            Log.Error($"Something went wrong in the {contextFeature.Error}");
                            await context.Response.WriteAsync(new Error
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Internal server error. Please try again later."
                            }.ToString());

                        }
                    });
                });
        }



    }
}
