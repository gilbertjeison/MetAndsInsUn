import { TiposDataState } from './types';
import {Module} from 'vuex';
import { RootState } from '../types';
import {getters} from './getters';
import {actions} from './actions';
import {mutations} from './mutations';

export const state: TiposDataState={
    tipos_data: [],
    tipos_instru: [],
    tipos:[],

}

const namespaced:boolean = true;

export const tipo_data:Module<TiposDataState,RootState> = {
    namespaced,
    state,
    getters,
    actions,
    mutations,
};
