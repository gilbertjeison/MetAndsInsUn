import { ActionTree } from 'vuex';
import { UsuariosState } from './types';
import { RootState } from '../types';
import { RespositoryFactory } from '@/Repository/RepositoryFactory';

const UsuariosRepository = RespositoryFactory.get('usuarios');

export const actions:ActionTree<UsuariosState,RootState> ={
    
  async getAllUsuariosAsync({commit,dispatch}):Promise<any>{
    var data = await UsuariosRepository.getAll();
    commit('setUsuarios',data.data);
    return data;        
  },

  
}