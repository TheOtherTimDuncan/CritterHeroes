using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Website.Services.Dispatchers;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace CH.Website.Dependency.Modules
{
    public class QueryCommandModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IQueryDispatcher>().To<QueryDispatcher>();
            Bind<ICommandDispatcher>().To<CommandDispatcher>();

            // Binding with AppDomain.CurrentDomain fails at runtime so it can't be used

            Kernel.Bind(x => x
                .FromAssemblyContaining(typeof(IQueryHandler<,>))
                .SelectAllClasses()
                .InheritedFrom(typeof(IQueryHandler<,>))
                .BindAllInterfaces()
            );

            Kernel.Bind(x => x
                .FromAssemblyContaining<MvcApplication>()
                .SelectAllClasses()
                .InheritedFrom(typeof(IQueryHandler<,>))
                .BindAllInterfaces()
            );

            Kernel.Bind(x => x
                .FromAssemblyContaining(typeof(IQueryHandler<,>))
                .SelectAllClasses()
                .InheritedFrom(typeof(ICommandHandler<,>))
                .BindAllInterfaces()
            );

            Kernel.Bind(x => x
                .FromAssemblyContaining<MvcApplication>()
                .SelectAllClasses()
                .InheritedFrom(typeof(ICommandHandler<,>))
                .BindAllInterfaces()
            );

            Kernel.Bind(x => x
                .FromAssemblyContaining(typeof(IQueryHandler<,>))
                .SelectAllClasses()
                .InheritedFrom(typeof(ICommandHandler<>))
                .BindAllInterfaces()
            );

            Kernel.Bind(x => x
                .FromAssemblyContaining<MvcApplication>()
                .SelectAllClasses()
                .InheritedFrom(typeof(ICommandHandler<>))
                .BindAllInterfaces()
            );
        }
    }
}
