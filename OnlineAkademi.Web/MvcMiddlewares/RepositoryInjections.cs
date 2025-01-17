﻿using OnlineAkademi.Core.Repositories;
using OnlineAkademi.Data.Sql.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAkademi.Web.MvcMiddlewares
{
    public static class RepositoryInjections
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return
                services
                    .AddScoped<ICategoryRepository, CategoryRepository>()
                    .AddScoped<ICourseRepository,CourseRepository>()
                    .AddScoped<ITrainerRepository,TrainerRepository>()
                    .AddScoped<IMaterialRepository, MaterialRepository>()
                    .AddScoped<IStudentRepository, StudentRepository>()
                    .AddScoped<IErrorRepository, ErrorRepository>()
                    .AddScoped<ICrudLoggerRepository, CrudLoggerRepository>();
        }
    }
}
