using FUNewsManagement_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_Repository.Interfaces
{
    public interface ITagRepository
    {
        IList<Tag> GetAll();
        Tag GetById(int id);
    }
}
