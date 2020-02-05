using AgregatorLinkow.Data.Interfaces;
using AgregatorLinkow.Data.Repositories;
using Autofac;
using Microsoft.AspNetCore.Http;

namespace AgregatorLinkow
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            builder.RegisterType<LinkRepository>()
                .As<ILinkRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<VoteRepository>()
                .As<IVoteRepository>()
                .InstancePerLifetimeScope();
        }
    }
}