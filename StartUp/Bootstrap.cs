using Aplikacija.Commands;
using Aplikacija.Models;
using Aplikacija.ViewModels;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aplikacija.StartUp
{
    public class Bootstrap
    {
        private ContainerBuilder builder;
        private Autofac.IContainer container;
        private static Bootstrap instance;

        public Bootstrap()
        {
            builder = new ContainerBuilder();

            builder.RegisterType<RelayCommand>().As<ICommand>();

            builder.RegisterType<MainViewModel>().As<IMainViewModel>();
            builder.RegisterType<PodaciService>().As<IPodaciService>();
            builder.RegisterType<MainWindow>().AsSelf();
        }
        public static Bootstrap Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bootstrap();
                }
                return instance;
            }
        }

        public Autofac.IContainer Bootstraper()
        {
            return builder.Build();
        }

        public Autofac.IContainer Container
        {
            get
            {
                return container;
            }
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public void Build()
        {
            container = builder.Build();
        }

    }
}
