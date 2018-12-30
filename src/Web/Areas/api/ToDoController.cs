using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DBEntities;
using DTO.Internal.TODO;
using DTO.Public.TODO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Utils;
using Utils.WebRequests;
using Web.Utils;

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
        private readonly ToDoServiceClient _client;

        public ToDoController(
            IMapper mapper,
            ToDoServiceClient client
            )
        {
            _mapper = mapper;
            _client = client;
        }
        
        /// <summary>
        /// list to-do items with pagination 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListTODOResponse> List(ListTODORequest request)
        {
            var data = _mapper.Map<ListTODO>(request);
            data.UserId = HttpContext.GetUserId();
            return await _client.Todo_List(data);
        }
        
        /// <summary>
        /// load to-do-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EditTODOResponse> Get(EditTODORequest request)
        {
            if(request.PublicId == null)
                return new EditTODOResponse().SetError("Id required") as EditTODOResponse;

            var data = new FindToDoRequest()
            {
                UserId = HttpContext.GetUserId(),
                PublicId = request.PublicId.Value
            };
            var ret= await _client.Todo_Get(data);
            
            return new EditTODOResponse()
            {
                Message = ret.Message,
                HasError = ret.HasError,
                Data = _mapper.Map<TodoPublicEntity>(ret.Data)
            };
        }
        
        /// <summary>
        /// update to-do-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SaveTODOResponse> Update([FromBody] SaveTODORequest request)
        {
            if(request.PublicId == null)
                return new SaveTODOResponse().SetError("Id required") as SaveTODOResponse;
            
            var data = _mapper.Map<UpdateTODO>(request);
            data.UserId = HttpContext.GetUserId();
            
            var ret= await _client.Todo_Update(data);

            return ret;
        }
        
        /// <summary>
        /// create new to-do-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SaveTODOResponse> Create([FromBody] SaveTODORequest request)
        {
            if(request.PublicId != null)
                return new SaveTODOResponse().SetError("Use update method for edit exist item") as SaveTODOResponse;
            request.PublicId = Guid.NewGuid();
            
            var data = _mapper.Map<CreateTODO>(request);
            data.UserId = HttpContext.GetUserId();
            
            var ret= await _client.Todo_Create(data);

            return ret;
        }
        
        /// <summary>
        /// delete to-do-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DeleteTODOResponse> Delete([FromBody] DeleteTODORequest request)
        {
            if(request.PublicId == Guid.Empty)
                return new DeleteTODOResponse().SetError("Id required") as DeleteTODOResponse;
            
            var data = _mapper.Map<DeleteTODO>(request);
            data.UserId = HttpContext.GetUserId();
            
            var ret= await _client.Todo_Delete(data);

            return ret;
        }
    }

    
}