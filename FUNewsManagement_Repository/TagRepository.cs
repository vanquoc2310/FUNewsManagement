using FUNewsManagement_BusinessObjects;
using FUNewsManagement_DAO;
using FUNewsManagement_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly TagDAO _dao;
        public TagRepository(FunewsManagementContext context)
        {
            _dao = new TagDAO(context);
        }

        public IList<Tag> GetAll() => _dao.GetAll();
        public Tag GetById(int id) => _dao.GetById(id);
    }
}
