import EquipoRepository from  './EquipoRepository';
import TiposDataRepository from './TiposDataRepository';
import ProgramacionesRepository from './ProgramacionesRepository';
import UsuariosRepository from './UsuariosRepository';

const respositories ={
    equipos: EquipoRepository,
    tipos_data: TiposDataRepository,
    programaciones: ProgramacionesRepository,
    usuarios:UsuariosRepository,
};

export const RespositoryFactory ={
    get: name => respositories[name]
}