﻿using Catalog.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Dal.Repository.Abstraction
{
    public interface IFilmRepository
    {
        Film GetFilm(int id);
        List<Film> GetAllFilms();
        List<Film> GetOneFilm();
        List<Film> GetProgramFilms();
        List<Film> GetSpecificFilms();
        List<December> GetDateFilms();
    }
}
