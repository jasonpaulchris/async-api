﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.Api.Filters;
using Books.Api.Models;
using Books.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [Route("api/bookcollections")]
    [ApiController]
    public class BookCollectionsController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;
        private IMapper _mapper;

        public BookCollectionsController(IBooksRepository booksRepository, IMapper mapper)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({bookIds})", Name = "GetBookCollection")]
        [BooksResultFilter]
        public async Task<IActionResult> GetBookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> bookIds)
        {
            var bookEntities = await _booksRepository.GetBooksAsync(bookIds);

            if (bookIds.Count() != bookEntities.Count())
            {
                return NotFound();
            }
            return Ok(bookEntities);
        }

        [HttpPost]
        [BooksResultFilter]
        public async Task<IActionResult> CreateBookCollection([FromBody] IEnumerable<BookForCreation> bookCollection)
        {
            var bookEntities = _mapper.Map<IEnumerable<Entities.Book>>(bookCollection);
            foreach (var bookEntity in bookEntities)
            {
                _booksRepository.AddBook(bookEntity);
            }

            await _booksRepository.SaveChangesAsync();

            var booksToReturn = await _booksRepository.GetBooksAsync(bookEntities.Select(b => b.Id).ToList());

            var bookIds = string.Join(",", booksToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetBookCollection", new { bookIds },
                booksToReturn);
        }

    }
}
