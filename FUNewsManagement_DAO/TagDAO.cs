using FUNewsManagement_BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_DAO
{
    public class TagDAO
    {
        private readonly FunewsManagementContext _context;
        public TagDAO(FunewsManagementContext context)
        {
            _context = context;
        }

        public IList<Tag> GetAll() => _context.Tags.ToList();
        public Tag GetById(int id) => _context.Tags.FirstOrDefault(x => x.TagId == id);
    }
}
