using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutoEntityGenerator.UI
{
    public class UserInteraction : IUserInteraction
    {
        public IUserInteractionResult ShowUIForm(Entity entityInfo)
        {
            try
            {
                using (var instance = new ConfigureEntity(entityInfo))
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
