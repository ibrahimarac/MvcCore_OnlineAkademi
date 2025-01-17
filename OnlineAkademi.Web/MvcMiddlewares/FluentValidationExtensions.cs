﻿using FluentValidation;
using FluentValidation.AspNetCore;
using OnlineAkademi.Web.Models.VM;
using OnlineAkademi.Web.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAkademi.Web.MvcMiddlewares
{
    public static class FluentValidationExtensions
    {
        public static IServiceCollection AddFluentValidators(this IServiceCollection services)
        {
            return
                services
                    .AddFluentValidation()
                    .AddTransient<IValidator<CategoryVM>, CategoryValidator>()
                    .AddTransient<IValidator<CourseVM>, CourseValidator>()
                    .AddTransient<IValidator<TrainerVM>, TrainerValidator>()
                    .AddTransient<IValidator<TrainerEditVM>, TrainerEditValidator>()
                    .AddTransient<IValidator<MaterialVM>,MaterialValidator>()
                    .AddTransient<IValidator<LoginVM>, LoginValidator>()
                    .AddTransient<IValidator<StudentVM>, StudentValidator>();
        }
    }
}
