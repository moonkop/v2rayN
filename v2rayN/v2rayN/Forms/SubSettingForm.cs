using System;
using System.Collections.Generic;
using System.Windows.Forms;
using v2rayN.Handler;
using v2rayN.Mode;

namespace v2rayN.Forms
{
    public partial class SubSettingForm : BaseForm
    {
        private SubItem curItem = null;

        public SubSettingForm()
        {
            InitializeComponent();
        }

        private void SubSettingForm_Load(object sender, EventArgs e)
        {
            if (config.subItem == null)
            {
                config.subItem = new List<SubItem>();
            }
            RefreshSubsView();

            if (lvSubs.Items.Count>0)
            {
                lvSubs.Items[0].Selected = true;

            }
        }


     

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreshSubsView()
        {
            lvSubs.Items.Clear();

            for (int k = 0; k < config.subItem.Count; k++)
            {
                var item = config.subItem[k];
                ListViewItem lvItem = new ListViewItem(new string[]
                {
                    (k+1).ToString(),
                    item.remarks,
                    item.url,
                    item.updateInterval.ToString()
                });
                lvSubs.Items.Add(lvItem);
            }
        }


        /// <summary>
        /// 取得ListView选中的行
        /// </summary>
        /// <returns></returns>
        private int GetLvSelectedIndex()
        {
            int index = -1;
            try
            {
                if (lvSubs.SelectedIndices.Count <= 0)
                {
                    UI.Show("请先选择");
                    return index;
                }
                index = lvSubs.SelectedIndices[0];
                return index;
            }
            catch
            {
                return index;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (config.subItem.Count <= 0)
            {
                AddSub();
            }

            if (ConfigHandler.SaveSubItem(ref config) == 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                UI.Show("操作失败，请检查重试");
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSub();

            RefreshSubsView();
            lvSubs.Items[lvSubs.Items.Count - 1].Selected = true;

           
        
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = GetLvSelectedIndex();
            if (index < 0)
            {
                return;
            }
            config.subItem.RemoveAt(index);

            RefreshSubsView();
        }

        private void lvSubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            curItem = null;
            int index = -1;
            try
            {
                if (lvSubs.SelectedIndices.Count > 0)
                {
                    index = lvSubs.SelectedIndices[0];
                }
                curItem = config.subItem[index];
                BindingSub();
            }
            catch
            {
            }
        }
        private void AddSub()
        {
            var subItem = new SubItem();
            subItem.updateInterval = 24;
            subItem.id =
            subItem.remarks =               
            subItem.url = string.Empty;
            config.subItem.Add(subItem);
            
        }


        private void BindingSub()
        {
            if (curItem != null)
            {
                txtInterval.Text = curItem.updateInterval.ToString();
                txtRemarks.Text = curItem.remarks.ToString();
                txtUrl.Text = curItem.url.ToString();
            }
        }

        private void EndBindingSub()
        {
            if (curItem != null)
            {
                curItem.remarks = txtRemarks.Text.Trim();
                curItem.url = txtUrl.Text.Trim();
                int updateInterval = 0;
                string txtInterval_text = txtInterval.Text.Trim();
                if (txtInterval_text != "")
                {
                    if (int.TryParse(txtInterval_text, out updateInterval))
                    {
                        curItem.updateInterval = updateInterval;
                    }
                    else
                    {
                        MessageBox.Show("输入更新时间不合法");
                    }
                }
                else
                {
                    curItem.updateInterval = 0;
                }

                RefreshSubsView();
            }
        }

        private void txt_leave(object sender, EventArgs e)
        {
            EndBindingSub();
        }
    }
}
