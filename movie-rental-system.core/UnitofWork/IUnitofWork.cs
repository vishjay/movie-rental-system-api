using movie_rental_system.core.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace movie_rental_system.core.UnitofWork
{
    public interface IUnitofWork : IDisposable
    {
        IMovieRepository Movies { get; }
        void Complete();
    }
}
