using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using TOTD.Utility.UnitTestHelpers;

namespace CH.RescueGroupsHelper
{
    public partial class RescueGroupsHelper : Form
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

        public RescueGroupsHelper()
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

            IEnumerable<string> critterFields = new CritterSourceStorage(_configuration, _client, new NullEventPublisher()).Fields.Select(x => x.Name);
            clbImporterFields.Items.AddRange(critterFields.ToArray());
            ChangeCheckState(clbImporterFields, CheckState.Checked);
        }

        private void ChangeCheckState(CheckedListBox clb, CheckState checkState)
        {
            for (int i = 0; i < clb.Items.Count; i++)
            {
                clb.SetItemCheckState(i, checkState);
            }
        }
    }
}
