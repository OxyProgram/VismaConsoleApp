using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaConsoleApp
{
    public class Person
    {
        private string _name;
        private string _surname;
        private List<BookTake> _booksTaken;
        private int _numberOfBooks;
        public string Name { get => _name; set => _name = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public List<BookTake> BooksTaken { get => _booksTaken; set => _booksTaken = value; }
        public int NumberOfBooks { get => _numberOfBooks; set => _numberOfBooks = value; }

        public Person() 
        {
            _booksTaken = new List<BookTake>();
        }
        public Person(string name, string surname, List<BookTake> booksTaken, int n)
        {
            _name = name;
            _surname = surname;
            _booksTaken = booksTaken;
            _numberOfBooks = n;
        }
    }
}
