using AutoEntityGenerator.Common.Interfaces;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEntityGenerator.CodeGenerator.Tests
{
    class ServicesExtensionsUnitTests
    {
        [Test]
        public void AddCodeGenerator_ShouldRegisterCodeGeneratorServices()
        {
            var services = A.Fake<IServices>();
            services.AddCodeGenerator();
            A.CallTo(() => services.AddSingleton<IEntityGenerator, EntityGenerator>()).MustHaveHappened();
            A.CallTo(() => services.AddSingleton<IMappingsClassGenerator, MappingsClassGenerator>()).MustHaveHappened();
            A.CallTo(() => services.AddSingleton<ICodeFileGenerator, CodeFileGenerator>()).MustHaveHappened();
        }
    }
}
