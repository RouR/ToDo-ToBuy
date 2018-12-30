/* tslint:disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v12.0.3.0 (NJsonSchema v9.12.5.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming



export class DeleteTODORequest implements IDeleteTODORequest {
    publicId!: string;

    constructor(data?: IDeleteTODORequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.publicId = data["PublicId"];
        }
    }

    static fromJS(data: any): DeleteTODORequest {
        data = typeof data === 'object' ? data : {};
        let result = new DeleteTODORequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PublicId"] = this.publicId;
        return data; 
    }
}

export interface IDeleteTODORequest {
    publicId: string;
}

export class DeleteTODOResponse implements IDeleteTODOResponse {
    hasError!: boolean;
    message?: string | undefined;
    data!: boolean;

    constructor(data?: IDeleteTODOResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = data["HasError"];
            this.message = data["Message"];
            this.data = data["Data"];
        }
    }

    static fromJS(data: any): DeleteTODOResponse {
        data = typeof data === 'object' ? data : {};
        let result = new DeleteTODOResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["HasError"] = this.hasError;
        data["Message"] = this.message;
        data["Data"] = this.data;
        return data; 
    }
}

export interface IDeleteTODOResponse {
    hasError: boolean;
    message?: string | undefined;
    data: boolean;
}

export class EditTODORequest implements IEditTODORequest {
    publicId?: string | undefined;

    constructor(data?: IEditTODORequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.publicId = data["PublicId"];
        }
    }

    static fromJS(data: any): EditTODORequest {
        data = typeof data === 'object' ? data : {};
        let result = new EditTODORequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PublicId"] = this.publicId;
        return data; 
    }
}

export interface IEditTODORequest {
    publicId?: string | undefined;
}

export abstract class DBEntity implements IDBEntity {
    publicId!: string;
    created!: Date;
    updated!: Date;
    isDeleted!: boolean;

    constructor(data?: IDBEntity) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.publicId = data["PublicId"];
            this.created = data["Created"] ? new Date(data["Created"].toString()) : <any>undefined;
            this.updated = data["Updated"] ? new Date(data["Updated"].toString()) : <any>undefined;
            this.isDeleted = data["IsDeleted"];
        }
    }

    static fromJS(data: any): DBEntity {
        data = typeof data === 'object' ? data : {};
        throw new Error("The abstract class 'DBEntity' cannot be instantiated.");
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PublicId"] = this.publicId;
        data["Created"] = this.created ? this.created.toISOString() : <any>undefined;
        data["Updated"] = this.updated ? this.updated.toISOString() : <any>undefined;
        data["IsDeleted"] = this.isDeleted;
        return data; 
    }
}

export interface IDBEntity {
    publicId: string;
    created: Date;
    updated: Date;
    isDeleted: boolean;
}

export class TodoEntity extends DBEntity implements ITodoEntity {
    title!: string;
    description?: string | undefined;
    userId!: string;

    constructor(data?: ITodoEntity) {
        super(data);
    }

    init(data?: any) {
        super.init(data);
        if (data) {
            this.title = data["Title"];
            this.description = data["Description"];
            this.userId = data["UserId"];
        }
    }

    static fromJS(data: any): TodoEntity {
        data = typeof data === 'object' ? data : {};
        let result = new TodoEntity();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Title"] = this.title;
        data["Description"] = this.description;
        data["UserId"] = this.userId;
        super.toJSON(data);
        return data; 
    }
}

export interface ITodoEntity extends IDBEntity {
    title: string;
    description?: string | undefined;
    userId: string;
}

export class TodoPublicEntity extends TodoEntity implements ITodoPublicEntity {

    constructor(data?: ITodoPublicEntity) {
        super(data);
    }

    init(data?: any) {
        super.init(data);
        if (data) {
        }
    }

    static fromJS(data: any): TodoPublicEntity {
        data = typeof data === 'object' ? data : {};
        let result = new TodoPublicEntity();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        super.toJSON(data);
        return data; 
    }
}

export interface ITodoPublicEntity extends ITodoEntity {
}

export class EditTODOResponse extends TodoPublicEntity implements IEditTODOResponse {

    constructor(data?: IEditTODOResponse) {
        super(data);
    }

    init(data?: any) {
        super.init(data);
        if (data) {
        }
    }

    static fromJS(data: any): EditTODOResponse {
        data = typeof data === 'object' ? data : {};
        let result = new EditTODOResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        super.toJSON(data);
        return data; 
    }
}

export interface IEditTODOResponse extends ITodoPublicEntity {
}

export class ListTODORequest implements IListTODORequest {
    page!: number;
    pageSize!: number;
    orderBy?: string | undefined;
    asc!: boolean;
    filter?: TODOFilter | undefined;

    constructor(data?: IListTODORequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.page = data["Page"];
            this.pageSize = data["PageSize"];
            this.orderBy = data["OrderBy"];
            this.asc = data["Asc"];
            this.filter = data["Filter"] ? TODOFilter.fromJS(data["Filter"]) : <any>undefined;
        }
    }

    static fromJS(data: any): ListTODORequest {
        data = typeof data === 'object' ? data : {};
        let result = new ListTODORequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Page"] = this.page;
        data["PageSize"] = this.pageSize;
        data["OrderBy"] = this.orderBy;
        data["Asc"] = this.asc;
        data["Filter"] = this.filter ? this.filter.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IListTODORequest {
    page: number;
    pageSize: number;
    orderBy?: string | undefined;
    asc: boolean;
    filter?: TODOFilter | undefined;
}

export class TODOFilter implements ITODOFilter {
    text?: string | undefined;

    constructor(data?: ITODOFilter) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.text = data["Text"];
        }
    }

    static fromJS(data: any): TODOFilter {
        data = typeof data === 'object' ? data : {};
        let result = new TODOFilter();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Text"] = this.text;
        return data; 
    }
}

export interface ITODOFilter {
    text?: string | undefined;
}

export class PaginationOfTodoPublicEntity implements IPaginationOfTodoPublicEntity {
    page!: number;
    pageSize!: number;
    totalItems!: number;
    items?: TodoPublicEntity[] | undefined;
    totalPages!: number;
    firstItem!: number;
    lastItem!: number;
    hasPreviousPage!: boolean;
    hasNextPage!: boolean;

    constructor(data?: IPaginationOfTodoPublicEntity) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.page = data["Page"];
            this.pageSize = data["PageSize"];
            this.totalItems = data["TotalItems"];
            if (data["Items"] && data["Items"].constructor === Array) {
                this.items = [];
                for (let item of data["Items"])
                    this.items.push(TodoPublicEntity.fromJS(item));
            }
            this.totalPages = data["TotalPages"];
            this.firstItem = data["FirstItem"];
            this.lastItem = data["LastItem"];
            this.hasPreviousPage = data["HasPreviousPage"];
            this.hasNextPage = data["HasNextPage"];
        }
    }

    static fromJS(data: any): PaginationOfTodoPublicEntity {
        data = typeof data === 'object' ? data : {};
        let result = new PaginationOfTodoPublicEntity();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Page"] = this.page;
        data["PageSize"] = this.pageSize;
        data["TotalItems"] = this.totalItems;
        if (this.items && this.items.constructor === Array) {
            data["Items"] = [];
            for (let item of this.items)
                data["Items"].push(item.toJSON());
        }
        data["TotalPages"] = this.totalPages;
        data["FirstItem"] = this.firstItem;
        data["LastItem"] = this.lastItem;
        data["HasPreviousPage"] = this.hasPreviousPage;
        data["HasNextPage"] = this.hasNextPage;
        return data; 
    }
}

export interface IPaginationOfTodoPublicEntity {
    page: number;
    pageSize: number;
    totalItems: number;
    items?: TodoPublicEntity[] | undefined;
    totalPages: number;
    firstItem: number;
    lastItem: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

export class ListTODOResponse extends PaginationOfTodoPublicEntity implements IListTODOResponse {

    constructor(data?: IListTODOResponse) {
        super(data);
    }

    init(data?: any) {
        super.init(data);
        if (data) {
        }
    }

    static fromJS(data: any): ListTODOResponse {
        data = typeof data === 'object' ? data : {};
        let result = new ListTODOResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        super.toJSON(data);
        return data; 
    }
}

export interface IListTODOResponse extends IPaginationOfTodoPublicEntity {
}

export class SaveTODORequest implements ISaveTODORequest {
    publicId?: string | undefined;
    title?: string | undefined;
    description?: string | undefined;

    constructor(data?: ISaveTODORequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.publicId = data["PublicId"];
            this.title = data["Title"];
            this.description = data["Description"];
        }
    }

    static fromJS(data: any): SaveTODORequest {
        data = typeof data === 'object' ? data : {};
        let result = new SaveTODORequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PublicId"] = this.publicId;
        data["Title"] = this.title;
        data["Description"] = this.description;
        return data; 
    }
}

export interface ISaveTODORequest {
    publicId?: string | undefined;
    title?: string | undefined;
    description?: string | undefined;
}

export class SaveTODOResponse implements ISaveTODOResponse {
    hasError!: boolean;
    message?: string | undefined;
    data?: TodoPublicEntity | undefined;

    constructor(data?: ISaveTODOResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = data["HasError"];
            this.message = data["Message"];
            this.data = data["Data"] ? TodoPublicEntity.fromJS(data["Data"]) : <any>undefined;
        }
    }

    static fromJS(data: any): SaveTODOResponse {
        data = typeof data === 'object' ? data : {};
        let result = new SaveTODOResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["HasError"] = this.hasError;
        data["Message"] = this.message;
        data["Data"] = this.data ? this.data.toJSON() : <any>undefined;
        return data; 
    }
}

export interface ISaveTODOResponse {
    hasError: boolean;
    message?: string | undefined;
    data?: TodoPublicEntity | undefined;
}

export class DeleteTOBUYRequest implements IDeleteTOBUYRequest {
    publicId!: string;

    constructor(data?: IDeleteTOBUYRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.publicId = data["PublicId"];
        }
    }

    static fromJS(data: any): DeleteTOBUYRequest {
        data = typeof data === 'object' ? data : {};
        let result = new DeleteTOBUYRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PublicId"] = this.publicId;
        return data; 
    }
}

export interface IDeleteTOBUYRequest {
    publicId: string;
}

export class DeleteTOBUYResponse implements IDeleteTOBUYResponse {
    hasError!: boolean;
    message?: string | undefined;
    data!: boolean;

    constructor(data?: IDeleteTOBUYResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = data["HasError"];
            this.message = data["Message"];
            this.data = data["Data"];
        }
    }

    static fromJS(data: any): DeleteTOBUYResponse {
        data = typeof data === 'object' ? data : {};
        let result = new DeleteTOBUYResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["HasError"] = this.hasError;
        data["Message"] = this.message;
        data["Data"] = this.data;
        return data; 
    }
}

export interface IDeleteTOBUYResponse {
    hasError: boolean;
    message?: string | undefined;
    data: boolean;
}

export class EditTOBUYRequest implements IEditTOBUYRequest {
    publicId?: string | undefined;

    constructor(data?: IEditTOBUYRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.publicId = data["PublicId"];
        }
    }

    static fromJS(data: any): EditTOBUYRequest {
        data = typeof data === 'object' ? data : {};
        let result = new EditTOBUYRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PublicId"] = this.publicId;
        return data; 
    }
}

export interface IEditTOBUYRequest {
    publicId?: string | undefined;
}

export class EditTOBUYResponse implements IEditTOBUYResponse {
    hasError!: boolean;
    message?: string | undefined;
    data?: TOBUYPublicEntity | undefined;

    constructor(data?: IEditTOBUYResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = data["HasError"];
            this.message = data["Message"];
            this.data = data["Data"] ? TOBUYPublicEntity.fromJS(data["Data"]) : <any>undefined;
        }
    }

    static fromJS(data: any): EditTOBUYResponse {
        data = typeof data === 'object' ? data : {};
        let result = new EditTOBUYResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["HasError"] = this.hasError;
        data["Message"] = this.message;
        data["Data"] = this.data ? this.data.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IEditTOBUYResponse {
    hasError: boolean;
    message?: string | undefined;
    data?: TOBUYPublicEntity | undefined;
}

export class TobuyEntity extends DBEntity implements ITobuyEntity {
    name!: string;
    qty!: number;
    price?: Price | undefined;
    dueToUtc?: Date | undefined;
    userId!: string;

    constructor(data?: ITobuyEntity) {
        super(data);
    }

    init(data?: any) {
        super.init(data);
        if (data) {
            this.name = data["Name"];
            this.qty = data["Qty"];
            this.price = data["Price"] ? Price.fromJS(data["Price"]) : <any>undefined;
            this.dueToUtc = data["DueToUtc"] ? new Date(data["DueToUtc"].toString()) : <any>undefined;
            this.userId = data["UserId"];
        }
    }

    static fromJS(data: any): TobuyEntity {
        data = typeof data === 'object' ? data : {};
        let result = new TobuyEntity();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Name"] = this.name;
        data["Qty"] = this.qty;
        data["Price"] = this.price ? this.price.toJSON() : <any>undefined;
        data["DueToUtc"] = this.dueToUtc ? this.dueToUtc.toISOString() : <any>undefined;
        data["UserId"] = this.userId;
        super.toJSON(data);
        return data; 
    }
}

export interface ITobuyEntity extends IDBEntity {
    name: string;
    qty: number;
    price?: Price | undefined;
    dueToUtc?: Date | undefined;
    userId: string;
}

export class TOBUYPublicEntity extends TobuyEntity implements ITOBUYPublicEntity {

    constructor(data?: ITOBUYPublicEntity) {
        super(data);
    }

    init(data?: any) {
        super.init(data);
        if (data) {
        }
    }

    static fromJS(data: any): TOBUYPublicEntity {
        data = typeof data === 'object' ? data : {};
        let result = new TOBUYPublicEntity();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        super.toJSON(data);
        return data; 
    }
}

export interface ITOBUYPublicEntity extends ITobuyEntity {
}

export class Price implements IPrice {
    amount!: number;
    currency!: Currency;

    constructor(data?: IPrice) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.amount = data["Amount"];
            this.currency = data["Currency"];
        }
    }

    static fromJS(data: any): Price {
        data = typeof data === 'object' ? data : {};
        let result = new Price();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Amount"] = this.amount;
        data["Currency"] = this.currency;
        return data; 
    }
}

export interface IPrice {
    amount: number;
    currency: Currency;
}

export enum Currency {
    Unknown = 0, 
    Rub = 1, 
    Usd = 2, 
    Euro = 3, 
}

export class ListTOBUYRequest implements IListTOBUYRequest {
    page!: number;
    pageSize!: number;
    orderBy?: string | undefined;
    asc!: boolean;

    constructor(data?: IListTOBUYRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.page = data["Page"];
            this.pageSize = data["PageSize"];
            this.orderBy = data["OrderBy"];
            this.asc = data["Asc"];
        }
    }

    static fromJS(data: any): ListTOBUYRequest {
        data = typeof data === 'object' ? data : {};
        let result = new ListTOBUYRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Page"] = this.page;
        data["PageSize"] = this.pageSize;
        data["OrderBy"] = this.orderBy;
        data["Asc"] = this.asc;
        return data; 
    }
}

export interface IListTOBUYRequest {
    page: number;
    pageSize: number;
    orderBy?: string | undefined;
    asc: boolean;
}

export class PaginationOfTOBUYPublicEntity implements IPaginationOfTOBUYPublicEntity {
    page!: number;
    pageSize!: number;
    totalItems!: number;
    items?: TOBUYPublicEntity[] | undefined;
    totalPages!: number;
    firstItem!: number;
    lastItem!: number;
    hasPreviousPage!: boolean;
    hasNextPage!: boolean;

    constructor(data?: IPaginationOfTOBUYPublicEntity) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.page = data["Page"];
            this.pageSize = data["PageSize"];
            this.totalItems = data["TotalItems"];
            if (data["Items"] && data["Items"].constructor === Array) {
                this.items = [];
                for (let item of data["Items"])
                    this.items.push(TOBUYPublicEntity.fromJS(item));
            }
            this.totalPages = data["TotalPages"];
            this.firstItem = data["FirstItem"];
            this.lastItem = data["LastItem"];
            this.hasPreviousPage = data["HasPreviousPage"];
            this.hasNextPage = data["HasNextPage"];
        }
    }

    static fromJS(data: any): PaginationOfTOBUYPublicEntity {
        data = typeof data === 'object' ? data : {};
        let result = new PaginationOfTOBUYPublicEntity();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Page"] = this.page;
        data["PageSize"] = this.pageSize;
        data["TotalItems"] = this.totalItems;
        if (this.items && this.items.constructor === Array) {
            data["Items"] = [];
            for (let item of this.items)
                data["Items"].push(item.toJSON());
        }
        data["TotalPages"] = this.totalPages;
        data["FirstItem"] = this.firstItem;
        data["LastItem"] = this.lastItem;
        data["HasPreviousPage"] = this.hasPreviousPage;
        data["HasNextPage"] = this.hasNextPage;
        return data; 
    }
}

export interface IPaginationOfTOBUYPublicEntity {
    page: number;
    pageSize: number;
    totalItems: number;
    items?: TOBUYPublicEntity[] | undefined;
    totalPages: number;
    firstItem: number;
    lastItem: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

export class ListTOBUYResponse extends PaginationOfTOBUYPublicEntity implements IListTOBUYResponse {

    constructor(data?: IListTOBUYResponse) {
        super(data);
    }

    init(data?: any) {
        super.init(data);
        if (data) {
        }
    }

    static fromJS(data: any): ListTOBUYResponse {
        data = typeof data === 'object' ? data : {};
        let result = new ListTOBUYResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        super.toJSON(data);
        return data; 
    }
}

export interface IListTOBUYResponse extends IPaginationOfTOBUYPublicEntity {
}

export class SaveTOBUYRequest implements ISaveTOBUYRequest {
    publicId?: string | undefined;
    name?: string | undefined;
    qty!: number;
    price?: Price | undefined;
    dueToUtc?: Date | undefined;

    constructor(data?: ISaveTOBUYRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.publicId = data["PublicId"];
            this.name = data["Name"];
            this.qty = data["Qty"];
            this.price = data["Price"] ? Price.fromJS(data["Price"]) : <any>undefined;
            this.dueToUtc = data["DueToUtc"] ? new Date(data["DueToUtc"].toString()) : <any>undefined;
        }
    }

    static fromJS(data: any): SaveTOBUYRequest {
        data = typeof data === 'object' ? data : {};
        let result = new SaveTOBUYRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PublicId"] = this.publicId;
        data["Name"] = this.name;
        data["Qty"] = this.qty;
        data["Price"] = this.price ? this.price.toJSON() : <any>undefined;
        data["DueToUtc"] = this.dueToUtc ? this.dueToUtc.toISOString() : <any>undefined;
        return data; 
    }
}

export interface ISaveTOBUYRequest {
    publicId?: string | undefined;
    name?: string | undefined;
    qty: number;
    price?: Price | undefined;
    dueToUtc?: Date | undefined;
}

export class SaveTOBUYResponse implements ISaveTOBUYResponse {
    hasError!: boolean;
    message?: string | undefined;
    data?: TOBUYPublicEntity | undefined;

    constructor(data?: ISaveTOBUYResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = data["HasError"];
            this.message = data["Message"];
            this.data = data["Data"] ? TOBUYPublicEntity.fromJS(data["Data"]) : <any>undefined;
        }
    }

    static fromJS(data: any): SaveTOBUYResponse {
        data = typeof data === 'object' ? data : {};
        let result = new SaveTOBUYResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["HasError"] = this.hasError;
        data["Message"] = this.message;
        data["Data"] = this.data ? this.data.toJSON() : <any>undefined;
        return data; 
    }
}

export interface ISaveTOBUYResponse {
    hasError: boolean;
    message?: string | undefined;
    data?: TOBUYPublicEntity | undefined;
}

export class LoginRequest implements ILoginRequest {
    userName?: string | undefined;
    password?: string | undefined;

    constructor(data?: ILoginRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.userName = data["UserName"];
            this.password = data["Password"];
        }
    }

    static fromJS(data: any): LoginRequest {
        data = typeof data === 'object' ? data : {};
        let result = new LoginRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["UserName"] = this.userName;
        data["Password"] = this.password;
        return data; 
    }
}

export interface ILoginRequest {
    userName?: string | undefined;
    password?: string | undefined;
}

export class LoginResponse implements ILoginResponse {
    hasError!: boolean;
    message?: string | undefined;
    data?: string | undefined;

    constructor(data?: ILoginResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = data["HasError"];
            this.message = data["Message"];
            this.data = data["Data"];
        }
    }

    static fromJS(data: any): LoginResponse {
        data = typeof data === 'object' ? data : {};
        let result = new LoginResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["HasError"] = this.hasError;
        data["Message"] = this.message;
        data["Data"] = this.data;
        return data; 
    }
}

export interface ILoginResponse {
    hasError: boolean;
    message?: string | undefined;
    data?: string | undefined;
}

export class CreateUserRequest implements ICreateUserRequest {
    publicId!: string;
    name!: string;
    email!: string;
    password!: string;
    iP?: string | undefined;

    constructor(data?: ICreateUserRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.publicId = data["PublicId"];
            this.name = data["Name"];
            this.email = data["Email"];
            this.password = data["Password"];
            this.iP = data["IP"];
        }
    }

    static fromJS(data: any): CreateUserRequest {
        data = typeof data === 'object' ? data : {};
        let result = new CreateUserRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PublicId"] = this.publicId;
        data["Name"] = this.name;
        data["Email"] = this.email;
        data["Password"] = this.password;
        data["IP"] = this.iP;
        return data; 
    }
}

export interface ICreateUserRequest {
    publicId: string;
    name: string;
    email: string;
    password: string;
    iP?: string | undefined;
}

export class RegisterRequest extends CreateUserRequest implements IRegisterRequest {

    constructor(data?: IRegisterRequest) {
        super(data);
    }

    init(data?: any) {
        super.init(data);
        if (data) {
        }
    }

    static fromJS(data: any): RegisterRequest {
        data = typeof data === 'object' ? data : {};
        let result = new RegisterRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        super.toJSON(data);
        return data; 
    }
}

export interface IRegisterRequest extends ICreateUserRequest {
}

export class RegisterResponse implements IRegisterResponse {
    hasError!: boolean;
    message?: string | undefined;
    data!: string;
    validationErrors?: KeyValuePairOfStringAndString[] | undefined;

    constructor(data?: IRegisterResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.hasError = data["HasError"];
            this.message = data["Message"];
            this.data = data["Data"];
            if (data["ValidationErrors"] && data["ValidationErrors"].constructor === Array) {
                this.validationErrors = [];
                for (let item of data["ValidationErrors"])
                    this.validationErrors.push(KeyValuePairOfStringAndString.fromJS(item));
            }
        }
    }

    static fromJS(data: any): RegisterResponse {
        data = typeof data === 'object' ? data : {};
        let result = new RegisterResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["HasError"] = this.hasError;
        data["Message"] = this.message;
        data["Data"] = this.data;
        if (this.validationErrors && this.validationErrors.constructor === Array) {
            data["ValidationErrors"] = [];
            for (let item of this.validationErrors)
                data["ValidationErrors"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IRegisterResponse {
    hasError: boolean;
    message?: string | undefined;
    data: string;
    validationErrors?: KeyValuePairOfStringAndString[] | undefined;
}

export class KeyValuePairOfStringAndString implements IKeyValuePairOfStringAndString {
    key?: string | undefined;
    value?: string | undefined;

    constructor(data?: IKeyValuePairOfStringAndString) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.key = data["Key"];
            this.value = data["Value"];
        }
    }

    static fromJS(data: any): KeyValuePairOfStringAndString {
        data = typeof data === 'object' ? data : {};
        let result = new KeyValuePairOfStringAndString();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Key"] = this.key;
        data["Value"] = this.value;
        return data; 
    }
}

export interface IKeyValuePairOfStringAndString {
    key?: string | undefined;
    value?: string | undefined;
}