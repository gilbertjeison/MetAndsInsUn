import { MutationTree } from 'vuex';
import { AuthState } from './types';

export const mutations: MutationTree<AuthState> = {
    setUser(state,user){
        state.user = user;
    },

    setAuthenticated(state,auth:boolean){
        state.isAuthenticated = auth;
    }
}