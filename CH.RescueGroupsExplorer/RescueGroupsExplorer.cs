using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                RescueGroupsExplorerStorage storage = new RescueGroupsExplorerStorage();
                result = await storage.GetAllAsync(cmbType.Text, cmbAction.Text, cbPrivate.Checked);
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
    }
}
