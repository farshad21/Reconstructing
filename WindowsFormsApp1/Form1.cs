using Models;

using Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        PhoneBookService _service = new PhoneBookService();

        string selectedId = "";
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {

            var contacts = _service.GetContacts();

            var newContact = new Contact();
            newContact.Firstname = txt_firstname.Text;
            newContact.Lastname = txt_lastname.Text;
            newContact.PhoneNumber = txt_phoneNumber.Text;


            if (string.IsNullOrEmpty(selectedId))
            {

                newContact.Id = Guid.NewGuid();
                contacts.Add(newContact);
            }
            else
            {
                var contactForEdit = contacts.FirstOrDefault(x => x.Id.ToString() == selectedId);
                contacts.Remove(contactForEdit);
                newContact.Id = Guid.Parse(selectedId);
                contacts.Add(newContact);
                selectedId = "";
            }



            var saveResult = _service.SaveContact(contacts);
            if (saveResult)
            {
                FillGridView(contacts);
            }
            clearForm();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            var contacts = _service.GetContacts();
            FillGridView(contacts);
        }





        public void FillGridView(List<Contact> model)
        {

            grd_contacts.Rows.Clear();
            foreach (Contact contact in model)
            {
                grd_contacts.Rows.Add(contact.Id, contact.Firstname, contact.Lastname, contact.PhoneNumber);
            }

        }


        public void clearForm()
        {
            txt_firstname.Text = "";
            txt_lastname.Text = "";
            txt_phoneNumber.Text = "";
        }

        private void grd_contacts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = grd_contacts.CurrentRow.Index;
            var id = grd_contacts.Rows[index].Cells[0].Value.ToString();
            var contacts = _service.GetContacts();

            try
            {
                var contactForEdit = contacts.FirstOrDefault(x => x.Id.ToString() == id);
                selectedId = id;


                txt_firstname.Text = contactForEdit.Firstname;
                txt_lastname.Text = contactForEdit.Lastname;
                txt_phoneNumber.Text = contactForEdit.PhoneNumber;
            }
            catch (Exception)
            {

            }


        }

        private void grd_contacts_CancelRowEdit(object sender, QuestionEventArgs e)
        {

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedId))
            {
                var deleteResult = _service.DeleteContact(selectedId);
                var contacts = _service.GetContacts();
                FillGridView(contacts);
                clearForm();
            }
        }
    }
}
