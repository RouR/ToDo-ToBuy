
declare namespace Domain {
	interface DBEntity {
		Created: Date;
		PublicId: string;
		Updated: Date;
	}
}
declare namespace Domain.DBEnities {
	interface TodoEntity extends Domain.DBEntity {
		Description: string;
		Title: string;
		UserId: string;
	}
}
declare namespace DTO.Public.TODO {
	interface DeleteTODORequest {
		PublicId: string;
	}
	interface DeleteTODOResponse {
		Data: boolean;
		HasError: boolean;
		Message: string;
	}
	interface ListTODORequest {
		Page: number;
		PageSize: number;
	}
	interface ListTODOResponse extends Utils.Pagination.Pagination<Domain.DBEnities.TodoEntity> {
	}
	interface SaveTODORequest {
		Description: string;
		Title: string;
	}
	interface SaveTODOResponse {
		Data: Domain.DBEnities.TodoEntity;
		HasError: boolean;
		Message: string;
	}
}
declare namespace Utils.Pagination {
	interface Pagination<T> {
		FirstItem: number;
		HasNextPage: boolean;
		HasPreviousPage: boolean;
		Items: T[];
		LastItem: number;
		Page: number;
		PageSize: number;
		TotalItems: number;
		TotalPages: number;
	}
}
