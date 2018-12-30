using System;
using System.Linq;
using AutoMapper;
using Domain.DBEntities;
using DTO.Internal.TODO;
using DTO.Public.TOBUY;
using DTO.Public.TODO;
using ToDoService.DAL;
using ToDoService.Interfaces;
using Utils.WebRequests;

namespace ToDoService.Services
{
    public class TodoService: ITodoService
    {
        
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TodoService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public ListTODOResponse List(ListTODO request)
        {
            var items = GetActualForUser(request.UserId);
                
            items = request.Filter(items);    
            items = request.Sort(items);    
            
            var page = items.AsPagination(request);

            return new ListTODOResponse(page.AutoMap<TodoEntity, TodoPublicEntity>(_mapper));
        }

        public FindToDoResponse Find(FindToDoRequest request)
        {
            var item = GetActualForUser(request.UserId)
                .SingleOrDefault(x=> x.PublicId == request.PublicId);

            var ret = new FindToDoResponse();
            if (item == null)
                ret.SetError($"Item with id {request.PublicId} not found");
            else
                ret.Data = item;
            return ret;
        }

        public SaveTODOResponse Create(CreateTODO request)
        {
            var ret = new SaveTODOResponse();
            if (request.PublicId == Guid.Empty)
            {
                ret.SetError("PublicId must be pregenerated");
                return ret;
            }
            
            if (request.UserId == Guid.Empty)
            {
                ret.SetError("Security violation - UserId is empty");
                return ret;
            }

            try
            {
                var newItem = _dbContext.Todos.Add(new TodoEntity()
                {
                    PublicId = request.PublicId,
                    UserId = request.UserId,
                    Title = request.Title,
                    Description = request.Description,
                    IsDeleted = false,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                });
                _dbContext.SaveChanges();

                ret.Data = _mapper.Map<TodoPublicEntity>(newItem.Entity);
            }
            catch (Exception e)
            {
                ret.SetError("Can`t create - " + e.Message);
            }

            return ret;
        }

        public SaveTODOResponse Update(UpdateTODO request)
        {
            var ret = new SaveTODOResponse();
            
            var item = GetActualForUser(request.UserId)
                .SingleOrDefault(x=> x.PublicId == request.PublicId);

            if (item == null)
                ret.SetError($"Item with id {request.PublicId} not found");
            else
            {
                item.Title = request.Title;
                item.Description = request.Description;
                item.Updated = DateTime.UtcNow;

                try
                {
                    _dbContext.SaveChanges();
                
                    item = GetActualForUser(request.UserId)
                        .SingleOrDefault(x => x.PublicId == request.PublicId);
                }
                catch (Exception e)
                {
                    ret.SetError(e.Message);
                    return ret;
                }
                
                ret.Data = _mapper.Map<TodoPublicEntity>(item);
            }
            return ret;
        }

        public DeleteTODOResponse Delete(DeleteTODO request)
        {
            var ret = new DeleteTODOResponse();
            
            var item = GetActualForUser(request.UserId)
                .SingleOrDefault(x=> x.PublicId == request.PublicId);

            if (item == null)
                ret.SetError($"Item with id {request.PublicId} not found");
            else
            {
                item.IsDeleted = true;
                item.Updated = DateTime.UtcNow;

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    ret.SetError(e.Message);
                    return ret;
                }
                
                _dbContext.SaveChanges();
                
                ret.Data = true;
            }
            return ret;
        }
        
        private IQueryable<TodoEntity> GetActualForUser(Guid userId)
        {
            return _dbContext.Todos
                .Where(x => x.UserId == userId && !x.IsDeleted);
        }
    }
}