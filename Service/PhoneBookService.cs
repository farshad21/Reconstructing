using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Service
{
    public class PhoneBookService
    {
        PhoneBookRespository _repo;
        public PhoneBookService()
        {
            _repo = new PhoneBookRespository();
        }

        public bool DeleteContact(string Id)
        {
            try
            {
                var contacts = _repo.GetContacts();
                var contactForDelete = contacts.FirstOrDefault(x => x.Id.ToString() == Id);
                contacts.Remove(contactForDelete);
                _repo.SaveContact(contacts);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public Contact GetContactById(string id)
        {

            var contacts = GetContacts();
            var contact = contacts.FirstOrDefault(x => x.Id.ToString() == id.ToString());
            return contact;

            //if (contact == null)
            //{
            //    return null;
            //}
            //return contact;
        }

        public List<Contact> GetContacts()
        {
            return _repo.GetContacts();
        }

        public bool SaveContact(Contact model)
        {
            var contacts = GetContacts();

            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                object propertyValue = property.GetValue(model);

                if (propertyValue == "")
                {
                    MessageBox.Show($"{property.Name} can not be empty\n"+"Please fill it", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                contacts.Add(model);
            }
            else
            {
                string contactId = model.Id.ToString();
                var contactForEdit = contacts.FirstOrDefault(x => x.Id.ToString() == contactId);
                if (contactForEdit != null)
                {
                    contacts.Remove(contactForEdit);
                    contacts.Add(model);
                }

            }


            return _repo.SaveContact(contacts);


        }

        //public bool SaveContact(Contact model)
        //{
        //    var contacts = GetContacts();


        //    if (model.Id == Guid.Empty)
        //    {
        //        model.Id = Guid.NewGuid();
        //        contacts.Add(model);
        //    }
        //    else
        //    {
        //        var contactForEdit = GetContactById(model.Id.ToString());
        //        if (contactForEdit!=null)
        //        {
        //            contacts.Remove(contactForEdit);
        //            contacts.Add(model);
        //        }

        //    }


        //    return _repo.SaveContact(contacts);
        //}
    }
}
