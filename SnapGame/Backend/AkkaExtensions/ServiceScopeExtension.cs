using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.AkkaExtensions
{
    public class ServiceScopeExtension : IExtension
    {
        private IServiceScopeFactory _serviceScopeFactory;

        public void Initialize(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IServiceScope CreateScope()
        {
            return _serviceScopeFactory.CreateScope();
        }
    }
}
