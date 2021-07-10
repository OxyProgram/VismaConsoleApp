using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaConsoleApp
{
    public class BookTake
    {
        private string _booksISBN;
        private DateTime _taken;
        private DateTime _returned;

        public string BookISNB { get => _booksISBN; set => _booksISBN = value; }
        public DateTime Taken { get => _taken; set => _taken = value; }
        public DateTime Returned { get => _returned; set => _returned = value; }

        public BookTake() { }
        public BookTake(string isbn, DateTime taken)
        {
            _booksISBN = isbn;
            _taken = taken;
        }
    }
}
