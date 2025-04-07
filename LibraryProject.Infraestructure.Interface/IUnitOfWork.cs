using LibraryProject.Infraestructure.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infraestructure.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IBooksRepository Books { get; }
    }
}
