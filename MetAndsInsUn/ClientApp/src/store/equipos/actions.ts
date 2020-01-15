import { ActionTree } from 'vuex';
import { EquiposState } from './types';
import { RootState } from '../types';
import { RespositoryFactory } from '@/Repository/RepositoryFactory';

const EquipoRepository = RespositoryFactory.get('equipos');


export const actions:ActionTree<EquiposState,RootState> ={
    async getEquiposAsync({commit,dispatch}):Promise<any>{
      
      var res = await EquipoRepository.get();      
      commit('setEquipos',res.data);
      return res; 
    },

    async getEquiposComboAsync({commit,dispatch},area):Promise<any>{
      
      var res = await EquipoRepository.getEquiCombo(area); 
      return res; 
    },

    setEquipos({commit,dispatch},data):any{
      commit('setEquipos',data);
    },

    async addEquipoAsync({commit,dispatch},data):Promise<any>{
      
      var res = await EquipoRepository.post(data.data,data.image);
      
      return res;  
    },
    
    async editEquipoAsync({commit,dispatch},data):Promise<any>{
      
      var res = await EquipoRepository.postEdit(data.data,data.image);
      
      return res;  
    },
    async addCalibracionAsync({commit,dispatch},data):Promise<any>{
      
      var res = await EquipoRepository.postCal(data);
      
      return res;  
    },

    async getCalibracionesAsync({commit,dispatch},idEquipo):Promise<any>{
      
      var res = await EquipoRepository.getCal(idEquipo); 
      return res; 
    },

    async deleteCalibracionAsync({commit,dispatch},id):Promise<any>{
      
      var res = await EquipoRepository.postDelete(id); 
      return res; 
    },
}