using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VismaConsoleApp
{
    public class Library : ILibrary
    {
        // The ammount of days during which the client has to return taken book (in days).
        private static int deadline = 60;
        private static int maxNumberOfBooksTaken = 3;
        private List<string> _books;
        private List<Person> _clients;


        public Library()
        {
            //Stores all books the library has (in JSON string form)
            _books = new List<string>();
            //Stores all the client that have ever taken a book
            _clients = new List<Person>();
        }


        public void addBook(Book book)
        {
            string bookJson = JsonSerializer.Serialize(book);
            _books.Add(bookJson);
        }

        public bool deleteBook(string bookISBN)
        {
            foreach(string bookJSON in _books)
            {
                Book book = JsonSerializer.Deserialize<Book>(bookJSON);
                if(book.ISBN.Equals(bookISBN))
                {
                    return _books.Remove(bookJSON);
                }
            }
            return false;
        }

        public List<Book> listAllBooks()
        {
            //Converts all elements from type 'string' to type 'Book'
            List<Book> books = new List<Book>();
            foreach(string s in _books)
            {
                books.Add(JsonSerializer.Deserialize<Book>(s));
            }
            return books;
        }

        public bool returnBook(string name, string surname, string bookISBN, DateTime returned)
        {
            //Checks if such client exists
            if (_clients.Exists(x => x.Name.Equals(name) && x.Surname.Equals(surname)))
            {
                Person person = _clients.Find(x => x.Name.Equals(name) && x.Surname.Equals(surname));
                //Checks if this person had taken this book.
                if (!person.BooksTaken.Exists(x => x.BookISNB.Equals(bookISBN)))
                    return false;
                BookTake take = person.BooksTaken.Find(x => x.BookISNB.Equals(bookISBN));
                person.BooksTaken.Remove(take);
                take.Returned = returned;
                //Checks the ammount of time that has passed since taking the book
                if ((take.Returned - take.Taken).TotalDays > deadline)
                    Console.WriteLine("The Book has been returned too late! This wasn't very cash money of you\n");
                //Makes returned book available again.
                Book returnedBook = listAllBooks().Find(x => x.ISBN.Equals(bookISBN));
                string returnedBookJson = JsonSerializer.Serialize(returnedBook);
                _books.Remove(returnedBookJson);
                returnedBook.IsTaken = false;
                returnedBookJson = JsonSerializer.Serialize(returnedBook);
                _books.Add(returnedBookJson);

                return true;
            }
            return false;
        }

        public bool takeBook(string name, string surname, string bookISBN, DateTime taken)
        {
            //Checks if there are any books in the library added.
            if (!_books.Any())
                return false;

            //Checks if the book exists in the library and if it's available
            bool bookAvailable = _books.Exists(x => 
            {
                Book b = JsonSerializer.Deserialize<Book>(x);
                if (b.ISBN.Equals(bookISBN) && b.IsTaken == false)
                {
                    //This code makes found book unavailable.
                    Book temp = b;
                    string bJSON = JsonSerializer.Serialize(b);
                    _books.Remove(bJSON);
                    temp.IsTaken = true;
                    string tempJSON = JsonSerializer.Serialize(temp);
                    _books.Add(tempJSON);
                    return true;

                }
                else
                    return false;
            });

            //If book not available return false;
            if (!bookAvailable)
                return false;



            //Checks if the client has used this library in the past
            if (_clients.Exists(x => x.Name.Equals(name) && x.Surname.Equals(surname)))
            {
                //Copies the person from customer list.
                Person person = _clients.Find(x => x.Name.Equals(name) && x.Surname.Equals(surname));
                if (person.NumberOfBooks >= maxNumberOfBooksTaken)
                    return false;
                _clients.Remove(person);
                //Adds a book taking record to person object.
                person.BooksTaken.Add(new BookTake(bookISBN, taken));
                person.NumberOfBooks += 1;
                //Re-adds person to customer list.
                _clients.Add(person);
                return true;
            }
            //Executes if this is the first time this client uses this library
            else
            {
                Person person = new()
                {
                    Name = name,
                    Surname = surname,
                    NumberOfBooks = 1 
                };
                person.BooksTaken.Add(new BookTake(bookISBN, taken));
                _clients.Add(person);
                return true;
            }
        }
    }
}
