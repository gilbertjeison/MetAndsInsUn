import Repository from './Repository';
import { Usuarios } from '@/models/Usuarios';


const resource = "/Usuarios";

export default{
    async getAll(){  
        return Repository.get<Usuarios[]>(`${resource}`)
        .catch((e) => 
        {
            console.log(`Error mientras se cargaban usuarios: ${e.message}.`) ;
        });
    },
}