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
using UserManager.Controllers;
using UserManager.Models;

namespace UserManager
{
    public partial class MainForm : Form
    {
        private bool IsShowingDeleted { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists("Database.sqlite"))
                    DatabaseController.CreateDatabase();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Can't create database file!");
            }

            saveFileDialog.Filter = "Excel Files|*.xlsx";
            SetupUserListView();
        }

        private void SetupUserListView()
        {
            userListView.Items.Clear();

            try
            {
                DatabaseController.GetUsers();
            }
            catch (Exception e)
            {
                MessageBox.Show("Can't get users from database!");
            }

            foreach (User user in UserController.UserList)
                if (IsShowingDeleted || !user.IsDeleted)
                    CreateListViewRow(user);
        }

        private void UpdateListView()
        {
            userListView.Items.Clear();
            foreach (User user in UserController.UserList)
                if (IsShowingDeleted || !user.IsDeleted)
                    CreateListViewRow(user);
        }

        private void CreateListViewRow(User user)
        {
            string[] row =
            {
                user.Id.ToString(), user.Login, user.LastName, user.FirstName, user.Patronymic,
                user.CreationDate.ToString(),
                user.IsDeleted.ToString()
            };

            ListViewItem listViewItem = new ListViewItem(row) {Tag = user};
            userListView.Items.Add(listViewItem);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userListView.SelectedItems.Count > 0 && userListView.SelectedItems[0].Tag is User user)
                UserController.ChangeUserStatus(user);

            UpdateListView();
        }

        private void contextMenuStrip_Opened(object sender, EventArgs e)
        {
            if (userListView.SelectedItems.Count > 0 && userListView.SelectedItems[0].Tag is User user)
                deleteToolStripMenuItem.Checked = user.IsDeleted;
        }

        private void showDeletedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem toolStripMenuItem)
            {
                toolStripMenuItem.Checked = !toolStripMenuItem.Checked;
                IsShowingDeleted = toolStripMenuItem.Checked;
            }

            UpdateListView();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserActionForm userActionForm = new UserActionForm();
            userActionForm.ShowDialog(this);

            UpdateListView();
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userListView.SelectedItems.Count > 0 && userListView.SelectedItems[0].Tag is User user)
            {
                UserActionForm userActionForm = new UserActionForm(user, true);
                userActionForm.ShowDialog(this);

                UpdateListView();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                try
                {
                    UserController.ExportUsers(saveFileDialog.FileName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Can't export users!");
                }
        }
    }
}