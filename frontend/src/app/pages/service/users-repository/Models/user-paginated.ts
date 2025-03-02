import { LazySearchResponse } from '../../../../shared/models/lazy-search-response';
import { User } from './User';

export interface UserPaginated extends LazySearchResponse<User> {}
