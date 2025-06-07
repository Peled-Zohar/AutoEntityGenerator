using AutoEntityGenerator.Common.Interfaces;
using FakeItEasy;
using AutoEntityGenerator.UI.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoEntityGenerator.UI.Interaction;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.Validators;
using AutoEntityGenerator.UI.ViewModels;
using FluentValidation;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace AutoEntityGenerator.UI.Tests
{
    class ServicesExtensionsUnitTests
    {
        [Test]
        public void AddUI_ShouldRegisterCodeGeneratorServices()
        {
            var services = A.Fake<IServices>();

            services.AddUI();
            A.CallTo(() => services.AddSingleton<IUserInteraction, UserInteraction>()).MustHaveHappened();
            A.CallTo(() => services.AddSingleton<IEntityConfigurationWindowFactory, EntityConfigurationWindowFactory>()).MustHaveHappened();
            A.CallTo(() => services.AddSingleton<IDialogService, DialogService>()).MustHaveHappened();
            A.CallTo(() => services.AddTransient<SettingsViewModel>()).MustHaveHappened();
            A.CallTo(() => services.AddSingleton<IValidator<IAppSettings>, AppSettingslValidator>()).MustHaveHappened();
            
        }
    }
}
