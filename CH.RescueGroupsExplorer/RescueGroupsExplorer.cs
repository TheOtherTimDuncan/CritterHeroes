using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CH.RescueGroups;
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
            JObject response = null;
            try
            {
                RescueGroupsStorage storage = new RescueGroupsStorage();

                JObject request = storage.CreateRequest(
                  new JProperty("objectType", cmbType.Text),
                new JProperty("objectAction", cmbAction.Text)
                );

                if (cbPrivate.Checked)
                {
                    IEnumerable<JProperty> loginResult = await storage.LoginAsync();
                    foreach (JProperty property in loginResult)
                    {
                        request.Add(property);
                    }
                }

                response = await storage.GetDataAsync(request);
                storage.ValidateResponse(response);
            }
            catch (Exception ex)
            {
                txtLog.AppendText(Environment.NewLine);
                txtLog.AppendText(ex.ToString());
            }

            if (response != null)
            {
                txtLog.AppendText(Environment.NewLine);
                txtLog.AppendText(response.ToString());

                tree.Nodes.Clear();
                AddObjectNodes(response, "JSON", tree.Nodes);
            }
        }

        public void AddObjectNodes(JObject root, string name, TreeNodeCollection parent)
        {
            TreeNode node = new TreeNode(name);
            parent.Add(node);

            foreach (JProperty property in root.Properties())
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
                AddObjectNodes((JObject)token, name, parent);
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
    }
}
