export interface LazySearchResponse<T> {
    totalItems: number;
    totalPages: number;
    currentPage: number;
    data: T[];
}
