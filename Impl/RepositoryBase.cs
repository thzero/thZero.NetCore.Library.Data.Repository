﻿/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2021 thZero.com

<development [at] thzero [dot] com>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 * ------------------------------------------------------------------------- */

using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using thZero.Instrumentation;
using thZero.Responses;

namespace thZero.Data.Repository
{
    public abstract class RepositoryBase<TConfig> : IRepository
        where TConfig : class
    {
        public RepositoryBase(IOptions<TConfig> config)
        {
            Config = config.Value;
        }

        #region Protected Methods
        protected ErrorResponse Error()
        {
            return new ErrorResponse();
        }

        protected ErrorResponse Error(string message, params object[] args)
        {
            ErrorResponse error = new();
            error.AddError(message, args);
            return error;
        }

        protected TResult Error<TResult>(TResult result)
             where TResult : SuccessResponse
        {
            result.Success = false;
            return result;
        }

        protected TResult Error<TResult>(TResult result, string message, params object[] args)
             where TResult : SuccessResponse
        {
            result.AddError(message, args);
            result.Success = false;
            return result;
        }

        /// <summary>
        /// Get a service via service locator; this is an anti-pattern so use with caution.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object? GetService(IServiceProvider provider, Type type)
        {
            return provider ?? provider.GetService(type);
        }

        /// <summary>
        /// Get the instrumentation packet via service locator; this is an anti-pattern so use with caution.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected IInstrumentationPacket GetInstrumentationPacket(IServiceProvider provider)
        {
            return (IInstrumentationPacket)(provider ?? GetService(provider, typeof(IInstrumentationPacket)));
        }

        protected SuccessResponse Success()
        {
            return new SuccessResponse();
        }
        #endregion


        #region Protected Properties
        protected TConfig Config { get; private set; }
        #endregion
    }

    public abstract class RepositoryBase<TConfig, TRepository> : RepositoryBase<TConfig>, IRepository
        where TConfig : class
    {
        public RepositoryBase(IOptions<TConfig> config, ILogger<TRepository> logger) : base(config)
        {
            Logger = logger;
        }

        #region Protected Properties
        protected ILogger<TRepository> Logger { get; private set; }
        #endregion
    }
}