import { ProgramacionesState } from './types';
import { Module } from 'vuex';
import { RootState } from '../types';
import {actions} from './actions';
import {getters} from './getters';
import {mutations} from './mutations';

export const state: ProgramacionesState = {
    programacionesList:[],
}

const namespaced:boolean = true;

export const programaciones:Module<ProgramacionesState,RootState> = {
    namespaced,
    state,
    getters,
    actions,
    mutations,
}