using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CH.RescueGroupsExplorer
{
    public partial class RescueGroupsExplorer : Form
    {
        public RescueGroupsExplorer()
        {
            InitializeComponent();
        }

        private async void btnExecute_Click(object sender, EventArgs e)
        {
            IEnumerable<JProperty> result = null;
            try
            {
                if (cmbType.Text == "animals" && cmbAction.Text == "publicSearch")
                {
                    CritterSearchResultStorage storage = new CritterSearchResultStorage(new RescueGroupsConfiguration(), new HttpClientProxy(txtHttp));
                    var searchResults = await storage.GetAllAsync();
                }
                else
                {
                    RescueGroupsExplorerStorage storage = new RescueGroupsExplorerStorage(new HttpClientProxy(txtHttp));
                    result = await storage.GetAllAsync(cmbType.Text, cmbAction.Text, cbPrivate.Checked);
                }
            }
            catch (Exception ex)
            {
                txtLog.AppendText(Environment.NewLine);
                txtLog.AppendText(ex.ToString());
            }

            if (result != null)
            {
                txtLog.AppendText(Environment.NewLine);
                txtLog.AppendText(result.ToString());

                tree.Nodes.Clear();
                AddObjectNodes(result, "JSON", tree.Nodes);
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
    }
}
