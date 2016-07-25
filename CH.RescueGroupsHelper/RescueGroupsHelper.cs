using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;

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
