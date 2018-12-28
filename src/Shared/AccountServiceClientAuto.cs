using System.Net.Http;
using System.Threading.Tasks;

namespace Shared
{
	//autogenerated
	public sealed partial class AccountServiceClient
	{
	
	

		/// <summary>
		/// Register new User
		/// UserId must be pregenerated
		/// </summary>
		/// <returns>Erroable Guid UserId</returns>
		
		public async Task<DTO.Internal.Account.CreateUserResponse> User_Register(DTO.Internal.Account.CreateUserRequest data)
		{
			var response = await _client.PostAsJsonAsync("/User/Register", data);
			response.EnsureSuccessStatusCode();
		
			var result = await response.Content.ReadAsAsync<DTO.Internal.Account.CreateUserResponse>();
			return result;
		}
		

		/// <summary>
		/// Try Login, with ban check
		/// </summary>
		
		public async Task<DTO.Internal.Account.TryLoginResponse> User_TryLogin(DTO.Internal.Account.TryLoginRequest data)
		{
			var response = await _client.PostAsJsonAsync("/User/TryLogin", data);
			response.EnsureSuccessStatusCode();
		
			var result = await response.Content.ReadAsAsync<DTO.Internal.Account.TryLoginResponse>();
			return result;
		}
		

	}
}