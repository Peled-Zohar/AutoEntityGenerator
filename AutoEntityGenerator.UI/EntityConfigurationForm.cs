using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AutoEntityGenerator.UI
{
    public interface IEntityConfigurationForm : IDisposable
    {
        IUserInteractionResult Result { get; }

        DialogResult ShowDialog();
    }

    internal partial class EntityConfigurationForm : Form, IEntityConfigurationForm
    {
        // TODO: Consider replacing this constant with a configuration value
        // TODO: Consider localizing all currently hard coded strings.
        private const string Generated = "Generated";

        private readonly Entity _entity;
        private bool _allowFileNameMismatch;
        private BindingList<SelectablePropertyInfo> _properties;

        public EntityConfigurationForm(Entity entity)
        {
            _entity = entity;
            InitializeComponent();
            PopulateData();
        }

        public IUserInteractionResult Result { get; private set; }

        private void PopulateData()
        {
            MappingDirection.SelectedIndex = 0;
            DTOName.Text = _entity.Name + "Request";

            var selectableProperties = _entity.Properties.Select(p => new SelectablePropertyInfo()
            {
                IsSelected = true,
                Name = p.Name,
                Type = p.Type,
            });

            DestinationFolder.Text = Path.Combine(Path.GetDirectoryName(_entity.SourceFilePath), Generated);
            folderBrowserDialog.SelectedPath = Path.GetDirectoryName(_entity.SourceFilePath);

            _properties = new BindingList<SelectablePropertyInfo>(selectableProperties.ToList());
            Properties.AutoGenerateColumns = false;
            Properties.DataSource = _properties;
            Properties.Columns["Check"].DataPropertyName = nameof(SelectablePropertyInfo.IsSelected);
            Properties.Columns["PropertyName"].DataPropertyName = nameof(SelectablePropertyInfo.Name);
        }

        private bool ValidateForm()
        {
            if (!IdentifierValidator.IsValidClassName(DTOName.Text))
            {
                return ShowError("Invalid class name.", DTOName);
            }

            if (string.IsNullOrWhiteSpace(DestinationFolder.Text))
            {
                return ShowError("Destination folder is required", DestinationFolder);
            }

            if (DestinationFolder.Text.Any(c => Path.GetInvalidPathChars().Contains(c)))
            {
                return ShowError("Invalid Destination folder path.", DestinationFolder);
            }

            var projectDirectory = Path.GetDirectoryName(_entity.Project.FilePath);
            if (!DestinationFolder.Text.StartsWith(projectDirectory))
            {
                return ShowError("Destination folder must be a subfolder of the project folder.", DestinationFolder);
            }

            if (!_properties.Any(p => p.IsSelected))
            {
                return ShowError("You must check at least one property.", Properties);
            }

            // TODO: Consider more form validation logic here

            return true;

            bool ShowError(string errorMessage, Control controlToFocus)
            {
                MessageBox.Show(errorMessage);
                controlToFocus.Focus();
                return false;
            }
        }

        private void DTOName_TextChanged(object sender, EventArgs e)
        {
            if (!_allowFileNameMismatch)
            {
                GeneratedFileName.Text = $"{DTOName.Text}.cs";
            }
        }

        private void GeneratedFileName_Leave(object sender, EventArgs e)
        {
            if (GeneratedFileName.Text != DTOName.Text + ".cs")
            {
                if (MessageBox.Show(
                    "Generated file name doesn't match entity name." + Environment.NewLine +
                    "Is that Intended?",
                    "File name and entity name mismatch",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    _allowFileNameMismatch = false;
                    GeneratedFileName.Text = DTOName.Text + ".cs";
                }
                else
                {
                    _allowFileNameMismatch = true;
                }
            }
        }

        private void Browes_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                DestinationFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            ToggleSelectedForAllProperties(true);
        }

        private void DeselectAll_Click(object sender, EventArgs e)
        {
            ToggleSelectedForAllProperties(false);
        }

        private void ToggleSelectedForAllProperties(bool selected)
        {
            foreach (var property in _properties)
            {
                property.IsSelected = selected;
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            Result = new UserInteractionResult(
                mappingDirection: MappingDirection.SelectedIndex == 0
                    ? Common.Interfaces.MappingDirection.FromDtoToModel
                    : Common.Interfaces.MappingDirection.FromModelToDto,
                targetDirectory: DestinationFolder.Text,
                entityName: DTOName.Text,
                entityProperties: _properties
                    .Where(p => p.IsSelected)
                    .Select(p => new Property()
                    {
                        Name = p.Name,
                        Type = p.Type
                    }
                    ).ToList(),
                fileName: GeneratedFileName.Text
            );

            DialogResult = DialogResult.OK;
        }
    }
}