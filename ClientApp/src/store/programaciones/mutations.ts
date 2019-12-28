import { ProgramacionesState } from './types';
import { MutationTree } from 'vuex';

export const mutations:MutationTree<ProgramacionesState> ={
    setProgramaciones(state,pl){
        state.programacionesList = pl;
    },
}