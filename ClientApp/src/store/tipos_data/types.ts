import { TiposData } from '@/models/TiposData';
import { TipoInstruento } from '@/models/TipoInstrumento';
import { Tipos } from '@/models/Tipos';

export interface TiposDataState{
    tipos_data: TiposData[];
    tipos_instru: TipoInstruento[];
    tipos: Tipos[];
}