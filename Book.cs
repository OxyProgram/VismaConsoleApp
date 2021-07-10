using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaConsoleApp
{
    public class Book
    {
        private string _name;
        private string _author;
        private string _category;
        private string _language;
        private DateTime _publicationDate;
        private string _ISBN;
        private bool _isTaken;

        public Book() { }
        public Book(string name, string author, string category,
            string language, DateTime publicationDate, string ISBN, bool taken)
        {
            _name = name;
            _author = author;
            _category = category;
            _language = language;
            _publicationDate = publicationDate;
            _ISBN = ISBN;
            _isTaken = taken;
        }

        public string Name { get => _name; set => _name = value; }
        public string Author { get => _author; set => _author = value; }
        public string Category { get => _category; set => _category = value; }
        public string Language { get => _language; set => _language = value; }
        public DateTime PublicationDate { get => _publicationDate; set => _publicationDate = value; }
        public string ISBN { get => _ISBN; set => _ISBN = value; }
        public bool IsTaken { get => _isTaken; set => _isTaken = value; }

        public void toString()
        {
            Console.WriteLine("Name: " + _name + " Author: " + _author + " Category: " + Category +
                "\n Language: " + _language + " Publication Date: " + _publicationDate.ToString() + " ISBN " + _ISBN + " available: " + _isTaken.ToString() + "\n\n");
        }

    }
}
