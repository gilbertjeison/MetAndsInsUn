<template>    
  <!-- <v-container fluid> -->
     <v-card class="pa-3 ma-1">      
        <v-slide-y-transition mode="out-in">
          <v-layout column>     
            <v-container>
              <v-alert style="border-radius:10px;" 
                :value="true"
                color="info"
                icon="scheduler"
                outline> 
                 
                <v-layout row wrap justify-center>
                  <v-flex xs12 md10> <h2>PLAN DE ASEGURAMIENTO METROLOGICO</h2></v-flex>  
                </v-layout>
                <v-container grid-list-md >
                  <v-layout row wrap>                 
                    <v-flex xs12 sm6 md4 lg3>
                      <v-select
                        :items="this.areas"
                        item-text="descripcion"
                        item-value="id"
                        label="Área"          
                        v-model="area_selected"                      
                      ></v-select>
                    </v-flex>
                    <v-flex xs12 sm6 md4 lg3 >               
                      <v-select
                        :items="this.anos"
                        label="Año"          
                        v-model="ano_selected"                      
                      ></v-select>
                    </v-flex>
                    <v-flex xs12 sm6 md4 lg3>
                      <v-btn color="primary" small top @click="aplicarFiltro();">                
                            Aplicar filtro
                        <v-icon right dark>filter_list</v-icon>
                      </v-btn>
                    </v-flex>
                  </v-layout> 
                </v-container> 
                
                <!-- DIALOGO PARA ADICCIÓN Y EDICIÓN DE EQUIPOS -->
               <v-layout row wrap>
                  <v-flex xs3 text-xs-center>
                    <v-dialog v-model="dialogAdd" persistent max-width="800px">
                      <template v-slot:activator="{ on }">      
                        <v-btn color="primary" v-on="on" @click="addProgClick" >                
                          Programar equipo
                          <v-icon right dark>add_alert</v-icon>
                        </v-btn>
                      </template>
                      <v-card >
                        <v-card-title light style="background-color:#1e88e5;">
                          <span class="headline white--text">{{headDialogText}}</span>
                        </v-card-title>
                        <v-card-text>
                           <v-container grid-list-md>
                            <v-layout row>                           
                              <v-flex xs12 md6>                                
                                <v-select
                                  :disabled="this.cboDisabled"
                                  :items="this.all_areas"
                                  item-text="descripcion"
                                  item-value="id"
                                  label="Seleccione area"          
                                  v-model="newProg.idArea"   
                                  :rules="[this.rules.required]"
                                  v-on:change="changeArea"                      
                                ></v-select>                               
                              </v-flex>
                            </v-layout>
                            <v-layout row wrap>
                              <v-flex xs12 md8>
                                <v-combobox
                                  :disabled="this.cboDisabled"
                                  auto-select-first
                                  :items="this.equipos"
                                  item-text="codNombre"
                                  item-value="id"
                                  label="Seleccione equipo"          
                                  v-model="newProg.idEquipo"   
                                  :rules="[this.rules.required]"                      
                                ></v-combobox>    
                              </v-flex>
                              <v-flex xs12 md4>
                               <v-select
                                  :items="this.base_anos"
                                  label="Año"          
                                  v-model="newProg.ano"   
                                  v-on:change="changeAno"                    
                                ></v-select>                                 
                              </v-flex>
                            </v-layout>
                            <v-layout row>
                              <v-flex xs12>                                
                                <v-select
                                  :items="this.weeks"
                                  item-text="fecha"
                                  item-value="week"
                                  label="Seleccione semana"          
                                  v-model="newProg.semana"   
                                  :rules="[this.rules.required]"                  
                                ></v-select>                               
                              </v-flex>
                            </v-layout>
                            <v-layout row wrap>
                              <v-flex xs12 md6>  
                                <v-combobox
                                  v-model="newProg.idEstado"
                                  :items="this.states"
                                  item-text="descripcion"
                                  item-value="id"
                                  chips
                                  label="Estado">
                                  <template v-slot:selection="data">
                                    <v-chip
                                      :key="JSON.stringify(data.item)"
                                      :selected="data.selected"
                                      :disabled="data.disabled"
                                      class="v-chip--select-multi"
                                      @click.stop="data.parent.selectedIndex = data.index"
                                      @input="data.parent.selectItem(data.item)"
                                    >
                                      <v-avatar v-if="data.item.id == 478" class="black--text font-weight-bold">X</v-avatar>
                                      <v-avatar v-if="data.item.id == 480" class="green black--text font-weight-bold">X</v-avatar>
                                      <v-avatar v-if="data.item.id == 481" class="red black--text font-weight-bold">X</v-avatar>
                                      <v-avatar v-if="data.item.id == 479" class="yellow black--text font-weight-bold">X</v-avatar>
                                      {{ data.item.descripcion }}
                                    </v-chip>
                                  </template>
                                </v-combobox>                          
                              </v-flex>
                              <v-flex xs12 md6>
                               <v-select class="mt-3"
                                  :items="this.users"
                                  item-text="nombreCompleto"
                                  item-value="id"
                                  label="Usuario responsable"          
                                  v-model="newProg.idUsuario"                      
                                ></v-select>                                 
                              </v-flex>
                            </v-layout>  
                            <v-layout row>  
                              <v-flex xs12>
                                <v-textarea
                                  outline
                                  label="Observaciones..."
                                  v-model="newProg.observaciones"
                                ></v-textarea>
                              </v-flex>
                            </v-layout>                           
                          </v-container>
                        </v-card-text>
                        <v-card-actions>
                          <v-spacer></v-spacer>
                          <v-btn color="blue darken-1" flat @click="dialogAdd = false">Cancelar</v-btn>
                          <v-btn color="blue darken-1" flat @click="addProgramacion">Guardar programación</v-btn>
                        </v-card-actions>
                      </v-card>
                    </v-dialog>
                  </v-flex>
                </v-layout> 
              </v-alert>

              <!-- DIALOGO DE VALIDACIÓN -->
              <v-dialog
                v-model="this.dialogVal"
                max-width="300">
                <v-card>
                  <v-card-title class="headline">
                    {{this.headDgText}}
                  </v-card-title>
                  <v-divider></v-divider>
                  <v-card-text>
                    {{this.bodyDgText}}
                  </v-card-text>
                  <v-divider></v-divider>
                  <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                      class="text-md-center"
                      color="green darken-1"
                      flat="flat"
                      @click="dialogVal = false">
                      Aceptar
                  </v-btn>                 
                  </v-card-actions>
                </v-card>
              </v-dialog>
            </v-container> 

            <div class="mt-2">
              <pivot-table 
              :data="proItems" 
              :row-fields="rowFields" 
              :col-fields="colFields" 
              :reducer="reducer">
              <template v-slot:value="{ value }">
                <div v-if="value.idEstado == 478" @click="showDetails(value)" class="text-center"><v-chip small text-color="black" class="font-weight-bold">X</v-chip></div>
                <div v-if="value.idEstado == 480" @click="showDetails(value)" class="text-center"><v-chip small text-color="black" color="green" class="font-weight-bold">X</v-chip></div>
                <div v-if="value.idEstado == 481" @click="showDetails(value)" class="text-center"><v-chip small text-color="black" color="red" class="font-weight-bold">X</v-chip></div>
                <div v-if="value.idEstado == 479" @click="showDetails(value)" class="text-center"><v-chip small text-color="black" color="yellow" class="font-weight-bold">X</v-chip></div>
              </template>
              </pivot-table>
            </div>
            
          </v-layout>
        </v-slide-y-transition>
     </v-card>
  <!-- </v-container>      -->
