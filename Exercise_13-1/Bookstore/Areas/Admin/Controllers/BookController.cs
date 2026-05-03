using Microsoft.AspNetCore.Mvc;
using Bookstore.Models;
using System.Xml.Linq;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private BookRepository bookData { get; set; }
        private Repository<Author> authorData { get; set; }
        private Repository<Genre> genreData { get; set; }

        private const string XmlFilePath = "wwwroot/data/books.xml";

        public BookController(BookstoreContext ctx) 
        {
            bookData = new BookRepository(ctx);
            authorData = new Repository<Author>(ctx);
            genreData = new Repository<Genre>(ctx);
        }

        public ViewResult Index() {
            var books = bookData.List(new QueryOptions<Book> { 
                OrderBy = b => b.Title
            });
            return View(books);
        }

        public IActionResult ExportToXml()
        {
            var books = bookData.List(new QueryOptions<Book> {
                OrderBy = b => b.Title,
                Includes = "Genre"
            });

            var xml = new XDocument(
                new XElement("Books",
                    books.Select(b => new XElement("Book",
                        new XElement("ID", b.BookId),
                        new XElement("Title", b.Title),
                        new XElement("Price", b.Price),
                        new XElement("Genre", b.Genre?.Name ?? "N/A")
                    ))
                )
            );

            xml.Save(XmlFilePath);

            TempData["message"] = "Books exported to XML successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToActionResult Select(int id, string operation)
        {
            switch (operation.ToLower())
            {
                case "edit":
                    return RedirectToAction("Edit", new { id });
                case "delete":
                    return RedirectToAction("Delete", new { id });
                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ViewResult Add()
        {
            var vm = new BookViewModel();
            LoadViewData(vm);
            return View("Book", vm);
        }

        [HttpPost]
        public IActionResult Add(BookViewModel vm)
        {
            if (ModelState.IsValid) {
                bookData.AddNewAuthors(vm.Book, vm.SelectedAuthors, authorData);
                bookData.Insert(vm.Book);
                bookData.Save();  
                TempData["message"] = $"{vm.Book.Title} added to Books.";
                return RedirectToAction("Index");
            }
            else {
                LoadViewData(vm);
                return View("Book", vm);
            }
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var vm = new BookViewModel {
                Book = GetBook(id)
            };
            LoadViewData(vm);
            return View("Book", vm);
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel vm)
        {
            if (ModelState.IsValid) {
                var book = GetBook(vm.Book.BookId);
                book.Title = vm.Book.Title;
                book.GenreId = vm.Book.GenreId;
                book.Price = vm.Book.Price;
                bookData.AddNewAuthors(book, vm.SelectedAuthors, authorData);
                bookData.Save(); 
                TempData["message"] = $"{vm.Book.Title} updated.";
                return RedirectToAction("Index");
            }
            else {
                LoadViewData(vm);
                return View("Book", vm);
            }
        }

        [HttpGet]
        public ViewResult Delete(int id)
        {
            var vm = new BookViewModel {
                Book = bookData.Get(id) ?? new Book()
            };
            return View("Book", vm);
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            bookData.Delete(book);
            bookData.Save();
            TempData["message"] = $"{book.Title} removed from Books.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Related(string id)
        {
            var parts = id.Split('-');
            string type = parts[0];
            id = parts[1];  

            var options = new QueryOptions<Book> {
                OrderBy = b => b.Title, 
                Includes = "Authors, Genre"
            };
            if (type.EqualsNoCase("author")) 
                options.Where = b => b.Authors.Any(ba => ba.AuthorId == id.ToInt()); 
            else if (type.EqualsNoCase("genre")) 
                options.Where = b => b.GenreId.ToLower() == id;

            return View(bookData.List(options));
        }

        private Book GetBook(int id)
        {
            var options = new QueryOptions<Book> {
                Where = b => b.BookId == id,
                Includes = "Authors"
            };
            return bookData.Get(options) ?? new Book();
        }

        private void LoadViewData(BookViewModel vm)
        {
            vm.SelectedAuthors = vm.Book.Authors?.Select(
                ba => ba.AuthorId).ToArray() ?? null!;
            vm.Authors = authorData.List(new QueryOptions<Author> {
                OrderBy = a => a.FirstName
            });
            vm.Genres = genreData.List(new QueryOptions<Genre> {
                OrderBy = g => g.Name
            });
        }
    }
}
