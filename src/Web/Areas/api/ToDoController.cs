using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Domain.DBEntities;
using DTO.Public.TODO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Utils;
using Utils.WebRequests;

namespace Web.Areas.api
{
    [Authorize]
    [GlobalValidator]
    [Area("api")]
    [ApiVersion("0.1")]
    [Route("api/[controller]/[action]")]
    public class ToDoController : Controller
    {
        private readonly IMapper _mapper;

        public ToDoController(
            IMapper mapper
            )
        {
            _mapper = mapper;
        }
        
        /// <summary>
        /// list to-do items with pagination 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public ListTODOResponse List(ListTODORequest request)
        {
            const int max = 100;
            var items = new List<TodoEntity>(max);
            for (var i = 0; i < max; i++)
            {
                items.Add(new TodoEntity()
                {
                    UserId = Guid.NewGuid(),
                    PublicId = Guid.NewGuid(),
                    Description = "assa " + i,
                    Created = DateTime.Now.AddDays(-1),
                    Updated = DateTime.Now.AddHours(-2)
                });                
            }

            //var show = request.Filter.ApplyFiler(items.AsQueryable()).AsPagination(request);
            var data = request.Sort(items.AsQueryable());
            data = request.Filter(data);
            var paged = data.AsPagination(request);
            var publicEntities = _mapper.Map<TodoPublicEntity[]>(paged.Items);
            return new ListTODOResponse(publicEntities, paged.TotalItems, request);
        }
        
        /// <summary>
        /// load to-do-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public EditTODOResponse Get(EditTODORequest request)
        {
            return new EditTODOResponse()
            {
                PublicId = request.PublicId.Value,
                Description = "asd",
                Title = "asds",
                Created = DateTime.Now.AddDays(-1),
                Updated = DateTime.Now.AddMinutes(-2)
            };
        }
        
        /// <summary>
        /// update to-do-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public SaveTODOResponse Update([FromBody] SaveTODORequest request)
        {
            return new SaveTODOResponse()
            {
                Data = new TodoPublicEntity()
                {
                    PublicId = request.PublicId ?? Guid.NewGuid(),
                    Description = "asd",
                    Title = "asds",
                    Created = DateTime.Now.AddDays(-1),
                    Updated = DateTime.Now.AddMinutes(-2)
                }
            };
        }
        
        /// <summary>
        /// create new to-do-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public SaveTODOResponse Create([FromBody] SaveTODORequest request)
        {
            request.PublicId = null;
            return Update(request);
        }
        
        /// <summary>
        /// delete to-do-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public DeleteTODOResponse Delete([FromBody] DeleteTODORequest request)
        {
            return new DeleteTODOResponse()
            {
                Data = true,
            };
        }
    }
}