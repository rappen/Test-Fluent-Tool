using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace Test_Fluent_Tool
{
    public partial class TestFluentTool : PluginControlBase
    {
        public TestFluentTool()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
        }

        private void GetAccounts()
        {
            var flqe = new MscrmTools.FluentQueryExpressions.Query("account")
                .Select("name")
                .Where("name", ConditionOperator.Equal, "gfgf");
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting accounts",
                Work = (worker, args) =>
                {
                    args.Result = flqe.GetAll(Service);
                },
                PostWorkCallBack = (args) =>
                {
                    PostHandling(args);
                    WorkAsync(new WorkAsyncInfo
                    {
                        Message = "Getting first",
                        Work = (worker, args2) =>
                        {
                            args2.Result = flqe.GetFirstOrDefault(Service);
                        },
                        PostWorkCallBack = (args2) =>
                        {
                            PostHandling(args2);
                        }
                    });
                }
            });
        }

        private void PostHandling(System.ComponentModel.RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                ShowErrorDialog(args.Error);
            }
            else if (args.Result is EntityCollection coll)
            {
                MessageBox.Show($"Found {coll.Entities.Count} accounts");
            }
            else if (args.Result is List<Entity> list)
            {
                MessageBox.Show($"Found {list.Count} accounts");
            }
            else if (args.Result is Entity ent)
            {
                MessageBox.Show($"Record {ent.LogicalName} {ent.Id}");
            }
            else if (args.Result == null)
            {
                MessageBox.Show("Null");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteMethod(GetAccounts);
        }
    }
}