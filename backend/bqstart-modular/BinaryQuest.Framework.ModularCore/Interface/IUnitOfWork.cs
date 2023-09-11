using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryQuest.Framework.ModularCore.Interface
{
    public interface IUnitOfWork
    {
        void Dispose();
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;
        Task SaveAsync();

    }
}
