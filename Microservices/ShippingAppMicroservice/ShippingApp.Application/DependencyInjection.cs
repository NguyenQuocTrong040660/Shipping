using AutoMapper;
using ShippingApp.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ShippingApp.Application.Queries;
using ShippingApp.Application.Commands;

namespace ShippingApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            services.AddMediatR(typeof(GetAllProductQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetProductByIDQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetCountryQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetAllShippingPlanQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetShippingPlanByIDQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetCountryQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetCountryByNameQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetCountryByNameQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetCountryByIdQuery).GetTypeInfo().Assembly);

            services.AddMediatR(typeof(CreateProductCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateCountryCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateNewShippingPLanCommand).GetTypeInfo().Assembly);

            services.AddMediatR(typeof(UpdateCountryCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(UpdateShippingPlanCommand).GetTypeInfo().Assembly);


            return services;
        }
    }
}
