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

        public List<Film> GetProgramFilms()
        {
            return _filmRepo.GetProgramFilms();
        }

        public List<Film> GetOneFilm()
        {
            return _filmRepo.GetOneFilm();
        }


        public List<December> GetDateFilms()
        {
            return _filmRepo.GetDateFilms();
        }
    }
}
