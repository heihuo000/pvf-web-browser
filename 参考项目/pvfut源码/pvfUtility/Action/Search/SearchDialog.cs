using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using pvfUtility.Action.Search;
using pvfUtility.Actions;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Model;
using pvfUtility.Service;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Shell.Dialogs.Search
{
    internal partial class SearchDialog : DialogBase
    {
        private readonly SearchPresenter _presenter;

        public SearchDialog(FileCollectionData data, SearchPresenter p)
        {
            _presenter = p;
            InitializeComponent();
            MainPresenter.Instance.View.vs.SetStyle(contextMenuStripHis, VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);

            btnOk.Click += buttonOK_Click;
            btnCancel.Click += buttonCancel_Click;
            labelText.Text = $"当前文件集：{data.Name.Replace("\r\n", "")}。";
            checkBoxUseLikePath.Checked = Config.Instance.IsUseLikeSearchPath;
            checkBoxNewCollection.Checked = Config.Instance.IsNewCollection;
            checkBoxStartMatch.Checked = Config.Instance.IsStartMatch;
            switch (Config.Instance.SearchMethod)
            {
                case SearchPresenter.SearchMethod.None:
                    radioButtonMethodNone.Checked = true;
                    break;
                case SearchPresenter.SearchMethod.Match:
                    radioButtonMethodMatch.Checked = true;
                    break;
                case SearchPresenter.SearchMethod.Remove:
                    radioButtonMethodRemove.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (Config.Instance.SearchType)
            {
                case SearchPresenter.SearchType.SearchNum:
                    radioButton1.Checked = true;
                    break;
                case SearchPresenter.SearchType.SearchStrings:
                    radioButton2.Checked = true;
                    break;
                case SearchPresenter.SearchType.SearchFileName:
                    radioButton3.Checked = true;
                    break;
                case SearchPresenter.SearchType.SearchScript:
                    radioButton4.Checked = true;
                    break;
                case SearchPresenter.SearchType.SearchName:
                    radioButton5.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (Config.Instance.SearchNormalUsing)
            {
                case SearchPresenter.SearchNormalUsing.None:
                    radioButtonUseNormal.Checked = true;
                    break;
                case SearchPresenter.SearchNormalUsing.Like:
                    radioButtonUseLike.Checked = true;
                    break;
                case SearchPresenter.SearchNormalUsing.Regex:
                    radioButtonUseRegex.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            _presenter.CurSetting.SearchPath = textBoxPath.Text;
            if (radioButtonMethodNone.Checked && _presenter.CurSetting.SearchPath != "")
            {
                if (Config.Instance.PathSearchHistory.Contains(_presenter.CurSetting.SearchPath))
                    Config.Instance.PathSearchHistory.Remove(_presenter.CurSetting.SearchPath);
                if (Config.Instance.PathSearchHistory.Count >= 10)
                    Config.Instance.PathSearchHistory.RemoveAt(0);
                Config.Instance.PathSearchHistory.Add(_presenter.CurSetting.SearchPath);
            }

            if (radioButton1.Checked)
                _presenter.CurSetting.SearchType = SearchPresenter.SearchType.SearchNum;
            else if (radioButton2.Checked)
                _presenter.CurSetting.SearchType = SearchPresenter.SearchType.SearchStrings;
            else if (radioButton3.Checked)
                _presenter.CurSetting.SearchType = SearchPresenter.SearchType.SearchFileName;
            else if (radioButton4.Checked)
                _presenter.CurSetting.SearchType = SearchPresenter.SearchType.SearchScript;
            else if (radioButton5.Checked) _presenter.CurSetting.SearchType = SearchPresenter.SearchType.SearchName;
            // (SearchPresenter.SearchType)Enum.ToObject(typeof(SearchPresenter.SearchType),
            //     comboBoxSearchType.SelectedIndex);

            _presenter.CurSetting.IsUseLikeSearchPath =
                Config.Instance.IsUseLikeSearchPath = checkBoxUseLikePath.Checked;

            _presenter.CurSetting.Keyword = _presenter.CurSetting.SearchType == SearchPresenter.SearchType.SearchScript
                ? textSearchScriptContent.Text
                : textSearchKeyword.Text;
            if (_presenter.CurSetting.Keyword == "")
            {
                DialogService.Error("请输入搜索内容", "", Handle);
                return;
            }

            if (radioButtonMethodNone.Checked && checkBoxNewCollection.Checked)
                CollectionExplorerPresenter.Instance.NewFileCollection(_presenter.CurSetting.Keyword);
            _presenter.CurSetting.FileCollection = CollectionExplorerPresenter.Instance.CurFileCollection;
            _presenter.CurSetting.IsStartMatch = Config.Instance.IsStartMatch = checkBoxStartMatch.Checked;
            _presenter.CurSetting.SearchMethod = SearchPresenter.SearchMethod.None;
            if (radioButtonMethodMatch.Checked)
                _presenter.CurSetting.SearchMethod = SearchPresenter.SearchMethod.Match;
            else if (radioButtonMethodRemove.Checked)
                _presenter.CurSetting.SearchMethod = SearchPresenter.SearchMethod.Remove;
            Config.Instance.IsNewCollection = checkBoxNewCollection.Checked;
            Config.Instance.SearchMethod = _presenter.CurSetting.SearchMethod;

            switch (_presenter.CurSetting.SearchType)
            {
                case SearchPresenter.SearchType.SearchNum:
                    if (_presenter.CurSetting.Keyword.IndexOf('.') > 0)
                    {
                        if (!float.TryParse(_presenter.CurSetting.Keyword, out _))
                        {
                            DialogService.Error("请输入正确的浮点数", "", Handle);
                            return;
                        }
                    }
                    else if (!int.TryParse(_presenter.CurSetting.Keyword, out _))
                    {
                        DialogService.Error("请输入正确的整数", "", Handle);
                        return;
                    }

                    break;
                case SearchPresenter.SearchType.SearchStrings:
                    break;
                case SearchPresenter.SearchType.SearchFileName:
                    break;
                case SearchPresenter.SearchType.SearchScript:
                    _presenter.CurSetting.IsUseRegexInScriptSearch = radioButtonRegex.Checked;

                    if (_presenter.CurSetting.IsUseRegexInScriptSearch)
                        try
                        {
                            _presenter.CurSetting.Regex = new Regex(_presenter.CurSetting.Keyword,
                                RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        }
                        catch (ArgumentException exception)
                        {
                            DialogService.Error("正则表达式错误", exception.Message, Handle);
                            return;
                        }

                    break;
                case SearchPresenter.SearchType.SearchName:
                    break;
            }
/*            if (radioButtonSearchTypeNum.Checked)
            {
                _presenter.CurSetting.SearchType = SearchManager.SearchType.SearchNum;

            }
            else if (radioButtonSearchTypeStrings.Checked)
                _presenter.CurSetting.SearchType = SearchManager.SearchType.SearchStrings;
            else if (radioButtonSearchTypeName.Checked)
                _presenter.CurSetting.SearchType = SearchManager.SearchType.SearchName;
            else if (radioButtonSearchTypeFileName.Checked)
                _presenter.CurSetting.SearchType = SearchManager.SearchType.SearchFileName;
            else if (radioButtonSearchTypeScriptContent.Checked)
            {

            }*/

            Config.Instance.SearchType = _presenter.CurSetting.SearchType;
            if (_presenter.CurSetting.SearchType != SearchPresenter.SearchType.SearchScript)
            {
                if (radioButtonUseNormal.Checked)
                {
                    _presenter.CurSetting.SearchNormalUsing = SearchPresenter.SearchNormalUsing.None;
                }
                else if (radioButtonUseLike.Checked)
                {
                    _presenter.CurSetting.SearchNormalUsing = SearchPresenter.SearchNormalUsing.Like;
                }
                else if (radioButtonUseRegex.Checked)
                {
                    _presenter.CurSetting.SearchNormalUsing = SearchPresenter.SearchNormalUsing.Regex;
                    try
                    {
                        _presenter.CurSetting.Regex = new Regex(_presenter.CurSetting.Keyword,
                            RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                    catch (ArgumentException exception)
                    {
                        DialogService.Error("正则表达式错误", exception.Message, Handle);
                        return;
                    }
                }

                Config.Instance.SearchNormalUsing = _presenter.CurSetting.SearchNormalUsing;
            }


            Task.Run(() => _presenter.StartSearch());
            Close();
            Dispose();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

//        private void radioButtonSearchType5_CheckedChanged(object sender, EventArgs e)
//        {
//            UpdateInterface();
//        }
//        private void radioButtonSearchType1_CheckedChanged(object sender, EventArgs e)
//        {
//            groupBoxMode.Visible = !radioButtonSearchTypeNum.Checked;
//            if (radioButtonSearchTypeNum.Checked)
//                radioButtonUseNormal.Checked = true;
//        }

        private void radioButtonMethod1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPath.Enabled = buttonHistory.Enabled = buttonPattern.Enabled = checkBoxNewCollection.Enabled = false;
        }

        private void radioButtonMethod0_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPath.Enabled = buttonHistory.Enabled = buttonPattern.Enabled = checkBoxNewCollection.Enabled = true;
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://msdn.microsoft.com/zh-cn/library/aa293095(v=vs.71).aspx");
        }

        private void buttonPattern_Click(object sender, EventArgs e)
        {
            var dialog = new SelectFolderDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
                textBoxPath.Text = dialog.SelectedPath;
            //contextMenuStripPattern.Show(buttonPattern.PointToScreen(buttonPattern.ClientRectangle.Location));
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            contextMenuStripHis.Items.Clear();
            var count = Config.Instance.PathSearchHistory.Count;
            var list = new List<string>();
            for (var i = count - 1; i >= 0; i--)
                list.Add(Config.Instance.PathSearchHistory[i]);
            foreach (var item in list)
            {
                var newItem = contextMenuStripHis.Items.Add(item);
                newItem.Tag = item;
            }

            contextMenuStripHis.Items.Add("-");
            contextMenuStripHis.Items.Add("清除历史记录");
            contextMenuStripHis.Show(buttonHistory.PointToScreen(buttonHistory.ClientRectangle.Location));
        }

        private void contextMenuStripHis_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "清除历史记录")
                Config.Instance.PathSearchHistory.Clear();
            else
                textBoxPath.Text = e.ClickedItem.Tag.ToString();
        }

        private void contextMenuStripPattern_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag != null)
                textBoxPath.Text += e.ClickedItem.Tag.ToString();
        }

        private void linkLabelLearningRegex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(
                "https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/regular-expression-language-quick-reference");
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            AcceptButton = null;
            CancelButton = null;
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private void radioButtonUseNormal_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxStartMatch.Enabled = radioButtonUseNormal.Checked;
            if (!checkBoxStartMatch.Enabled)
                checkBoxStartMatch.Checked = false;
        }

        // private void comboBoxSearchType_SelectedIndexChanged(object sender, EventArgs e)
        // {
        //     panelNormal.Visible = comboBoxSearchType.SelectedIndex != 3;
        //     panelScriptContent.Visible = !panelNormal.Visible;
        // }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            panelNormal.Visible = !radioButton4.Checked;
            panelScriptContent.Visible = !panelNormal.Visible;
        }
    }
}