using System;
using System.Collections.Generic;
using System.Linq;
using CH.Dependency.Dispatchers;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace CH.Dependency.Modules
{
    public class QueryCommandModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IQueryDispatcher>().To<QueryDispatcher>();
            Bind<ICommandDispatcher>().To<CommandDispatcher>();

            Kernel.Bind(x => x
                .From(AppDomain.CurrentDomain.GetAssemblies())
                .SelectAllClasses()
                .InheritedFrom(typeof(IQueryHandler<,>))
                .BindAllInterfaces()
            );

            Kernel.Bind(x => x
                .From(AppDomain.CurrentDomain.GetAssemblies())
                .SelectAllClasses()
                .InheritedFrom(typeof(ICommandHandler<>))
                .BindAllInterfaces()
            );
        }
    }
}