</template>


<script  lang="ts">

import {Component,Vue } from 'vue-property-decorator';
import { State, Action, Getter,namespace,Mutation } from "vuex-class";
import PivotTable from '../plugins/Pivot/PivotTable.vue';
import {DatePicker} from 'element-ui';
import { Programaciones } from '../models/Programaciones';

const td_namespace = namespace('tipo_data');
const p_namespace = namespace('programaciones');
const e_namespace = namespace('equipo');
const u_namespace = namespace('usuarios');

@Component({
    components: {
    PivotTable
  }
})    
export default class SchedulerView extends Vue {

  @p_namespace.Action
  private getDistinctAreas: any;

  @p_namespace.Action
  private getProgsAsync: any;

  @p_namespace.Action
  private getYearsAsync: any;

  @p_namespace.Action
  private getCalculatedWeeksAsync: any;

  @p_namespace.Action
  private addProgramacionAsync: any;

  @p_namespace.Action
  private existsInYearWeekAsync: any;
  
  @td_namespace.Action
  private getAllTiposDataByTypeAsync:any;

  @e_namespace.Action
  private getEquiposComboAsync:any;

  @u_namespace.Action
  private getAllUsuariosAsync:any;

  private eq_selected = '';
  private area_selected = 0;
  private ano_selected = 0;
  private all_areas = [];
  private areas = [];
  private anos = [];
  private base_anos = [];
  private proItems = [];
  private equipos = [];
  private weeks = [];
  private states = [];
  private users = [];
  private dialogAdd = false;
  private headDialogText='Nueva programación';
  private bodyDgText = '';
  private headDgText = '';
  private dialogVal = false;
  private cboDisabled = false;
  private isAdding = true;
  private rules = {
      required: value => !!value || 'Campo requerido...'
    }

