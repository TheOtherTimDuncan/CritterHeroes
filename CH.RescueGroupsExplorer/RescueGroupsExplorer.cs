using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CH.RescueGroupsExplorer.Helpers;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using CritterHeroes.Web.Shared.Proxies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TOTD.Utility.EnumerableHelpers;

namespace CH.RescueGroupsExplorer
{
    public partial class RescueGroupsExplorer : Form
    {
        private RescueGroupsExplorerLogger _logger;
        private HttpClientProxy _client;
        private RescueGroupsConfiguration _configuration;

        public RescueGroupsExplorer()
        {
            InitializeComponent();

            _logger = new RescueGroupsExplorerLogger(txtHttp);
            _client = new HttpClientProxy();
            _configuration = new RescueGroupsConfiguration();
        }

        private async void btnExecute_Click(object sender, EventArgs e)
        {
            if (new[] { ObjectActions.Search, ObjectActions.List, "add/update", "get" }.Contains(cmbAction.Text))
            {
                switch (cmbType.Text)
                {
                    case "animals":
                        await HandleObjectActionAsync(new CrittersStorageHelper(_configuration, _client, _logger));
                        break;

                    case "animalBreeds":
                        await HandleObjectActionAsync(new BreedStorageHelper(_configuration, _client, _logger));
                        break;

                    case "animalSpecies":
                        await HandleObjectActionAsync(new SpeciesStorageHelper(_configuration, _client, _logger));
                        break;

                    case "animalStatuses":
                        await HandleObjectActionAsync(new StatusStorageHelper(_configuration, _client, _logger));
                        break;

                    case "businesses":
                        await HandleObjectActionAsync(new BusinessSourceStorageHelper(_configuration, _client, _logger));
                        break;

                    case "people":
                        await HandleObjectActionAsync(new PersonStorageHelper(_configuration, _client, _logger));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("Object Type", cmbType.Text);
                }
            }
            else
            {
                RescueGroupsExplorerStorageHelper storage = new RescueGroupsExplorerStorageHelper(_configuration, _client, _logger, cmbType.Text, cmbAction.Text, cbPrivate.Checked);
                await HandleObjectActionAsync(storage);
            }
        }

        public void AddObjectNodes(IEnumerable<JProperty> properties, string name, TreeNodeCollection parent)
        {
            TreeNode node = new TreeNode(name);
            parent.Add(node);

            foreach (JProperty property in properties)
            {
                AddTokenNodes(property.Value, property.Name, node.Nodes);
            }
        }

        public void AddTokenNodes(JToken token, string name, TreeNodeCollection parent)
        {
            if (token is JValue)
            {
                parent.Add(new TreeNode(string.Format("{0}: {1}", name, ((JValue)token).Value)));
            }
            else if (token is JArray)
            {
                AddArrayNodes((JArray)token, name, parent);
            }
            else if (token is JObject)
            {
                AddObjectNodes(((JObject)token).Properties(), name, parent);
            }
        }

        public void AddArrayNodes(JArray array, string name, TreeNodeCollection parent)
        {
            TreeNode node = new TreeNode(name);
            parent.Add(node);

            for (var i = 0; i < array.Count; i++)
            {
                AddTokenNodes(array[i], string.Format("[{0}]", i), node.Nodes);
            }
        }

        private async Task HandleObjectActionAsync<TEntity>(BaseStorageHelper<TEntity> storageHelper) where TEntity : BaseSource
        {
            try
            {
                switch (cmbAction.Text)
                {
                    case ObjectActions.Search:
                        await storageHelper.SearchAsync(clbFields.CheckedItems);
                        break;

                    case "add/update":
                        TEntity entity = storageHelper.CreateEntity();
                        txtHttp.AppendText(JsonConvert.SerializeObject(entity, Formatting.Indented));
                        await storageHelper.StorageContext.AddAsync(entity);
                        storageHelper.UpdateEntity(entity);
                        await storageHelper.StorageContext.UpdateAsync(entity);
                        break;

                    case "get":
                        await storageHelper.GetAsync(int.Parse(txtKeyValue.Text));
                        break;

                    default:
                        await storageHelper.ListAsync();
                        break;
                }

            }
            catch (Exception ex)
            {
                txtLog.AppendText(Environment.NewLine);
                txtLog.AppendText(ex.ToString());
            }

            IEnumerable<string> responses = _logger.Entries.Select(x => x.Response).ToList();

            _logger.Flush();

            if (!responses.IsNullOrEmpty())
            {
                tree.Nodes.Clear();
                int c = 1;
                foreach (string response in responses)
                {
                    JObject json = JObject.Parse(response);

                    txtLog.AppendText(Environment.NewLine);
                    txtLog.AppendText(json.ToString(Formatting.Indented));

                    AddObjectNodes(json.Properties(), c.ToString(), tree.Nodes);
                    c++;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void btnLoadJson_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    tree.Nodes.Clear();

                    IEnumerable<string> lines = File.ReadAllLines(dlg.FileName);
                    foreach (string line in lines)
                    {
                        JObject json = JObject.Parse(line);
                        AddObjectNodes(json.Properties(), Path.GetFileName(dlg.FileName), tree.Nodes);
                    }
                }
            }
        }

        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            clbFields.Items.Clear();

            if (cmbAction.Text == ObjectActions.Search || cmbAction.Text == "get")
            {
                IEnumerable<SearchField> searchFields;

                switch (cmbType.Text)
                {
                    case "animals":
                        searchFields = new CritterSourceStorage(_configuration, _client, _logger).Fields;
                        break;

                    case "animalBreeds":
                        searchFields = new BreedSourceStorage(_configuration, _client, _logger).Fields;
                        break;

                    case "animalSpecies":
                        searchFields = new SpeciesSourceStorage(_configuration, _client, _logger).Fields;
                        break;

                    case "animalStatuses":
                        searchFields = new CritterStatusSourceStorage(_configuration, _client, _logger).Fields;
                        break;

                    case "businesses":
                        searchFields = new BusinessSourceStorage(_configuration, _client, _logger).Fields;
                        break;

                    case "people":
                        searchFields = new PersonSourceStorage(_configuration, _client, _logger).Fields;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("Object Type", cmbType.Text);
                }

                if (cmbAction.Text == ObjectActions.Search)
                {
                    clbFields.Enabled = true;
                    clbFields.Items.AddRange(searchFields.Select(x => x.Name).ToArray());
                    btnCheckAll_Click(sender, e);
                }
                else if (cmbAction.Text == "get")
                {
                    lblKeyField.Enabled = true;
                    txtKeyValue.Enabled = true;
                    cmbKeyField.Items.AddRange(searchFields.Select(x => x.Name).ToArray());
                }
            }
            else
            {
                clbFields.Enabled = false;
                lblKeyField.Enabled = false;
                txtKeyValue.Enabled = false;
            }

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

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbAction_SelectedIndexChanged(sender, e);
        }
    }
}
