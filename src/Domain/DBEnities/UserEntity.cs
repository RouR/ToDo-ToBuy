using System;
using Domain.Interfaces;

namespace Domain.DBEnities
{
    public class UserEntity: DBEntity, IUser
         {
             public Guid UserId { get; set; }
         }
}