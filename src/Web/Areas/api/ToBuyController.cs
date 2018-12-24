using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Domain.DBEnities;
using Domain.Enums;
using Domain.Models;
using DTO.Public.TOBUY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utils.WebRequests;
using Web.Utils;

namespace Web.Areas.api
{
    [Authorize]
    [GlobalValidator]
    [Area("api")]
    [ApiVersion("0.1")]
    [Route("api/[controller]/[action]")]
    public class ToBuyController : Controller
    {
        private readonly IMapper _mapper;

        public ToBuyController(
            IMapper mapper
            )
        {
            _mapper = mapper;
        }
        
        /// <summary>
        /// list to-buy items with pagination 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public ListTOBUYResponse List(ListTOBUYRequest request)
        {
            const int max = 100;
            var items = new List<TobuyEntity>(max);
            for (var i = 0; i < max; i++)
            {
                items.Add(new TobuyEntity()
                {
                    UserId = Guid.NewGuid(),
                    PublicId = Guid.NewGuid(),
                    Name = "nnn " + i,
                    Qty = 4 % (1+i),
                    Price = i%2 ==0 ? new Price(){Amount = 2*i, Currency = Currency.Euro} : null,
                    DueToUtc = i%2 == 0 ? (DateTime?)null : DateTime.UtcNow,
                    Created = DateTime.Now.AddDays(-1),
                    Updated = DateTime.Now.AddHours(-2)
                });                
            }

            //var show = request.Filter.ApplyFiler(items.AsQueryable()).AsPagination(request);
            var data = request.Sort(items.AsQueryable());
            var paged = data.AsPagination(request);
            var publicEntities = _mapper.Map<TOBUYPublicEntity[]>(paged.Items);
            return new ListTOBUYResponse(publicEntities, paged.TotalItems, request);
        }
        
        /// <summary>
        /// load to-buy-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public EditTOBUYResponse Get(EditTOBUYRequest request)
        {
            return new EditTOBUYResponse()
            {
                PublicId = request.PublicId.Value,
                Name = "nnn ",
                Qty = 22,
                Price = new Price(){Amount = 42, Currency = Currency.Euro},
                DueToUtc = DateTime.UtcNow,
                Created = DateTime.Now.AddDays(-1),
                Updated = DateTime.Now.AddMinutes(-2)
            };
        }
        
        /// <summary>
        /// update to-buy-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public SaveTOBUYResponse Update([FromBody] SaveTOBUYRequest request)
        {
            return new SaveTOBUYResponse()
            {
                Data = new TOBUYPublicEntity()
                {
                    PublicId = request.PublicId ?? Guid.NewGuid(),
                    Name = "nnn ",
                    Qty = 22,
                    Price = new Price(){Amount = 42, Currency = Currency.Euro},
                    DueToUtc = DateTime.UtcNow,
                    Created = DateTime.Now.AddDays(-1),
                    Updated = DateTime.Now.AddMinutes(-2)
                }
            };
        }
        
        /// <summary>
        /// create new to-buy-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public SaveTOBUYResponse Create([FromBody] SaveTOBUYRequest request)
        {
            request.PublicId = null;
            return Update(request);
        }
        
        /// <summary>
        /// delete to-buy-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public DeleteTOBUYResponse Delete([FromBody] DeleteTOBUYRequest request)
        {
            return new DeleteTOBUYResponse()
            {
                Data = true,
            };
        }
    }
}