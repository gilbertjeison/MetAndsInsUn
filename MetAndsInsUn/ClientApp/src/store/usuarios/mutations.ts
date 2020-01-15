import { MutationTree } from 'vuex';
import { UsuariosState } from './types';

export const mutations:MutationTree<UsuariosState> ={
    setUsuarios(state,us){
        state.usuarios = us;
    },
}