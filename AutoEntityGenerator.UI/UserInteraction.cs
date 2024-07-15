using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutoEntityGenerator.UI
{
    internal class UserInteraction : IUserInteraction
    {
        private readonly IEntityConfigurationFormFactory _configureEntityFactory;

        public UserInteraction(IEntityConfigurationFormFactory configureEntityFactory)
        {
            _configureEntityFactory = configureEntityFactory;
        }
        public IUserInteractionResult ShowUIForm(Entity entityInfo)
        {
            try
            {
                using (var instance = _configureEntityFactory.Create(entityInfo))
                {
                    var dialogResult = instance.ShowDialog();
                    return dialogResult == DialogResult.OK
                        ? instance.Result
                        : new UserInteractionResult();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception initialising the form or calling ShowDialog");
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
