using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using TOTD.Utility.UnitTestHelpers;

namespace CH.RescueGroupsExplorer
{
    public partial class RescueGroupsExplorer : Form
    {
        // Explorer
        private HttpClientProxy _client;
        private RescueGroupsConfiguration _configuration;
        private Writer _explorerWriter;
        private List<string> _responses;

        // Importer
        private string _path;
        private string _filePath;
        private Writer _importerWriter;

        public RescueGroupsExplorer()
        {
            InitializeComponent();

            _responses = new List<string>();

            // Explorer
            _explorerWriter = new Writer(txtHttp);
            _client = new HttpClientProxy(_explorerWriter, _responses);
            _configuration = new RescueGroupsConfiguration();

            // Importer
            _path = Path.Combine(UnitTestHelper.GetSolutionRoot(), ".vs", "Critters");
            _filePath = Path.Combine(_path, "critters.json");
            _importerWriter = new Writer(txtImporterLog);
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbFields.Items.Count; i++)
            {
                clbFields.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbFields.Items.Count; i++)
            {
                clbFields.SetItemCheckState(i, CheckState.Checked);
            }
        }
    }
}
