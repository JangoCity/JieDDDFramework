﻿using System;
using System.Collections.Generic;
using System.Text;
using JieDDDFramework.Data.EntityFramework.AopConfigurations;
using JieDDDFramework.Data.EntityFramework.ModelConfigurations.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JieDDDFramework.Data.EntityFramework.ModelConfigurations
{
    public static class ModelConfigurationExtensions
    {
        public static IServiceCollection AddEFModelConfiguration(this IServiceCollection service,
            Action<ModelConfigurationOption> setupAction = null)
        {
            var option = new ModelConfigurationOption();
            setupAction?.Invoke(option);
            service.TryAddSingleton(option);
            if (setupAction != null)
            {
                service.Configure(setupAction);
            }
            else
            {
                service.Configure<ModelConfigurationOption>(_=>{});
            }
            service.TryAddTransient<IAutoApplyConfigurationService, DefaultAutoApplyConfigurationService>();
            service.TryAddTransient<IFixModelConfigurationService, DefaultFixModelConfigurationService>();
            service.TryAddTransient<IGlobalFilterService, DefaultGlobalFilterService>();
            service.TryAddTransient<IModelConfigurationProvider, DefaultModelConfigurationProvider>();
            return service;
        }
    }
}
