using System;
using Domain.DBEntities;
using DTO.Internal.TOBUY;
using DTO.Public.TOBUY;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Shared;
using ToBuyService.Interfaces;

namespace ToBuyService.Controllers
{
    [GlobalValidator]
    public class TobuyController : Controller
    {
        private readonly ITracer _tracer;
        private readonly ITobuyService _tobuyService;

        public TobuyController(ITracer tracer, ITobuyService tobuyService)
        {
            _tracer = tracer;
            _tobuyService = tobuyService;
        }

        [HttpGet]
        public ListTOBUYResponse List([FromQuery] ListTOBUY request)
        {
            if (string.IsNullOrWhiteSpace(request.OrderBy))
                request.OrderBy = nameof(TobuyEntity.Created);
            
            return _tobuyService.List(request);
        }
        
        
        [HttpGet]
        public FindToBuyResponse Get([FromQuery] FindToBuyRequest request)
        {
            return _tobuyService.Find(request);
        }
        
        
        [HttpPost]
        public SaveTOBUYResponse Create([FromBody] CreateTOBUY request)
        {
            return _tobuyService.Create(request);
        }
        
        [HttpPost]
        public SaveTOBUYResponse Update([FromBody] UpdateTOBUY request)
        {
            return _tobuyService.Update(request);
        }
        
        [HttpPost]
        public DeleteTOBUYResponse Delete([FromBody] DeleteTOBUY request)
        {
            return _tobuyService.Delete(request);
        }
    }
}