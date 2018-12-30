using System;
using System.Linq;
using AutoMapper;
using Domain.DBEntities;
using DTO.Internal.TOBUY;
using DTO.Public.TOBUY;
using ToBuyService.DAL;
using ToBuyService.Interfaces;
using Utils.WebRequests;

namespace ToBuyService.Services
{
    public class TobuyService: ITobuyService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TobuyService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public ListTOBUYResponse List(ListTOBUY request)
        {
            var items = GetActualForUser(request.UserId);
                
            items = request.Sort(items);    
            
            var page = items.AsPagination(request);

            return new ListTOBUYResponse(page.AutoMap<TobuyEntity, TOBUYPublicEntity>(_mapper));
        }

        public FindToBuyResponse Find(FindToBuyRequest request)
        {
            var item = GetActualForUser(request.UserId)
                .SingleOrDefault(x=> x.PublicId == request.PublicId);

            var ret = new FindToBuyResponse();
            if (item == null)
                ret.SetError($"Item with id {request.PublicId} not found");
            else
                ret.Data = item;
            return ret;
        }

        public SaveTOBUYResponse Create(CreateTOBUY request)
        {
            var ret = new SaveTOBUYResponse();
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
                var newItem = _dbContext.Tobuy.Add(new TobuyEntity()
                {
                    PublicId = request.PublicId,
                    UserId = request.UserId,
                    Price = request.Price,
                    Qty = request.Qty,
                    Name = request.Name,
                    DueToUtc = request.DueToUtc,
                    IsDeleted = false,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                });
                _dbContext.SaveChanges();

                ret.Data = _mapper.Map<TOBUYPublicEntity>(newItem.Entity);
            }
            catch (Exception e)
            {
                ret.SetError("Can`t create - " + e.Message);
            }

            return ret;
        }

        public SaveTOBUYResponse Update(UpdateTOBUY request)
        {
            var ret = new SaveTOBUYResponse();
            
            var item = GetActualForUser(request.UserId)
                .SingleOrDefault(x=> x.PublicId == request.PublicId);

            if (item == null)
                ret.SetError($"Item with id {request.PublicId} not found");
            else
            {
                item.Name = request.Name;
                item.DueToUtc = request.DueToUtc;
                item.Price = request.Price;
                item.Qty = request.Qty;
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
                
                ret.Data = _mapper.Map<TOBUYPublicEntity>(item);
            }
            return ret;
        }

        public DeleteTOBUYResponse Delete(DeleteTOBUY request)
        {
            var ret = new DeleteTOBUYResponse();
            
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

        private IQueryable<TobuyEntity> GetActualForUser(Guid userId)
        {
            return _dbContext.Tobuy
                .Where(x => x.UserId == userId && !x.IsDeleted);
        }
    }
}