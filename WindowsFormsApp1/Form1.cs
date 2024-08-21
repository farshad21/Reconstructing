using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {


        string filePath = @"D:\data.json";
        string selectedId = "";
        public Form1()
        {
            InitializeComponent();
        }


        public class Contact
        {
            public Guid Id { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string PhoneNumber { get; set; }
        }



        private void button1_Click(object sender, EventArgs e)
        {

            var contacts = GetContacts();

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



            var saveResult = SaveContact(contacts);
            if (saveResult)
            {
                FillGridView(contacts);
            }
            clearForm();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath);
            }

            var contacts = GetContacts();
            FillGridView(contacts);
        }



        public List<Contact> GetContacts()
        {
            var result = new List<Contact>();

            try
            {
                var fileString = System.IO.File.ReadAllText(filePath);
                result = JsonConvert.DeserializeObject<List<Contact>>(fileString);
                if (result == null)
                {
                    result = new List<Contact>();
                }
                bool hasInvalidId = false;
                foreach (var contact in result)
                {
                    if (contact.Id == null || contact.Id == Guid.Empty)
                    {
                        hasInvalidId = true;
                        contact.Id = Guid.NewGuid();
                    }
                }
                if (hasInvalidId)
                {
                    SaveContact(result);
                }

            }
            catch (Exception)
            {

            }


            return result;

        }

        public bool SaveContact(List<Contact> model)
        {

            try
            {
                var stringModel = JsonConvert.SerializeObject(model);
                System.IO.File.WriteAllText(filePath, stringModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }


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
            var contacts = GetContacts();

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
                var contacts = GetContacts();
                var contactForDelete = contacts.FirstOrDefault(x => x.Id.ToString() == selectedId);
                contacts.Remove(contactForDelete);
                SaveContact(contacts);
                FillGridView(contacts);
                clearForm();
            }
        }
    }
}
