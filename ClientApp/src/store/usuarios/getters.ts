import { GetterTree } from 'vuex';
import { UsuariosState } from './types';
import { RootState } from '../types';

export const getters: GetterTree<UsuariosState,RootState> = {
    getUsuarios(state){
        return state.usuarios;
    },
   
}