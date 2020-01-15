import Repository from './Repository';
import { TiposData } from '@/models/TiposData';
import { TipoInstruento } from '@/models/TipoInstrumento';
import { Tipos } from '@/models/Tipos';


const resource = "/TiposData";
const resource_ti = "/TipoInstrumentoes";


export default{
    async getAll(){  
        return Repository.get<TiposData[]>(`${resource}`)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban tipos data all: ${e.message}.`) ;
        });
    },

    async getAllByType(tipo){  
        return Repository.get<TiposData[]>(`${resource}/bytipo/`+tipo)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban tipos data all by type: ${e.message}.`) ;
        });
    },

    async getTipoInstrumento(){  
        return Repository.get<TipoInstruento[]>(`${resource_ti}`)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban tipos instrumento: ${e.message}.`) ;
        });
    },

    async getTipos(){  
        return Repository.get<Tipos[]>(`${resource}/tipos`)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban tipos: ${e.message}.`) ;
        });
    },

    async post(tds:any){
     
        var formData = new FormData();

        for ( var key in tds) {
            formData.append(key, tds[key]);
        }           
        
        return await Repository.post(`${resource}/add`,
        formData,
        ).catch((e) => 
        {
            console.log(`Error mientras se guardaba tipo data: ${e.message}.`) ;
        });
    },

    async post_ti(tds:any){
     
        var formData = new FormData();

        for ( var key in tds) {
            formData.append(key, tds[key]);
        }           
        
        return await Repository.post(`${resource}/add_ti`,
        formData,
        ).catch((e) => 
        {
            console.log(`Error mientras se guardaba tipo instrumento: ${e.message}.`) ;
        });
    },

    async postEdit(tds:any){
     
        var formData = new FormData();

        for ( var key in tds) {
            formData.append(key, tds[key]);
        }           
        
        return await Repository.post(`${resource}/edit`,
        formData,
        ).catch((e) => 
        {
            console.log(`Error mientras se editaba tipo data: ${e.message}.`) ;
        });
    },

    async postEdit_ti(tds:any){
     
        var formData = new FormData();

        for ( var key in tds) {
            formData.append(key, tds[key]);
        }           
        
        return await Repository.post(`${resource}/edit_ti`,
        formData,
        ).catch((e) => 
        {
            console.log(`Error mientras se editaba tipo instrumento: ${e.message}.`) ;
        });
    },

    async postDelete(tds:any){
     
        var formData = new FormData();
        for ( var key in tds) {
            formData.append(key, tds[key]);
        }           
        
        return await Repository.post(`${resource}/delete`,
        formData,
        ).catch((e) => 
        {
            console.log(`Error mientras se eliminaba tipo data: ${e.message}.`) ;
        });
    },

    async postDelete_ti(tds:any){
     
        var formData = new FormData();
        for ( var key in tds) {
            formData.append(key, tds[key]);
        }           
        
        return await Repository.post(`${resource}/delete_ti`,
        formData,
        ).catch((e) => 
        {
            console.log(`Error mientras se eliminaba tipo instrumento: ${e.message}.`) ;
        });
    },

    async getTIById(codigo){  
        return Repository.get<TiposData[]>(`${resource}/byidti/`+codigo)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban tipo de instrumento por código: ${e.message}.`) ;
        });
    },
}