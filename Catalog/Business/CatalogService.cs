using Catalog.Dal.Entities;
using Catalog.Dal.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Business
{
    public class CatalogService
    {
        private IFilmRepository _filmRepo;

        public CatalogService (IFilmRepository filmRepo)
        {
            _filmRepo = filmRepo;
        }

        public Film GetFilm(int id)
        {
            var result = _filmRepo.GetFilm(id);
            return (result);
        }
    }
}
