import { MutationTree } from 'vuex';
import { TiposDataState } from './types';

export const mutations:MutationTree<TiposDataState> ={
    setTiposData(state,tipos){
        state.tipos_data = tipos;
    },
    setTipoInstrumento(state,tipo_ins){
        state.tipos_instru = tipo_ins;
    },
    setTipos(state,tipo){
        state.tipos = tipo;
    },
}