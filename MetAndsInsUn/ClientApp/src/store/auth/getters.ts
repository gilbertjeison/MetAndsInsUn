import { GetterTree } from 'vuex';
import { AuthState } from './types';
import { RootState } from '../types';

export const getters: GetterTree<AuthState,RootState> = {
    isAthenticated(state){
        return state.isAuthenticated;
    },
    getUser(state){
        return state.user;
    },
    getSecurity(state){
        return state.security;
    },
}