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

        public List<Film> GetAllFilms()
        {
            return _filmRepo.GetAllFilms();
        }

        public List<Film> GetSpecificFilms()
        {
            return _filmRepo.GetSpecificFilms();
        }

        public Film GetOneFilm(int id)
        {
            return _filmRepo.GetOneFilm(id);
        }


        public List<December> GetDateFilms()
        {
            return _filmRepo.GetProgramFilms();
        }
    }
}
