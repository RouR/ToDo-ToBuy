using System;
using System.Linq;
using AutoMapper;
using Domain.DBEntities;
using DTO;
using DTO.Internal.TODO;
using DTO.Public.TODO;
using Microsoft.EntityFrameworkCore;
using ToDoService.DAL;
using ToDoService.Services;
using Xunit;

namespace TodoMicroserviceTests
{
    public class TodoServiceTests
    {
        [Fact]
        public void TodoService_Should_CRUD()
        {
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingDTOProfile()); });
            var mapper = mockMapper.CreateMapper();

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoService(context, mapper);


                Assert.Equal(0, context.Todos.Count());
                var list1 = service.List(new ListTODO() {Page = 1, PageSize = 10, UserId = userId});
                Assert.Equal(0, list1.TotalItems);
                Assert.Equal(0, list1.Items.Count());

                for (var i = 0; i < 50; i++)
                {
                    var ret1 = service.Create(new CreateTODO()
                    {
                        Title = "item " + i, PublicId = Guid.NewGuid(), UserId = userId
                    });
                    Assert.Equal(false, ret1.HasError);
                }

                Assert.Equal(50, context.Todos.Count());

                var list2 = service.List(new ListTODO() {Page = 1, PageSize = 10, UserId = userId});
                Assert.Equal(50, list2.TotalItems);
                Assert.Equal(10, list2.Items.Count());

                var editId = context.Todos.Skip(3).First().PublicId;

                var edit1 = service.Find(new FindToDoRequest() {PublicId = editId, UserId = userId});
                Assert.Equal(false, edit1.HasError);
                Assert.NotNull(edit1.Data);
                var origField = edit1.Data.Title;

                const string newValue = "new value";
                service.Update(new UpdateTODO() {Title = newValue, PublicId = editId, UserId = userId});

                var edit2 = service.Find(new FindToDoRequest() {PublicId = editId, UserId = userId});
                Assert.Equal(false, edit1.HasError);
                Assert.NotNull(edit1.Data);
                Assert.Equal(newValue, edit1.Data.Title);
                Assert.Equal(newValue, edit1.Data.Title);
                Assert.Equal(editId, edit1.Data.PublicId);

                service.Delete(new DeleteTODO() {PublicId = editId, UserId = userId});
                Assert.Equal(50, context.Todos.Count());
                var list3 = service.List(new ListTODO() {Page = 1, PageSize = 10, UserId = userId});
                Assert.Equal(49, list3.TotalItems);

                var newId = Guid.NewGuid();
                var cr1 = service.Create(new CreateTODO() {Title = newValue, PublicId = newId, UserId = userId});
                Assert.Equal(false, cr1.HasError);
                Assert.NotNull(cr1.Data);
                Assert.Equal(newValue, cr1.Data.Title);
                Assert.Equal(newId, cr1.Data.PublicId);

                Assert.Equal(51, context.Todos.Count());
                var list4 = service.List(new ListTODO() {Page = 1, PageSize = 10, UserId = userId});
                Assert.Equal(50, list4.TotalItems);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(50, context.Todos.Count(x => !x.IsDeleted));
                Assert.Equal(51, context.Todos.Count());
            }
        }


        [Fact]
        public void TodoService_Should_Filter()
        {
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingDTOProfile()); });
            var mapper = mockMapper.CreateMapper();

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoService(context, mapper);

                Assert.Equal(0, context.Todos.Count());
                for (var i = 0; i < 50; i++)
                {
                    var ret1 = service.Create(new CreateTODO()
                    {
                        Title = "item " + i, PublicId = Guid.NewGuid(), UserId = userId
                    });
                }

                Assert.Equal(50, context.Todos.Count());
                service.Create(new CreateTODO() {Title = "tt1", PublicId = Guid.NewGuid(), UserId = userId});
                service.Create(new CreateTODO() {Title = "tt2", PublicId = Guid.NewGuid(), UserId = userId});
                service.Create(new CreateTODO() {Description = "dd3", PublicId = Guid.NewGuid(), UserId = userId});
                Assert.Equal(53, context.Todos.Count());

                var filter1 = service.List(new ListTODO()
                    {Page = 1, PageSize = 10, UserId = userId, Filter = new TODOFilter() {Text = "tt"}});
                var filter2 = service.List(new ListTODO()
                    {Page = 1, PageSize = 10, UserId = userId, Filter = new TODOFilter() {Text = "dd"}});


                Assert.Equal(2, filter1.Items.Count());
                Assert.Equal(1, filter2.Items.Count());

                Assert.Equal(2, filter1.TotalItems);
                Assert.Equal(1, filter2.TotalItems);
            }
        }


        [Fact]
        public void TodoService_Should_Sort()
        {
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingDTOProfile()); });
            var mapper = mockMapper.CreateMapper();

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoService(context, mapper);

                Assert.Equal(0, context.Todos.Count());
                for (var i = 0; i < 50; i++)
                {
                    var ret1 = service.Create(new CreateTODO()
                    {
                        PublicId = Guid.NewGuid(), UserId = userId,
                        Created = new DateTime(2000, 1, 1).AddDays(i)
                    });
                }

                Assert.Equal(50, context.Todos.Count());

                var list1 = service.List(new ListTODO()
                    {Page = 1, PageSize = 10, UserId = userId, OrderBy = nameof(TodoEntity.PublicId), Asc = true});
                var list2 = service.List(new ListTODO()
                    {Page = 1, PageSize = 10, UserId = userId, OrderBy = nameof(TodoEntity.PublicId), Asc = false});
                var list3 = service.List(new ListTODO()
                    {Page = 1, PageSize = 10, UserId = userId, OrderBy = nameof(TodoEntity.Created), Asc = true});
                var list4 = service.List(new ListTODO()
                    {Page = 1, PageSize = 10, UserId = userId, OrderBy = nameof(TodoEntity.Created), Asc = false});

                var first1 = list1.Items.First().PublicId;
                var first2 = list2.Items.First().PublicId;
                var first3 = list3.Items.First().PublicId;
                var first4 = list4.Items.First().PublicId;

                Assert.Equal(4, new[] {first1, first2, first3, first4}.Distinct().Count());
            }
        }
    }
}