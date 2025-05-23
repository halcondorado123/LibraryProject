﻿using LibraryProject.Infraestructure.Interface;
using LibraryProject.Infraestructure.Interface.Library;

namespace LibraryProject.Infraestructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IBooksRepository _booksRepository;

        public UnitOfWork(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        // Cambio aquí
        public IBooksRepository Books => _booksRepository;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}