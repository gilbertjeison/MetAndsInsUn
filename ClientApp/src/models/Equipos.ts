export class Equipos{
    constructor(
        public id:number,
        public código:string,
        public nombre:string,
        // public Id:number,
        public Rango:string,
        public MedidaRango:number,
        public MaxRango:string,
        public DivisiónEscala:string,
        public MarcaId:number,
        public marcaDesc:string,
        public Tipo:string,
        public serie:string,
        public FechaIngreso:Date,
        public estatusId:number,
        public estatusDesc:string,
        public BuscarPor:string,
        public tipoInstrumentoId:number,
        public tipoInstrumentoDesc:string,
        public AreaId:number,
        public areaDesc:string,
        public fechaAdd:string,
        public imagePath:string,
    ){

    }
}