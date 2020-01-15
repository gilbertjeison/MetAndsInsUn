import { AuthState } from './types';
import { Module } from 'vuex';
import { RootState } from '../types';
import { getters } from './getters'
import { actions } from './actions'
import { mutations } from './mutations'

export const state: AuthState ={
    isAuthenticated: false,
    user : null,
    security :
    {
        url : 'www.supapa.com'
    },
}

const namespaced:boolean = true;

export const auth:Module<AuthState,RootState> = {
    namespaced,
    state,
    getters,
    actions,
    mutations,
};
