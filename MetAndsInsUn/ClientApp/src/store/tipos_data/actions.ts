import { ActionTree } from 'vuex';
import { TiposDataState } from './types';
import { RootState } from '../types';
import { RespositoryFactory } from '@/Repository/RepositoryFactory';

const TipoDataRepository = RespositoryFactory.get('tipos_data');

export const actions:ActionTree<TiposDataState,RootState> ={
    
  async getAllTiposDataAsync({commit,dispatch}):Promise<any>{
    var data = await TipoDataRepository.getAll();
    commit('setTiposData',data.data);
    return data;        
  },

  async getAllTiposDataByTypeAsync({commit,dispatch},tipo):Promise<any>{
    var data = await TipoDataRepository.getAllByType(tipo);
    return data;        
  },
   
  async getAllTipoInstrumentoAsync({commit,dispatch}):Promise<any>{
    var data = await TipoDataRepository.getTipoInstrumento();
    commit('setTipoInstrumento',data.data);
    return data;
  },

  setFiltro({commit,dispatch},data):any{
    commit('equipo/setFiltroMenu',data,{root:true});
  },

  async getTiposAsync({commit,dispatch}):Promise<any>{
    var data = await TipoDataRepository.getTipos();
    commit('setTipos',data.data);
    return data;        
  },
    
  async addTipoDataAsync({commit,dispatch},data):Promise<any>{    
    var res = await TipoDataRepository.post(data);    
    return res;  
  },

  async addTipoInstrumentoAsync({commit,dispatch},data):Promise<any>{    
    var res = await TipoDataRepository.post_ti(data);    
    return res;  
  },

  async editTipoDataAsync({commit,dispatch},data):Promise<any>{    
    var res = await TipoDataRepository.postEdit(data);    
    return res;  
  },

  async editTipoInstrumentoAsync({commit,dispatch},data):Promise<any>{    
    var res = await TipoDataRepository.postEdit_ti(data);    
    return res;  
  },

  async deleteTipoDataAsync({commit,dispatch},data):Promise<any>{    
    var res = await TipoDataRepository.postDelete(data);    
    return res;  
  },

  async deleteTipoInstrumentoAsync({commit,dispatch},data):Promise<any>{    
    var res = await TipoDataRepository.postDelete_ti(data);    
    return res;  
  },

  async getTIByIdAsync({commit,dispatch},id):Promise<any>{
    var data = await TipoDataRepository.getTIById(id);
    return data;        
  },

  
}