import { EquiposState } from './types';
import {Module} from 'vuex';
import { RootState } from '../types';
import {getters} from './getters'
import {actions} from './actions'
import {mutations} from './mutations'

export const state: EquiposState={
    equipos: [],    
    filtro:{
        menu:false,
        marcas:[],
        marca_selected:'Todas...',
        areas:[],
        area_selected:'Todas...',
        tipos:[],
        tipo_selected:'Todos...',
        estatus:[],
        estatus_selected:[0,1]
    }
}

const namespaced:boolean = true;

export const equipo:Module<EquiposState,RootState> = {
    namespaced,
    state,
    getters,
    actions,
    mutations,
};
