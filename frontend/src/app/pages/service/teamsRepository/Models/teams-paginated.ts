import { LazySearchResponse } from '../../../../shared/models/lazy-search-response';
import { Team } from './team';

export interface TeamsPaginated extends LazySearchResponse<Team> {}
