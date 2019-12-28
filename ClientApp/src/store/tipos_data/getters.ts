import { GetterTree } from 'vuex';
import { TiposDataState } from './types';
import { RootState } from '../types';

export const getters: GetterTree<TiposDataState,RootState> = {
    getTiposData(state){
        return state.tipos_data;
    },
    getTipoIntrumento(state){
        return state.tipos_instru;
    },
    getTipos(state){
        return state.tipos;
    }
}