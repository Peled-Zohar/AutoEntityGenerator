﻿using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.ViewModels;
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
        private readonly EntityConfigurationViewModel _viewModel;
        public EntityConfigurationWindow(EntityConfigurationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.RequestClose += ViewModel_RequestClose;
            _viewModel.RequestFocus += ViewModel_RequestFocus;
        }

        private void ViewModel_RequestFocus()
        {
            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        private void ViewModel_RequestClose(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public IUserInteractionResult Result => _viewModel.Result;

    }
}
