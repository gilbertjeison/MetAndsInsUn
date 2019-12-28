import { User } from 'oidc-client';

export interface AuthState{
    isAuthenticated: boolean;
    user: User;  
    security: {};
}