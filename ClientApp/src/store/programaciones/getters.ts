import { ProgramacionesState } from './types';
import { GetterTree } from 'vuex';
import { RootState } from '../types';

export const getters: GetterTree<ProgramacionesState,RootState> = {
    getProgramaciones(state){
        return state.programacionesList;
    },   
}