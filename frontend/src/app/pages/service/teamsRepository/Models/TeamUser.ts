import { User } from '../../users-repository/Models/User';

export interface TeamUser {
    user?: User;
    role?: string;
}
