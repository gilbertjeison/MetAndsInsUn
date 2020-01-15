import Repository from './Repository';
import { Programaciones } from '@/models/Programaciones';

const resource = "/Programaciones";

export default{

    async get(data){
      
        return Repository.get<Programaciones[]>(`${resource}/`+data.area+'/'+data.year)
            .catch((e) => 
            {
                console.log(`Error mientras se cargaban programaciones: ${e.message}.`) ;
            });
    },

    async getDistinctYears(){
      
        return Repository.get<number[]>(`${resource}/distinct_years`)
            .catch((e) => 
            {
                console.log(`Error mientras se cargaban años de programaciones: ${e.message}.`) ;
            });
    },

    async getCalculatedWeeks(year){
      
        return Repository.get(`${resource}/calculate_weeks/`+year)
            .catch((e) => 
            {
                console.log(`Error mientras se cargaban las semanas calculadas: ${e.message}.`) ;
            });
    },

    async getDistinctAreas(){
      
        return Repository.get<number[]>(`${resource}/distinct_areas`)
            .catch((e) => 
            {
                console.log(`Error mientras se cargaban las areas: ${e.message}.`) ;
            });
    },

    async ExistsInYearWeek(data){
      
        return Repository.get(`${resource}/`+data.idEquipo.id+'/'+data.year+'/'+data.week)
            .catch((e) => 
            {
                console.log(`Error mientras se comprobaba existencia de programación: ${e.message}.`) ;
            });
    },

    async post(ps:any){
     
        var formData = new FormData();

        for ( var key in ps) {
            if (key == 'idEquipo' || key == 'idEstado') {
                formData.append(key, ps[key].id);
            }            
            else {
                formData.append(key, ps[key]);
            }            
        }       
        return await Repository.post(`${resource}/add`,
        formData,
        ).catch((e) => 
        {
            console.log(`Error mientras se guardaba programación: ${e.message}.`) ;
        });
    },

    async postEdit(ps:any){
     
        var formData = new FormData();

        for ( var key in ps) {
            if (key == 'idEquipo' || key == 'idEstado') {
                formData.append(key, ps[key].id);
            }            
            else {
                formData.append(key, ps[key]);
            }            
        }       
        return await Repository.post(`${resource}/edit`,
        formData,
        ).catch((e) => 
        {
            console.log(`Error mientras se editaba programación: ${e.message}.`) ;
        });
    },
    
}