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
            //Converts Book Object to type json string
            string bookJson = BookObjectToJson(book);
            _books.Add(bookJson);
        }

        public bool deleteBook(string bookISBN)
        {
            foreach(string bookJSON in _books)
            {
                Book book = BookJsonToObject(bookJSON);
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
            List<Book> books = new();
            foreach(string bookJson in _books)
            {
                books.Add(BookJsonToObject(bookJson));
            }
            return books;
        }

        public bool returnBook(string name, string surname, string bookISBN, DateTime returned)
        {
            //Checks if such client exists
            if (clientExists(name, surname))
            {
                Person person = findClient(name, surname);
                //Checks if this person had taken this book.
                if (!person.BooksTaken.Exists(x => x.BookISNB.Equals(bookISBN)))
                    return false;

                _clients.Remove(person);

                //Removes a person's record of having taken a book
                BookTake take = person.BooksTaken.Find(x => x.BookISNB.Equals(bookISBN));
                person.BooksTaken.Remove(take);
                person.NumberOfBooks -= 1;
                _clients.Add(person);

                take.Returned = returned; //Marks when was the book returned

                //Checks the ammount of time that has passed since taking the book
                if ((take.Returned - take.Taken).TotalDays > deadline)
                    Console.WriteLine("The Book has been returned too late! This wasn't very cash money of you\n");

                //Makes returned book available again.
                Book returnedBook = listAllBooks().Find(x => x.ISBN.Equals(bookISBN));
                string returnedBookJson = BookObjectToJson(returnedBook);
                _books.Remove(returnedBookJson);
                returnedBook.IsTaken = false;
                returnedBookJson = BookObjectToJson(returnedBook);
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

            //If book is not available return false;
            if (!IsBookAvailable(bookISBN))
                return false;

            //Checks if the client has used this library in the past
            if (clientExists(name, surname))
            {
                //Copies the person from clients list.
                Person person = findClient(name, surname);

                //Checks if the client has taken more book than allowed
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

        private static string BookObjectToJson(Book book)
        {
            string bookJson = JsonSerializer.Serialize(book);
            return bookJson;
        }

        private static Book BookJsonToObject(string bookJson)
        {
            Book book = JsonSerializer.Deserialize<Book>(bookJson);
            return book;
        }

        private bool clientExists(string name, string surname)
        {
            return _clients.Exists(x => x.Name.Equals(name) && x.Surname.Equals(surname));
        }

        private Person findClient(string name, string surname)
        {
            return _clients.Find(x => x.Name.Equals(name) && x.Surname.Equals(surname));
        }

        private bool IsBookAvailable(string bookISBN)
        {
            return _books.Exists(x =>
            {
                Book book = BookJsonToObject(x);
                if (book.ISBN.Equals(bookISBN) && book.IsTaken == false)
                {
                    //This code makes found book unavailable.
                    string bookJson = BookObjectToJson(book);
                    _books.Remove(bookJson);
                    book.IsTaken = true;
                    bookJson = BookObjectToJson(book);
                    _books.Add(bookJson);
                    return true;

                }
                else
                    return false;
            });
        }

    }
}