  private newProg:Programaciones = {
    id:0,
    codigoEquipo:'',
    idEquipo:0,
    descEquipo:'',
    ano:0,
    semana:0,
    fecha:new Date(),
    idEstado:0,
    descEstado:'',
    idUsuario:0,
    nombreUsuario:'',
    observaciones:'',
    frecuencia:0,
    idArea:0,
    mes:0
  } 

  fields= [];
  rowFields= [{
    getter: item => item.codigoEquipo,
    label: 'Country'
    }, {
    getter: item => item.descEquipo,
    label: 'Gender',
    //headerSlotName: 'genderHeader'
    },{
    getter: item => item.frecuencia + ' Meses',
    label: 'Frecuencia',
    //headerSlotName: 'genderHeader'
    }];
    colFields= [{
      getter: item => item.mes,
      label: 'Mes'
    },
    {
      getter: item => item.semana,
      label: 'Semana'
    }];
  reducer= (sum, item) => item;//sum + item.count;
  defaultShowSettings: true;
  isDataLoading: false
  

  private async beforeMount() {
    const self = this;

    //Load all data
    await this.loadAllData();

    //Cargar todas las areas
    await this.getAllTiposDataByTypeAsync(2).then((r) => {
      self.all_areas =  r.data; 

      if (self.all_areas.length > 0) {      
          //Fijar la primer area como la seleccionada
          self.newProg.idArea = self.all_areas[0].id;

          self.getEquiposComboAsync(self.newProg.idArea).then((r) => { 
            self.equipos = r.data;
            if (self.equipos.length > 0) {
              self.newProg.idEquipo =  self.equipos[0];
            }
          });     
      }
    }); 

    //calcular años
    this.calculateYears();

    //Cargar estados
    await this.getAllTiposDataByTypeAsync(8).then((r) => {
      self.states = r.data; 
      if (self.states.length > 0) {
        self.newProg.idEstado =  self.states[0];
      }
    });   
      
    //Cargar usuarios
    await this.getAllUsuariosAsync().then((r) => {
        self.users = r.data; 
      }); 
  }

  private async loadAllData(){
    const self = this;

    await Promise.all([this.getYearsAsync(),this.getDistinctAreas()])
      .then(t => {
      var params ={ anosP:t[0].data,areasP:t[1].data};
       
      self.anos = params.anosP;  
      self.areas = params.areasP;
              
      if (self.anos.length > 0 && self.ano_selected == 0) {      
        self.ano_selected = self.anos[0];
      }  

      if (self.areas.length > 0 && self.area_selected == 0) {      
          //Fijar la primer area como la seleccionada
          self.area_selected = self.areas[0].id;
      } 
      
      //Cargar programaciones con los parametros por defecto
      this.getProgsAsync({area:this.area_selected,year:this.ano_selected}).then((r) => {
        self.proItems = r.data; 
        //Obtener las diferentes areas de los equipos de las programaciones      
        // var fil_areas = Array.from(new Set(self.proItems.map(x=> x.idArea)));
        //Buscar areas implicadas en el listado de todas
        // self.areas = self.all_areas.filter(x => fil_areas.includes(x.id));
      });      
    });      
  }

