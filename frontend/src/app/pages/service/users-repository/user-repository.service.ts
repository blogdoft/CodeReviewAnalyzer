import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
    addPaginatedParam,
    PaginatedRequest
} from '../../../shared/models/paginated-request';
import { UserPaginated } from './Models/user-paginated';

@Injectable({
    providedIn: 'root'
})
export class UserRepositoryService {
    private readonly baseUrl = 'http://localhost:8080/code-review';
    constructor(private http: HttpClient) {}

    getAll(
        userName?: string,
        active?: boolean,
        paginatedRequest?: PaginatedRequest
    ): Observable<UserPaginated> {
        let params = new HttpParams();

        params = addPaginatedParam(params, paginatedRequest);
        if (userName) params = params.set('userName', userName);
        if (active) params = params.set('status', active);
        return this.http.get<UserPaginated>(`${this.baseUrl}/api/users`, {
            params
        });
    }
}
