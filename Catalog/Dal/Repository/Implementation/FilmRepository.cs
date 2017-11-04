using Catalog.Configuration;
using Catalog.Dal.Context;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Dal.Repository.Abstraction;
using Catalog.Dal.Entities;
using System.Linq;

namespace Catalog.Dal.Repository.Implementation
{
    class FilmRepository : IFilmRepository
    {
        private readonly CatalogOptions _options;
        private ILogger<FilmRepository> _logger;
        private readonly CatalogDbContext _context;

        public FilmRepository(IOptions<CatalogOptions> options, ILogger<FilmRepository> logger, CatalogDbContext catalogDbContext)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
            _context = catalogDbContext;
        }

        public Film GetFilm(int id)
        {
            var result = _context.Film.FirstOrDefault(s => (s.IdFilm == id));
            return result;
        }
    }
}
