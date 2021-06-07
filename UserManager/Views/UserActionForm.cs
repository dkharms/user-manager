using System;
using System.Windows.Forms;
using UserManager.Controllers;
using UserManager.Models;

namespace UserManager
{
    public partial class UserActionForm : Form
    {
        private bool IsUpdating { get; set; }
        private User CurrentUser { get; set; }

        public UserActionForm()
        {
            InitializeComponent();
        }
        
        public UserActionForm(User user, bool isUpdating)
        {
            InitializeComponent();
            
            IsUpdating = isUpdating;
            CurrentUser = user;
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            string login = loginTextBox.Text;
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string patronymic = patronymicTextBox.Text;

            if (IsUpdating)
                UserController.UpdateUser(CurrentUser, login, firstName, lastName, patronymic);
            else
                UserController.CreateUser(login, firstName, lastName, patronymic);
            
            this.Close();
        }

        private void UserActionForm_Load(object sender, EventArgs e)
        {
            if (IsUpdating)
            {
                actionButton.Text = "UPDATE";
                
                loginTextBox.Text = CurrentUser.Login;
                firstNameTextBox.Text = CurrentUser.FirstName;
                lastNameTextBox.Text = CurrentUser.LastName;
                patronymicTextBox.Text = CurrentUser.Patronymic;
            }
        }
    }
}