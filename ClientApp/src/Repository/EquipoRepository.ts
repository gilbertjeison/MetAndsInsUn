import Repository from './Repository';
import { Equipos } from '@/models/Equipos';
import FormData from 'form-data'

const resource = "/Equipos";

export default{
    async get(){
        return Repository.get<Equipos[]>(`${resource}`)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban equipos: ${e.message}.`) ;
        });
    },

    async getEquiCombo(area){
        return Repository.get<Equipos[]>(`${resource}/equipos_combo/`+area)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban equipos combo: ${e.message}.`) ;
        });
    },

    async post(eqs:any,image:boolean){
     
        var formData = new FormData();
       
        if (image) {     

            formData.append("imagefile",eqs.file);

            for ( var key in eqs.data) {
                formData.append(key, eqs.data[key]);
            }
        }     
        else{
            for ( var key in eqs) {
                formData.append(key, eqs[key]);
            }
        }      
        
        return await Repository.post(`${resource}/add`,
            formData,
            { 
                headers:{'Content-Type': 'multipart/form-data'}
            }).catch((e) => 
            {
                console.log(`Error mientras se guardaba equipos: ${e.message}.`) ;
            });
    },

    async postEdit(eqs:any,image:boolean){
     
        var formData = new FormData();
       console.log('eqs :', eqs);
        if (image) {     

            formData.append("imagefile",eqs.file);

            for ( var key in eqs.data) {
                formData.append(key, eqs.data[key]);
            }
        }     
        else{
            for ( var key in eqs) {
                formData.append(key, eqs[key]);
            }
        }      
        
        return await Repository.post(`${resource}/edit`,
            formData,
            { 
                headers:{'Content-Type': 'multipart/form-data'}
            }).catch((e) => 
            {
                console.log(`Error mientras se editaba equipo: ${e.message}.`) ;
            });
    },

    async postCal(cals:any){
     
        var formData = new FormData();
       
        formData.append("file",cals.file);

        for ( var key in cals.data) {
            formData.append(key, cals.data[key]);
        }                    
        
        return await Repository.post(`${resource}/addCal`,
            formData,
            { 
                headers:{'Content-Type': 'multipart/form-data'}
            }).catch((e) => 
            {
                console.log(`Error mientras se guardaba calibración: ${e.message}.`) ;
            });
    },

    async getCal(idEquipo){  
        return await Repository.get(`${resource}/cals/`+idEquipo)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban calibraciones: ${e.message}.`) ;
        });
    },

    async postDelete(id){    
        
        return await Repository.post(`${resource}/calsdel/`+id).catch((e) => 
        {
            console.log(`Error mientras se eliminaba calibración: ${e.message}.`) ;
        });
    },
}