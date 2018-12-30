using Domain.DBEntities;
using DTO.Internal.TODO;
using DTO.Public.TODO;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Shared;
using ToDoService.Interfaces;

namespace ToDoService.Controllers
{
    [GlobalValidator]
    public class TodoController : Controller
    {
        private readonly ITracer _tracer;
        private readonly ITodoService _todoService;

        public TodoController(ITracer tracer, ITodoService todoService)
        {
            _tracer = tracer;
            _todoService = todoService;
        }
        
        [HttpGet]
        public ListTODOResponse List([FromQuery] ListTODO request)
        {
            if (string.IsNullOrWhiteSpace(request.OrderBy))
                request.OrderBy = nameof(TodoEntity.Created);
            
            return _todoService.List(request);
        }
        
        
        [HttpGet]
        public FindToDoResponse Get([FromQuery] FindToDoRequest request)
        {
            return _todoService.Find(request);
        }
        
        
        [HttpPost]
        public SaveTODOResponse Create([FromBody] CreateTODO request)
        {
            return _todoService.Create(request);
        }
        
        [HttpPost]
        public SaveTODOResponse Update([FromBody] UpdateTODO request)
        {
            return _todoService.Update(request);
        }
        
        [HttpPost]
        public DeleteTODOResponse Delete([FromBody] DeleteTODO request)
        {
            return _todoService.Delete(request);
        }
    }
}