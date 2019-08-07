using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Books.Api.Contexts;
using Books.Api.Entities;
using Books.Api.ExternalModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Books.Api.Services
{
    public class BooksRepository : IBooksRepository, IDisposable
    {
        private BooksContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public BooksRepository(BooksContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            return await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Entities.Book>> GetBooksAsync(IEnumerable<Guid> bookIds)
        {
            return await _context.Books.Where(b => b.bookIds.Contains(b.Id).Include(b => b.Author).ToListAsync();
        }

        public void AddBook(Book bookToAdd)
        {
            if (bookToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookToAdd));
            }

            _context.Add(bookToAdd);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetBooksAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BookCover> GetBookCoverAsync(string coverId)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"http://localhost:52644/api/bookcovers/{coverId}");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<BookCover>(
                    await response.Content.ReadAsStringAsync());
            }

            return null;
        }
    }
}
