using System.Linq;
using Domain.DBEnities;
using Domain.Interfaces;

namespace DTO.Public.TODO
{
    public class TODOFilter : IFilter<TodoEntity>
    {
        /// <summary>
        /// Any text in Header or Description
        /// </summary>
        public string Text { get; set; }

        public IQueryable<TodoEntity> ApplyFiler(IQueryable<TodoEntity> data)
        {
            if (!string.IsNullOrEmpty(Text))
                data = data.Where(x => (x.Title != null && x.Title.Contains(Text))
                                       || (x.Description != null && x.Description.Contains(Text)));

            return data;
        }
    }
}