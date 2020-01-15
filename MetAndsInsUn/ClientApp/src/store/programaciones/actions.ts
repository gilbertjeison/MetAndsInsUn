import { RespositoryFactory } from "@/Repository/RepositoryFactory";
import { ActionTree } from 'vuex';
import { ProgramacionesState } from './types';
import { RootState } from '../types';

const ProgramacionesRep = RespositoryFactory.get('programaciones');

export const actions:ActionTree<ProgramacionesState,RootState>={
  async getProgsAsync({commit,dispatch},data):Promise<any>{     
      var res = await ProgramacionesRep.get(data);      
      return res; 
    },

  async getYearsAsync({commit,dispatch}):Promise<any>{      
      var res = await ProgramacionesRep.getDistinctYears();      
      return res; 
    },
  async getDistinctAreas({commit,dispatch}):Promise<any>{      
    var res = await ProgramacionesRep.getDistinctAreas();      
    return res; 
  },
  async getCalculatedWeeksAsync({commit,dispatch},year):Promise<any>{      
    var res = await ProgramacionesRep.getCalculatedWeeks(year);      
    return res; 
  },
  async addProgramacionAsync({commit,dispatch},data):Promise<any>{    
    var res = await ProgramacionesRep.post(data);    
    return res;  
  },
  async existsInYearWeekAsync({commit,dispatch},data):Promise<any>{     
    var res = await ProgramacionesRep.ExistsInYearWeek(data);      
    return res; 
  },
}