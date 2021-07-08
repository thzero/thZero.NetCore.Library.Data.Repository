/* ------------------------------------------------------------------------- *
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
            Enforce.AgainstNull(() => config);

            Config = config.Value;
            Enforce.AgainstNull(() => Config);
        }

        #region Protected Methods
        protected ErrorResponse Error(IInstrumentationPacket instrumentation = null)
        {
            return new ErrorResponse(instrumentation);
        }

        protected ErrorResponse Error(string message, params object[] args)
        {
            ErrorResponse error = new();
            error.AddError(message, args);
            return error;
        }

        protected ErrorResponse Error(IInstrumentationPacket instrumentation, string message, params object[] args)
        {
            ErrorResponse error = new(instrumentation);
            error.AddError(message, args);
            return error;
        }

        protected TResult Error<TResult>(TResult result)
             where TResult : SuccessResponse
        {
            result.Success = false;
            return result;
        }

        protected TResult Error<TResult>(IInstrumentationPacket instrumentation, TResult result)
             where TResult : SuccessResponse
        {
            result.Instrumentation = instrumentation;
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

        protected TResult Error<TResult>(IInstrumentationPacket instrumentation, TResult result, string message, params object[] args)
             where TResult : SuccessResponse
        {
            result.Instrumentation = instrumentation;
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

        protected bool IsSuccess(SuccessResponse response)
        {
            return (response != null) && response.Success;
        }

        protected SuccessResponse Success(IInstrumentationPacket instrumentation = null)
        {
            return new SuccessResponse(instrumentation);
        }

        protected SuccessResponse Success(bool success)
        {
            return new SuccessResponse(success);
        }

        protected SuccessResponse Success(IInstrumentationPacket instrumentation, bool success)
        {
            return new SuccessResponse(instrumentation, success);
        }

        protected SuccessResponse Success(bool success, string message)
        {
            SuccessResponse response = new(success);
            response.AddError(message);
            return response;
        }

        protected SuccessResponse Success(IInstrumentationPacket instrumentation, bool success, string message)
        {
            SuccessResponse response = new(instrumentation, success);
            response.AddError(message);
            return response;
        }
        #endregion


        #region Protected Properties
        protected TConfig Config { get; private set; }
        #endregion
    }

    public abstract class RepositoryBase<TRepository, TConfig> : RepositoryBase<TConfig>, IRepository
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