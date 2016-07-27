using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CH.RescueGroupsHelper.Importer;

namespace CH.RescueGroupsHelper
{
    public partial class RescueGroupsHelper
    {
        private async void btnImportWeb_Click(object sender, EventArgs e)
        {
            CritterImporter importer = new Importer.CritterImporter(_importerWriter);
            await importer.ImportFromWebAsync(clbImporterFields.CheckedItems);
            NullEventPublisher publisher = new NullEventPublisher();
        }

        private async void btnImportFile_Click(object sender, EventArgs e)
        {
            CritterImporter importer = new CritterImporter(_importerWriter);
            await importer.ImportFromFileAsync();
        }

        private async void btnImportPeopleWeb_Click(object sender, EventArgs e)
        {
            PersonImporter importer = new PersonImporter(_importerWriter);
            await importer.ImportWebAsync();
        }

        private async void btnImportPeopleFile_Click(object sender, EventArgs e)
        {
            PersonImporter importer = new PersonImporter(_importerWriter);
            await importer.ImportFileAsync();
        }

        private async void btnImportBusinessessFile_Click(object sender, EventArgs e)
        {
            BusinessImporter importer = new BusinessImporter(_importerWriter);
            await importer.ImportFileAsync();
        }

        private async void btnImportBusinessesWeb_Click(object sender, EventArgs e)
        {
            BusinessImporter importer = new BusinessImporter(_importerWriter);
            await importer.ImportWebAsync();
        }

        private void btnImporterCheckAll_Click(object sender, EventArgs e)
        {
            ChangeCheckState(clbImporterFields, CheckState.Checked);
        }

        private void btnImporterUncheckAll_Click(object sender, EventArgs e)
        {
            ChangeCheckState(clbImporterFields, CheckState.Unchecked);
        }
    }
}
