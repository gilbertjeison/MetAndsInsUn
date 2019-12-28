import { MutationTree } from 'vuex';
import { EquiposState } from './types';

export const mutations:MutationTree<EquiposState> ={
    setEquipos(state,equipos){
        state.equipos = equipos;
    },
    setFiltroMenu(state,tipos){
        state.filtro.estatus = tipos.d1.filter(e => e.idTipo == 7).map(e => e.descripcion);      
         
        state.filtro.areas = tipos.d1.filter(e => e.idTipo == 2).map(e => e.descripcion);     
        state.filtro.areas.unshift("Todas...");
        
        state.filtro.marcas = tipos.d1.filter(e => e.idTipo == 1).map(e => e.descripcion);
        state.filtro.marcas.unshift("Todas...");
        
        state.filtro.tipos = tipos.d2.map(e => e.descripcion);
        state.filtro.tipos.unshift("Todos...");
    },
    
}