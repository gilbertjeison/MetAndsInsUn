export class Programaciones{
    constructor(
        public id:number,
        public codigoEquipo:string,
        public idEquipo:number,
        public descEquipo:string,
        public ano:number,
        public semana:number,
        public fecha:Date,
        public idEstado:number,
        public descEstado:string,        
        public idUsuario:number,
        public nombreUsuario:string,
        public observaciones:string,
        public frecuencia:number,
        public idArea:number,
        public mes:number
    ){

    }
}