﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Caliburn.Micro;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.ViewModels;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace
{
    public class AppBootstrapper : Bootstrapper<IShell>
    {
        CompositionContainer container;

        protected override void Configure()
        {
            ConfigureContainer();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Count() > 0)
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        void ConfigureContainer()
        {
            container = CompositionHost.Initialize(
                new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue<UploadPhotoManagerViewModel>(new UploadPhotoManagerViewModel());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override void OnUnhandledException(object sender, System.Windows.ApplicationUnhandledExceptionEventArgs e)
        {
            
            MessageBoxViewModel message = new MessageBoxViewModel();
            message.DisplayName = AppStrings.ApplicationErrorWindowTitle;
            message.Header = ErrorStrings.GenericErrorMessageHeader;
            message.Message = ErrorStrings.GenericErrorMessage;
            message.Buttons = MessageBoxButtons.OK;
            message.MessageLevel = MessageLevel.Error;
            IoC.Get<IWindowManager>().ShowDialog(message);
            e.Handled = true;
        }
    }
}
