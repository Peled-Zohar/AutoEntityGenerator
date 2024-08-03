using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoEntityGenerator.UI.Views
{
    public interface IEntityConfigurationWindow
    {
        IUserInteractionResult Result { get; }

        bool? ShowDialog();
    }

    /// <summary>
    /// Interaction logic for EntityConfigurationWindow.xaml
    /// </summary>
    public partial class EntityConfigurationWindow : Window, IEntityConfigurationWindow
    {
        private readonly Entity _entity;

        public EntityConfigurationWindow(Entity entity)
        {
            InitializeComponent();
            _entity = entity;
        }

        public IUserInteractionResult Result => throw new NotImplementedException();

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }
    }
}
