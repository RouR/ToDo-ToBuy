using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DTO;
using Newtonsoft.Json;

namespace Shared
{
	//autogenerated
	public sealed partial class AccountServiceClient
	{
	
	

		/// <summary>
		/// Register new User
		/// UserId must be pregenerated
		/// 
		/// </summary>
		/// <returns>Erroable Guid UserId</returns>
		
		public async Task<DTO.Internal.Account.CreateUserResponse> User_Register(DTO.Internal.Account.CreateUserRequest data)
		{
			var response = await _client.PostAsJsonAsync("/User/Register", data);
			response.EnsureSuccessStatusCode();
		
			var result = await response.Content.ReadAsAsync<DTO.Internal.Account.CreateUserResponse>();
			return result;
		}
		

	}
}