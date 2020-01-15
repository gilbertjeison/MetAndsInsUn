import { UsuariosState } from './types';
import { Module} from 'vuex';
import { RootState } from '../types';
import { getters} from './getters';
import { actions} from './actions';
import { mutations} from './mutations';

export const state: UsuariosState={
    usuarios: [],
}

const namespaced:boolean = true;

export const usuarios:Module<UsuariosState,RootState> = {
    namespaced,
    state,
    getters,
    actions,
    mutations,
};
