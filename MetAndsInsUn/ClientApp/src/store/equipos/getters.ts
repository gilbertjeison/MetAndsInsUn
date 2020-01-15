import { GetterTree } from 'vuex';
import { EquiposState } from './types';
import { RootState } from '../types';

export const getters: GetterTree<EquiposState,RootState> = {
    getEquipos(state){
        return state.equipos;
    },
    getFiltro(state){
        return state.filtro;
    }
}