  private async addProgClick(){
    this.headDialogText='Nueva programación';
    this.cboDisabled = false;
  } 


  private async addProgramacion(){
    const self = this;
    console.log('newProg :', this.newProg);
    //Verificar si ya existe una programación con los datos dados
    await this.existsInYearWeekAsync({
       idEquipo:this.newProg.idEquipo,
       year:this.newProg.ano,
       week:this.newProg.semana})
       .then((r) => {
        if (r.data.exists) {
          this.buildDialog('PROGRAMACIÓN','El equipo a programar ya fue programado en la fecha indicada');
        }
        else{
          this.dialogAdd = false;
          const self = this;         

          this.addProgramacionAsync(this.newProg).then((r) => {
            if (r.data.code == 1) {
                this.buildDialog('PROGRAMACIÓN','¡Dato agregado correctamente!');
                //Recargar equipos                      
                this.area_selected = this.newProg.idArea;
                this.ano_selected = this.newProg.ano;
                this.loadAllData();
            }
            else{
                this.buildDialog('PROGRAMACIÓN','¡Hubo un error al realizar acción, intente nuevamente!');
                console.log('response :', r);
            }                    
          });
        }
      });        
  }

  private async aplicarFiltro(){
    const self = this;
    //Cargar programaciones con los parametros por defecto
    await this.getProgsAsync({area:this.area_selected,year:this.ano_selected}).then((r) => {
      self.proItems = r.data; 
    });

    //Recargar equipos
    await self.getEquiposComboAsync(self.area_selected).then((r) => { 
      self.equipos = r.data;
        if (self.equipos.length > 0) {
          self.newProg.idEquipo =  self.equipos[0];
        }
      });  
    }

  private async changeArea(param){
    let self = this;  
    self.newProg.idEquipo = 0;
    //Dependiendo la opción seleccionada en el combo, mostrar la data       
    await this.getEquiposComboAsync(param).then((r) => { 
      self.equipos = r.data;
      if (self.equipos.length > 0) {
         self.newProg.idEquipo =  self.equipos[0];
      }
    });       
  }   

  private async changeAno(param){
    let self = this;  
    self.newProg.semana = 0;
    //Dependiendo la opción seleccionada en el combo, mostrar la data       
    await this.getCalculatedWeeksAsync(param).then((r) => { 
      self.weeks = r.data;
      if (self.weeks.length > 0) {
         self.newProg.semana =  self.weeks[0].week;
      }
    });       
  } 

  private async showDetails(item){
    const self = this;
    console.log('item :', item);
    this.newProg = Object.assign({}, item);
    this.newProg.idEquipo = this.equipos.filter(x=> x.id == this.newProg.idEquipo)[0];
    this.newProg.idEstado = this.states.filter(x=> x.id == this.newProg.idEstado)[0];
    this.cboDisabled = true;
    this.headDialogText = 'Detalles de programación';

    //Cargar semanas de año en detalle
     await this.getCalculatedWeeksAsync(this.newProg.ano).then((r) => { 
      self.weeks = r.data;
    });       
    
    this.dialogAdd = true;
  }

  private calculateYears()
  {
    var backoff = 2;
    var nextoff = 5;
    var currentYear = (new Date()).getFullYear();
    
    for (let index = currentYear - backoff; index <= currentYear + nextoff; index++) {
      this.base_anos.push(index);      
    }
  }

  private buildDialog(head,body){
    this.dialogVal = true;    
    this.bodyDgText = body;
    this.headDgText = head;
  }
}
</script>

</script>

<style lang="scss" scoped>
  

  .main_value {
    border: 1px solid black;
    border-radius: 25px;
    width: 25%;
    height: 25%;
  }

  
</style>