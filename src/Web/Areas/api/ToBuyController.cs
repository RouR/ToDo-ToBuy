using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DBEntities;
using Domain.Enums;
using Domain.Models;
using DTO.Internal.TOBUY;
using DTO.Public.TOBUY;
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
    public class ToBuyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ToBuyServiceClient _client;

        public ToBuyController(
            IMapper mapper,
            ToBuyServiceClient client
            )
        {
            _mapper = mapper;
            _client = client;
        }
        
        /// <summary>
        /// list to-buy items with pagination 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListTOBUYResponse> List(ListTOBUYRequest request)
        {
            var data = _mapper.Map<ListTOBUY>(request);
            data.UserId = HttpContext.GetUserId();
            return await _client.Tobuy_List(data);
        }
        
        /// <summary>
        /// load to-buy-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<EditTOBUYResponse> Get(EditTOBUYRequest request)
        {
            if(request.PublicId == null)
                return new EditTOBUYResponse().SetError("Id required") as EditTOBUYResponse;

            var data = new FindToBuyRequest()
            {
                UserId = HttpContext.GetUserId(),
                PublicId = request.PublicId.Value
            };
            var ret= await _client.Tobuy_Get(data);
            
            return new EditTOBUYResponse()
            {
                Message = ret.Message,
                HasError = ret.HasError,
                Data = _mapper.Map<TOBUYPublicEntity>(ret.Data)
            };
        }
        
        /// <summary>
        /// update to-buy-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SaveTOBUYResponse> Update([FromBody] SaveTOBUYRequest request)
        {
            if(request.PublicId == null)
                return new SaveTOBUYResponse().SetError("Id required") as SaveTOBUYResponse;
            
            var data = _mapper.Map<UpdateTOBUY>(request);
            data.UserId = HttpContext.GetUserId();
            
            var ret= await _client.Tobuy_Update(data);

            return ret;
        }
        
        /// <summary>
        /// create new to-buy-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SaveTOBUYResponse> Create([FromBody] SaveTOBUYRequest request)
        {
            if(request.PublicId != null)
                return new SaveTOBUYResponse().SetError("Use update method for edit exist item") as SaveTOBUYResponse;
            request.PublicId = Guid.NewGuid();
            
            var data = _mapper.Map<CreateTOBUY>(request);
            data.UserId = HttpContext.GetUserId();
            
            var ret= await _client.Tobuy_Create(data);

            return ret;
        }
        
        /// <summary>
        /// delete to-buy-item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DeleteTOBUYResponse> Delete([FromBody] DeleteTOBUYRequest request)
        {
            if(request.PublicId == Guid.Empty)
                return new DeleteTOBUYResponse().SetError("Id required") as DeleteTOBUYResponse;
            
            var data = _mapper.Map<DeleteTOBUY>(request);
            data.UserId = HttpContext.GetUserId();
            
            var ret= await _client.Tobuy_Delete(data);

            return ret;
        }
    }
}