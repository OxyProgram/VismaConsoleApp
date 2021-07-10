using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaConsoleApp
{
    interface ILibrary
    {
        public void addBook(Book book);
        public bool takeBook(string name, string surname, string bookISBN, DateTime taken);
        public bool returnBook(string name, string surname, string bookISBN, DateTime returned);
        public List<Book> listAllBooks();
        public bool deleteBook(string bookISBN);
    }
}
