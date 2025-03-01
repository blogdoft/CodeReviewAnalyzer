import { LazySearchResponse } from './lazy-search-response';
import { Team } from './team';

export interface TeamsPaginated extends LazySearchResponse<Team> {}
