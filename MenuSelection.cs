using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaConsoleApp
{
    static class MenuSelection
    {
        public static Library library = new();

        public static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Menu:\n 1. Add a new book to the library\n 2. Take a book from the library\n" +
                " 3. Return a book\n 4. List all books\n 5.Delete a book\n");
            string s;
            s = Console.ReadLine();

            switch (s)
            {
                case "1":
                    AddNewBook();
                    break;
                case "2":
                    TakeBook();
                    break;
                case "3":
                    ReturnBook();
                    break;
                case "4":
                    ListBooks();
                    break;
                case "5":
                    DeleteBook();
                    break;
                default:
                    break;
            }
        }

       
        public static void AddNewBook()
        {
            string name, author, category, language, isbn, temp;
            DateTime date = DateTime.Now;
            Console.WriteLine("Enter the name of the book:  ");
            name = Console.ReadLine();
            Console.WriteLine("Enter the author:  ");
            author = Console.ReadLine();
            Console.WriteLine("Enter category:  ");
            category = Console.ReadLine();
            Console.WriteLine("Enter language: ");
            language = Console.ReadLine();
            Console.WriteLine("Enter ISBN:  ");
            isbn = Console.ReadLine();
            Console.WriteLine("Enter publication date (MM/DD/YYYY):   ");
            temp = Console.ReadLine();
            string format = "d";
            CultureInfo provider = CultureInfo.InvariantCulture;

            //Tries to parse string into DateTime
            try
            {
                date = DateTime.ParseExact(temp, format, provider);
            } catch(FormatException)
            {
                Console.WriteLine("Wrong date!");
            }
            Book book = new()
            {
                Name = name,
                Author = author,
                Category = category,
                PublicationDate = date,
                Language = language,
                ISBN = isbn,
                IsTaken = false
            };

            //Adds books to the book catalog in the library
            library.addBook(book);
            Console.WriteLine("Book successfully added! ");
            Console.ReadKey();
            Menu();
        }

        public static void TakeBook()
        {
            string name, surname, isbn;
            Console.WriteLine("Enter your name:  ");
            name = Console.ReadLine();
            Console.WriteLine("Enter your surname: ");
            surname = Console.ReadLine();
            Console.WriteLine("Enter ISBN of the book you want to take:  ");
            isbn = Console.ReadLine();

            //Checks if such book is available
            if (!library.takeBook(name, surname, isbn, DateTime.Now))
            {
                Console.WriteLine("Book taken unsuccessfully!   ");
                Console.ReadKey();
                Menu();
            }
            Console.WriteLine("Book successfully taken! ");
            Console.ReadKey();
            Menu();
        }

        public static void ReturnBook()
        {
            string name, surname, isbn;
            Console.WriteLine("Enter your name:  ");
            name = Console.ReadLine();
            Console.WriteLine("Enter your surname: ");
            surname = Console.ReadLine();
            Console.WriteLine("Enter ISBN of the book you want to take:  ");
            isbn = Console.ReadLine();

            //Checks if such book was taken by this person
            if(library.returnBook(name, surname, isbn, DateTime.Now))
            {
                Console.WriteLine("Book successfully returned! ");
                Console.ReadKey();
                Menu();
            }
            Console.WriteLine("Book returned unsuccessfully! ");
            Console.ReadKey();
            Menu();
        }

        public static void ListBooks()
        {
            Console.WriteLine("Select how to filter the content: \n1. Filter by Author\n" +
                "2. Filter by Category \n3. Filter by language\n4. Filter by ISBN \n5. Filter by name\n 6. Filter by availability");
            string k = Console.ReadLine();
            List<Book> list = library.listAllBooks();
            List<Book> sortedList = list;
            switch(k)
            {
                case "1":
                    sortedList = list.OrderBy(o => o.Author).ToList();
                    break;
                case "2":
                    sortedList = list.OrderBy(o => o.Category).ToList();
                    break;
                case "3":
                    sortedList = list.OrderBy(o => o.Language).ToList();
                    break;
                case "4":
                    sortedList = list.OrderBy(o => o.ISBN).ToList();
                    break;
                case "5":
                    sortedList = list.OrderBy(o => o.Name).ToList();
                    break;
                case "6":
                    sortedList = list.OrderBy(o => o.IsTaken).ToList();
                    break;
                default:
                    Console.WriteLine("Filtering unsuccessful!  ");
                    break;
            }
            foreach(Book b in sortedList)
            {
                b.toString();
            }
            Console.ReadKey();
            Menu();
        }

        public static void DeleteBook()
        {
            string isbn;
            Console.WriteLine("Enter ISBN:  ");
            isbn = Console.ReadLine();

            //Checks if such book exists
            if(library.deleteBook(isbn))
            {
                Console.WriteLine("Book successfully deleted! ");
                Console.ReadKey();
                Menu();
            }
            Console.WriteLine("Such book doesnt exist!");
            Console.ReadKey();
            Menu();
        }
    }
}
