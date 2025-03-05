import { HttpParams } from '@angular/common/http';

export interface PaginatedRequest {
    page?: number;
    size?: number;
    order?: string;
}

export function addPaginatedParam(
    params: HttpParams,
    paginatedRequest?: PaginatedRequest
): HttpParams {
    if (!paginatedRequest) return params;

    if (paginatedRequest.page) params.set('page', paginatedRequest.page);
    if (paginatedRequest.size) params.set('size', paginatedRequest.size);
    if (paginatedRequest.order) params.set('order', paginatedRequest.order);

    return params;
}
