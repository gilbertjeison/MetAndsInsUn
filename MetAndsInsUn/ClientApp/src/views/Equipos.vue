<template>
  <v-container fluid>
      <v-card class="pa-3">      
        <v-slide-y-transition mode="out-in">
        <v-layout column>            
            <div>
              <v-alert style="border-radius:10px;" 
                :value="true"
                color="info"
                icon="settings_applications"
                outline
              >  
                <v-layout row wrap justify-end>
                  <v-flex xs10> <h1 class="text-md-center">Listado de equipos</h1></v-flex>
                  
                  <v-flex xs2 text-xs-right>
                    <v-menu
                      v-model="getFiltro.menu"
                      :close-on-content-click="false"
                      :nudge-width="300"
                      origin="center center"                    
                      offset-y>
                      <v-btn flat slot="activator" icon justify-end flexbox ><v-icon >filter_list</v-icon></v-btn>
                      <v-card class="filtri">
                      <v-card-title  class="subheading font-weight-bold">Filtro general</v-card-title>
                      <v-divider></v-divider>

                      <v-card-text>
                        <!-- Estatus -->
                        <v-list-tile-content>
                          <v-list-tile-title>Estatus</v-list-tile-title>
                          <v-list-tile-action>
                            <v-item-group multiple v-model="getFiltro.estatus_selected">
                              <v-item v-for="e in getFiltro.estatus" :key="e">
                                <v-chip 
                                  slot-scope="{active,toggle}"
                                  :selected="active"
                                  @click="toggle"
                                  :color="active ? 'primary' : ''"
                                  :text-color="active ? 'white' : ''">
                                  {{e}}
                                </v-chip>
                              </v-item>                            
                            </v-item-group>
                          </v-list-tile-action>
                        </v-list-tile-content>
                        <!-- Area -->
                        <v-list-tile-content>
                          <v-list-tile-title>Areas</v-list-tile-title>                        
                            <v-select
                              :items="this.getFiltro.areas"
                              v-model="getFiltro.area_selected"
                              solo                              
                            ></v-select>                
                        </v-list-tile-content>
                        <!-- Marcas -->
                        <v-list-tile-content>
                          <v-list-tile-title>Marcas</v-list-tile-title>                        
                            <v-select
                              :items="this.getFiltro.marcas"
                              v-model="getFiltro.marca_selected"
                              solo                              
                            ></v-select>                
                        </v-list-tile-content>

                        <!-- Tipo Instrumento -->
                        <v-list-tile-content>
                          <v-list-tile-title>Tipo instrumento</v-list-tile-title>                        
                            <v-select
                              :items="this.getFiltro.tipos"
                              v-model="getFiltro.tipo_selected"
                              solo                              
                            ></v-select>                
                        </v-list-tile-content>
                      </v-card-text>

                      <v-card-actions>
                        <v-spacer></v-spacer>

                        <v-btn color="primary" flat @click="aplicarFiltro(true);"
                          >Eliminar</v-btn
                        >
                        <v-btn color="primary" @click="aplicarFiltro(false);">Aplicar filtro</v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-menu>
                  </v-flex>
                </v-layout>
                <v-text-field 
                  class="ml-0 mt-1" 
                  flat 
                  prepend-icon="search" 
                  label="Buscar equipos..."                   
                  @keyup="updateData();"
                  v-model="search">                
                </v-text-field> 

                <!-- DIALOGO PARA ADICCIÓN Y EDICIÓN DE EQUIPOS -->
                <v-layout row wrap>
                  <v-flex xs3 text-xs-center>
                    <v-dialog v-model="dialogAdd" persistent max-width="800px">
                      <template v-slot:activator="{ on }">      
                        <v-btn color="primary" v-on="on" @click="addEquipoClick" >                
                          Nuevo equipo
                          <v-icon right dark>library_add</v-icon>
                        </v-btn>
                      </template>
                      <v-card >
                        <v-card-title light style="background-color:#1e88e5;">
                          <span class="headline white--text">{{headDialogText}}</span>
                        </v-card-title>
                        <v-divider></v-divider>
                        <v-card-text>
                          <v-container grid-list-md>
                            <v-layout wrap>
                              <v-flex xs12>
                                <el-upload
                                  action="/api/Equipos/upload"
                                  :http-request="addAttachment"
                                  :multiple="false" 
                                  :limit="1" 
                                  :auto-upload="false"
                                  list-type="picture-card"
                                  :on-preview="handlePictureCardPreview"
                                  :on-remove="handleRemove" 
                                  :on-change="onChangeFile"
                                  ref="upload"
                                  :data="newEquipo"
                                  :file-list="fileList"
                                  class="text-md-center">
                                  <i class="el-icon-plus"></i>
                                </el-upload>
                                <el-dialog :visible.sync="dialogVisible" :modalAppendToBody="false">
                                  <img width="100%" :src="dialogImageUrl" alt="">
                                </el-dialog>
                              </v-flex>
                              <v-flex xs12 sm6 md3 lg3>                                
                                <v-text-field outline label="Código*" :rules="[this.rules.required]"  v-model="newEquipo.código" hint="tener en cuenta la nomenclatura del código para cada equipo..." ></v-text-field>                               
                              </v-flex>
                              <v-flex xs12 sm6 md3 lg3>
                                <v-text-field outline label="Nombre" :rules="[this.rules.required]"  v-model="newEquipo.nombre" ></v-text-field>
                              </v-flex>
                              <v-flex xs12 sm6 md3 lg3>
                                <v-menu
                                  v-model="bFechIngreso"
                                  :close-on-content-click="false"
                                  :nudge-right="40"
                                  lazy
                                  transition="scale-transition"
                                  offset-y
                                  full-width
                                  min-width="290px"
                                >
                                  <template v-slot:activator="{ on }">
                                    <v-text-field
                                      v-model="newEquipo.fechaAdd"
                                      label="Fecha de ingreso"                                      
                                      outline
                                      readonly
                                      v-on="on"
                                    ></v-text-field>
                                  </template>
                                  <v-date-picker v-model="newEquipo.fechaAdd" @input="bFechIngreso = false"></v-date-picker>
                                </v-menu>
                              </v-flex>
                            
                              <v-flex xs12 sm6 md3 lg3>
                                <v-text-field outline label="Rango" v-model="newEquipo.Rango"></v-text-field>
                              </v-flex>

                              <v-flex xs12 sm6 md3 lg3>
                                <v-text-field outline label="Max Rango" v-model="newEquipo.MaxRango"></v-text-field>
                              </v-flex>

                              <v-flex xs12 sm6 md3 lg3>
                                <v-text-field outline label="División escala" v-model="newEquipo.DivisiónEscala"></v-text-field>
                              </v-flex>

                              <v-flex xs12 sm6 md3 lg3>
                                <v-text-field outline label="Tipo" v-model="newEquipo.Tipo"></v-text-field>
                              </v-flex>

                              <v-flex xs12 sm6 md3 lg3>
                                <v-text-field outline label="Serie" v-model="newEquipo.serie"></v-text-field>
                              </v-flex>
                              <v-flex xs12 sm6 md3 lg3>
                                <v-select
                                  :items="this.filterTipo(7)"
                                  item-text="descripcion"
                                  item-value="id"
                                  label="Estatus"          
                                  v-model="newEquipo.estatusId"   
                                  :rules="[this.rules.required]"                      
                                ></v-select>
                              </v-flex>
                              <v-flex xs12 sm6 md3 lg3>
                                <v-select
                                  :items="this.filterTipo(1)"
                                  item-text="descripcion"
                                  item-value="id"
                                  label="Marca"          
                                  v-model="newEquipo.MarcaId" 
                                  :rules="[this.rules.required]"                        
                                ></v-select>
                              </v-flex>

                              <v-flex xs12 sm6 md3 lg3>
                                <v-select
                                  :items="this.filterTipo(2)"
                                  item-text="descripcion"
                                  item-value="id"
                                  label="Área"          
                                  v-model="newEquipo.AreaId"   
                                  :rules="[this.rules.required]"                      
                                ></v-select>
                              </v-flex>

                              <v-flex xs12 sm6 md3 lg3>
                                <v-select
                                  :items="this.getTipoIntrumento"
                                  item-text="descripcion"
                                  item-value="id"
                                  label="Tipo de instrumento"          
                                  v-model="newEquipo.tipoInstrumentoId"   
                                  :rules="[this.rules.required]"                      
                                ></v-select>
                              </v-flex>
                            </v-layout>
                          </v-container>
                          <small>*Indica que el campo es obligatorio</small>
                        </v-card-text>
                        <v-card-actions>
                          <v-spacer></v-spacer>
                          <v-btn color="blue darken-1" flat @click="dialogAdd = false">Cancelar</v-btn>
                          <v-btn color="blue darken-1" flat @click="addEquipo">Guardar</v-btn>
                        </v-card-actions>
                      </v-card>
                    </v-dialog>
                  </v-flex>
                </v-layout>    
      

              </v-alert>
            </div>
            
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
            <!-- DIALOGO DE CARGA -->
            <v-dialog
              v-model="dialogLoad"
              hide-overlay
              persistent
              width="300">
              <v-card
                color="primary"
                dark>
                <v-card-text>
                  Cargando información...
                  <v-progress-linear
                    indeterminate
                    color="white"
                    class="mb-0"
                  ></v-progress-linear>
                </v-card-text>
              </v-card>
            </v-dialog>

            <!-- HOJA DE VIDA DE CADA EQUIPO -->
            <v-dialog v-model="dlgHv" fullscreen hide-overlay transition="dialog-bottom-transition">  
              <v-card>
                <v-toolbar dark color="primary">
                  <v-btn icon dark @click="dlgHv = false">
                    <v-icon>close</v-icon>
                  </v-btn>
                  <v-toolbar-title>Hoja de vida - {{this.newEquipo.nombre}} ({{this.newEquipo.código}})</v-toolbar-title>
                  <v-spacer></v-spacer>                  
                </v-toolbar>             
                <!-- <v-subheader>Información del equipo</v-subheader>
                 <v-card class="ml-3 mr-3 mb-2" hover>             
                   <v-card-text>
                    <v-container fluid grid-list-md> 
                      <v-layout row wrap>
                        <v-flex d-flex xs12 sm2 md3 lg3>
                          <v-img
                            height="100px"
                            width="100px"
                            v-if="this.newEquipo.imagePath"
                            :src="imageBase + this.newEquipo.imagePath">                            
                          </v-img>
                        </v-flex>
                      </v-layout>
                    </v-container>
                   </v-card-text>
                 </v-card>
                <v-divider></v-divider>                -->
                <v-subheader>Historial de calibraciones | 
                  <v-btn color="info" @click="handleCalib()" flat>{{txtCalAddbtn}}</v-btn> | 
                  <v-btn v-if="addCalCardVisible" color="success" @click="sumbitCal()" flat>Guardar registro</v-btn>
                </v-subheader> 
               
                <v-card class="ma-2" v-if="addCalCardVisible" color="#B8ECB1" >
                  <v-card-text>
                   <v-container fluid grid-list-xl>
                     <v-layout row wrap>
                       <v-flex d-flex xs12 sm6 md3 lg3>
                        <v-select
                          :items="['Calibración','Verificación']"
                          label="Tipo de procedimiento"          
                          v-model="newCal.tipo"                      
                        ></v-select>       
                       </v-flex>
                        <v-flex d-flex xs12 sm6 md3 lg3>
                          <v-select
                            :items="this.users"
                            item-text="nombreCompleto"
                            item-value="id"
                            label="Ejecutado por"          
                            v-model="newCal.idUsuario"                      
                          ></v-select>                 
                       </v-flex>
                       <v-flex d-flex xs12 sm6 md3 lg3>
                          <v-text-field
                            label="Observaciones..."
                            v-model="newCal.observaciones"
                          ></v-text-field>
                       </v-flex>
                       <v-flex d-flex xs12 sm6 md3 lg3 class="white" style="border-radius: 10px;">
                        <el-upload
                          class="upload-demo"
                          :auto-upload="false"
                          action="/api/Calibraciones/upload"
                          :http-request="addAttachmentCal"
                          :on-remove="handleRemoveCal"
                          :on-change="onChangeFileCal"
                          :limit="1"
                          :data="newCal"
                          ref="uploadCal"
                          :file-list="fileCalList">
                          <el-button size="small" type="success">Clic para subir archivo</el-button>                         
                        </el-upload>
                       </v-flex>             
                     </v-layout>
                   </v-container>
                  </v-card-text>
                </v-card>

                <v-card class="ma-2" color="#E9FADE" >
                  <v-card-text>
                    <v-data-table expand               
                    :headers="headers"
                    :items="calibraciones"
                    :search="busqueda">
                    <template v-slot:items="props">                          
                        <td >{{ props.item.fecha }}</td>
                        <td >{{ props.item.tipo }}</td>
                        <td >{{ props.item.nombresUsuario }}</td>
                        <td >{{ props.item.observaciones }}</td>
                        <td >{{ props.item.file_path }}</td>
                        <td class="justify-center layout px-0">
                            <v-btn
                                small
                                color="success"
                                @click="downloadFile(props.item.file_path)"
                                class="white--text">
                                Descargar
                                <v-icon right dark>cloud_download</v-icon>
                            </v-btn>
                            <v-btn
                                small
                                color="red"
                                class="white--text"    
                                @click="deleteItem(props.item)"                               >
                                Eliminar
                                <v-icon right dark>delete</v-icon>
                            </v-btn>
                        </td>   
                    </template>
                    <template v-slot:no-results>
                        <v-alert :value="true" color="error" icon="warning">
                        Tu búsqueda para "{{ busqueda }}" no tuvo resultados.
                        </v-alert>
                    </template>
                    </v-data-table>
                  </v-card-text>
                </v-card>
              </v-card>
            </v-dialog>

            <v-divider></v-divider>

            <!-- TABLA DE EQUIPOS -->
            <v-data-iterator class="mt-4"                
                :items="this.eqpss"
                :value="selected"
                item-key="id"
                :rows-per-page-items="rowsPerPageItems"
                :pagination.sync="pagination"
                content-tag="v-layout"
                style="height: 60vh; overflow: auto"
                row
                wrap>
                <template v-slot:item="props" >
                    <v-flex class="pa-1"
                    xs12 sm6 md4 lg3>
                    <v-card style="border: solid 1px #B5B2B1;border-radius: 8px;">
                        <v-img
                          class="white--text"
                          height="200px"
                          v-if="props.item.imagePath"
                          :src="imageBase + props.item.imagePath">
                          <v-container fill-height fluid justify-center height="10">
                              <v-layout fill-height align-end  >
                              <v-flex xs12 align-self-end flexbox class="elevation-10" style="background-color: white;  border-radius:10px;" >                              
                                  <span class="text-md-center text-lg-center font-weight-black blue--text pa-1 " v-text="props.item.nombre"></span>                                                        
                              </v-flex>
                              </v-layout>
                          </v-container>
                        </v-img>
                        <v-img
                          class="white--text"
                          height="200px"
                          v-else-if="props.item.estatusId == 471"
                          :src="require('@/assets/images/ico_met_2.jpg')">
                          <v-container fill-height fluid justify-center height="10">
                              <v-layout fill-height align-end  >
                              <v-flex xs12 align-self-end flexbox class="elevation-10" style="background-color: white;  border-radius:10px;" >
                              
                                  <span  class="text-md-center text-lg-center font-weight-black blue--text pa-1 " v-text="props.item.nombre"></span>
                                                        
                              </v-flex>
                              </v-layout>
                          </v-container>
                        </v-img>
                        <v-img
                          class="white--text"
                          height="200px"
                          v-else
                          :src="require('@/assets/images/ico_instru.png')">
                          <v-container fill-height fluid justify-center height="10">
                              <v-layout fill-height align-end  >
                              <v-flex xs12 align-self-end flexbox class="elevation-10" style="background-color: white;  border-radius:10px;" >
                              
                                  <span  class="text-md-center text-lg-center font-weight-black blue--text pa-1 " v-text="props.item.nombre"></span>
                                                        
                              </v-flex>
                              </v-layout>
                          </v-container>
                        </v-img>
                        <v-card-title>
                            <div>
                                <span style="font-weight: bold;" class="grey--text">CODIGO:</span> <span>{{props.item.código}}</span><br>
                            
                                <span style="font-weight: bold;">TIPO:</span> <span> {{props.item.tipoInstrumentoDesc}}</span> <br>
                                <span style="font-weight: bold;">MARCA:</span> <span> {{props.item.marcaDesc}}</span><br>
                                <span style="font-weight: bold;">AREA:</span> <span> {{props.item.areaDesc}}</span><br>
                                
                            </div>
                        </v-card-title>
                        <v-divider></v-divider>
                        <v-card-actions >
                          <v-btn @click="prepareEditClick(props.item)" class="ml-0" small color="primary">                
                            Editar
                            <v-icon right dark>create</v-icon>
                          </v-btn>
                          <v-btn small color="success" class="ml-2" @click="showHv(props.item)" >
                              Hoja de vida
                          </v-btn>
                        </v-card-actions>
                    </v-card>
                    </v-flex>
                </template>
            </v-data-iterator>

        </v-layout>
        </v-slide-y-transition>
    </v-card>     
  </v-container>  
</template>

<script lang="ts">
//Importación de namespaces necesarios
import { Component, Vue } from 'vue-property-decorator';
import { Equipos } from '../models/Equipos';
import { State, Action, Getter,namespace,Mutation } from "vuex-class";
import {Upload,Dialog} from 'element-ui';
import {ValidationProvider,ValidationObserver} from 'vee-validate';
import axios from 'axios';
import { isBoolean } from 'util';

//Namespaces de vuex
const eq_namespace = namespace('equipo');
const td_namespace = namespace('tipo_data');
const u_namespace = namespace('usuarios');

//Inicio de la clase
@Component({})
export default class EquiposView extends Vue {

  @eq_namespace.Getter
  private getEquipos!: Equipos[];

  @eq_namespace.Getter
  private getFiltro!: any;
  
  @eq_namespace.Action
  private getEquiposAsync: any;

  @eq_namespace.Action
  private setEquipos: any;

  @eq_namespace.Action
  private addEquipoAsync: any;

  @eq_namespace.Action
  private editEquipoAsync: any;

  @td_namespace.Getter
  private getTiposData!: any;

  @td_namespace.Getter
  private getTipoIntrumento!: any;

  @td_namespace.Action
  private getAllTiposDataAsync: any;

  @td_namespace.Action
  private getAllTipoInstrumentoAsync: any;

  @td_namespace.Action
  private setFiltro:any;

  @u_namespace.Action
  private getAllUsuariosAsync:any;

  @eq_namespace.Action
  private addCalibracionAsync: any;

  @eq_namespace.Action
  private getCalibracionesAsync: any;

   @eq_namespace.Action
  private deleteCalibracionAsync: any;

  private eqpss: any = [];
  private users = [];

  //Info de hoja de vida
  private txtCalAddbtn = 'Agregar calibración';
  private addCalCardVisible = false;
  //Datos de tabla 
  private headers = [
          { text: 'Fecha', align: 'left', value: 'fecha'},
          { text: 'Tipo procedimiento', value: 'tipo', sortable: true },
          { text: 'Ejecutado por', value: 'nombresUsuario', sortable: true, align: 'center', },   
          { text: 'Observaciones', value: 'observaciones', sortable: true },     
          { text: 'Adjunto', value: 'file_path', sortable: false },
          { text: 'Acciones', value: 'name', sortable: false },
          
        ];
  private calibraciones = [];
  private busqueda = '';

  private headDialogText='Nuevo equipo';
  private bodyDgText = '';
  private headDgText = '';
  private str_tdas = 'Todas...';
  private str_tdos = 'Todos...';
  private search = '';
  private dialogAdd = false;
  private dialogVal = false;
  private dialogLoad = true;
  private isAdding = true;
  private bFechIngreso = false;
  private numImages = 0;
  private numFiles = 0;
  private selected = [];
  private dlgHv = false;
  private rules = {
        required: value => !!value || 'Campo requerido...'
      }

  //Imagen
  private dialogImageUrl= '';
  private dialogVisible = false;

  private newEquipo:Equipos = this.emptyEquipo();
  private newCal = {tipo:'',idUsuario:0,observaciones:''};
  private imageBase = window.location.origin + '/api/Equipos/image?image=';
  private fileBase = window.location.origin + '/api/Equi;pos/file?file='
  private imgMet = 'ico_met_2.jpg';
  private imgIns = 'ico_instru.png';
  private fileList = [];
  private fileCalList = [];

  private updating = false;
  private rowsPerPageItems = [20, 100, 500];
  private pagination =  { rowsPerPage: 20 };

  private async beforeMount() {
    let self = this;  
    
    await Promise.all([this.getAllTiposDataAsync(),this.getAllTipoInstrumentoAsync()])
      .then(t => {

        var params ={d1:t[0].data, d2:t[1].data};
        self.setFiltro(params);

      });
                        
      await this.getEquiposAsync().then((r) => {
        this.eqpss = r.data; 
        
        this.dialogLoad = false;
      });

      //Cargar usuarios
    await this.getAllUsuariosAsync().then((r) => {
        self.users = r.data; 
    }); 
  }

  private aplicarFiltro(eliminar){
    try {
      if (eliminar) 
      {
        this.getFiltro.marca_selected = this.str_tdas;
        this.getFiltro.area_selected = this.str_tdas;
        this.getFiltro.tipo_selected = this.str_tdos;
        this.getFiltro.estatus_selected = [0,1];

        this.eqpss = this.getEquipos;
      }    
      else{
        
      }
      this.getFiltro.menu = false;
      this.updateData();

    } catch (error) {
      console.log(`Error en la manipulación del filtro!: ${error}`);
    }    
  }

  private updateData() {
    this.updating = true;
    var eqs = this.getEquipos;
      if (this.getFiltro) {
          eqs = eqs.filter((equipo) =>{
              var condiciones = [];
               
               if (this.getFiltro.marca_selected != this.str_tdas) {
                 condiciones.push(this.getFiltro.marca_selected.includes(equipo.marcaDesc));
               }
              
              if (this.getFiltro.area_selected != this.str_tdas) {
                condiciones.push(this.getFiltro.area_selected.includes(equipo.areaDesc));
              }
              
              if (this.getFiltro.estatus_selected) {
                condiciones.push(this.getFiltro.estatus_selected.includes(equipo.estatusDesc == 'Crítico' ? 0 : 1));
              }
              
              if (this.getFiltro.tipo_selected != this.str_tdos) {
                condiciones.push(this.getFiltro.tipo_selected.includes(equipo.tipoInstrumentoDesc));
              }
              return condiciones.every(Boolean);
          });
        }
    
    if (this.search !== '') {        
         const term = this.search.toLowerCase();
         
        var p = eqs.filter((equipo) =>   
            equipo.código.toLowerCase().includes(term) ||
            equipo.nombre.toLowerCase().includes(term) ||
            equipo.serie.toLowerCase().includes(term)
        );
        eqs = p;
      }

    //Fin del filtro 
    this.eqpss = eqs;
    this.updating = false;
  }

  private getExactDate(){
    var tzoffset = (new Date()).getTimezoneOffset() * 60000; //offset in milliseconds
    var localISOTime = (new Date(Date.now() - tzoffset)).toISOString().slice(0, -1);
    return localISOTime.substr(0, 10);
  }

  private filterTipo(tipo)
  {
    return this.getTiposData.filter(td => td.idTipo == tipo);
  }

  private handleRemove(file, fileList) {
       console.log('cantidad :', fileList.length);
       this.numImages = fileList.length;
  }

  private handlePictureCardPreview(file) {
        this.dialogImageUrl = file.url;
        this.dialogVisible = true;
  }

  /*Evento que se dispara cuando se presiona click en agregar 
  nuevo equipo*/
  private addEquipoClick(){
    //Asignar el objeto al mismo que se agrega
    this.newEquipo = this.emptyEquipo();
    //Indicar que se va a gregar
    this.isAdding = true;
    //Cambiar el titulo de la edicion
    this.headDialogText = 'Nuevo equipo';
    //Vaciar imagenes asignadas
    this.fileList = []; 
  }

  private async addEquipo()
  {
    if (this.newEquipo.AreaId > 0 &&
          this.newEquipo.estatusId > 0 &&
            this.newEquipo.MarcaId > 0   &&
              this.newEquipo.tipoInstrumentoId > 0 &&
                this.newEquipo.código.length > 0   &&
                  this.newEquipo.nombre.length > 0) 
    {
      //Verificar si hay imagen    
      if (this.numImages > 0) 
      {
        //Verificar que la imagen no sea la de referencia
        if (this.newEquipo.imagePath == this.imgMet 
              || this.newEquipo.imagePath == this.imgIns) {
                await this.sendDataWithoutImage();
        }
        else {
          //Tener en cuenta que si la imagen no cambia, no se parte desde el envío
          //de la foto
          //Disparar evento de envío de imagen
          var env = (this.$refs.upload as Vue & { submit: () => boolean });
          env.submit();
        }         
      }
      else 
      {
         await this.sendDataWithoutImage();
      }           

      this.dialogAdd = false;   
    }
    else{
      this.buildDialog('ATENCIÓN','Los campos no deben estar en blanco.');
    }    
  }

  private async sendDataWithoutImage(){
    //Indicar parámetros sin imagen
    var params ={data:this.newEquipo, image:false};

    if (this.isAdding) {
      await this.addEquipoAsync(params).then((r) => {
        //Imprimir mensaje de respuesta de servidor
        
         if (r.data.code == 1) {
          this.buildDialog('CREACIÓN DE EQUIPO','¡Equipo agregado correctamente!');
          //Recargar equipos
          this.getEquiposAsync().then((r) => {
              this.eqpss = r.data; 
            
              this.dialogLoad = false;
          });
        }
        else{
          this.buildDialog('CREACIÓN DE EQUIPO','¡Hubo un error al agregar equipo, intente nuevamente!');
          console.log('response :', r);
        }
      });
    }
    else{
      await this.editEquipoAsync(params).then((r) => {
        //Imprimir mensaje de respuesta de servidor
        // console.log('r :', r.data.code);
        if (r.data.code == 1) {
          this.buildDialog('EDICIÓN DE EQUIPO','¡Equipo editado correctamente!');
          //Recargar equipos
          this.getEquiposAsync().then((r) => {
              this.eqpss = r.data; 
            
              this.dialogLoad = false;
          });
        }
        else{
          this.buildDialog('EDICIÓN DE EQUIPO','¡Hubo un error al editar equipo, intente nuevamente!');
          console.log('response :', r);
        }

      });
    }
  }
  
  private async addAttachment(data)
  {  
    console.log('Se envía data con imagen '+this.isAdding);  
    var params ={data:data, image:true};
    if (this.isAdding) {
      await this.addEquipoAsync(params).then( (r) => {
            // console.log('r :', r.data.code);
            if (r.data.code == 1) {
              this.buildDialog('CREACIÓN DE EQUIPO','¡Equipo agregado correctamente!');
              //Recargar equipos
              this.getEquiposAsync().then((r) => {
                  this.eqpss = r.data; 
                
                  this.dialogLoad = false;
              });
            }
            else{
              this.buildDialog('CREACIÓN DE EQUIPO','¡Hubo un error al agregar equipo, intente nuevamente!');
              console.log('response :', r);
            }
          }
        );
    }
    else{
       await this.editEquipoAsync(params).then((r) => {
        // console.log('r :', r.data.code);
        if (r.data.code == 1) {
          this.buildDialog('EDICIÓN DE EQUIPO','¡Equipo editado correctamente!');
          //Recargar equipos
          this.getEquiposAsync().then((r) => {
              this.eqpss = r.data; 
            
              this.dialogLoad = false;
          });
        }
        else{
          this.buildDialog('EDICIÓN DE EQUIPO','¡Hubo un error al editar equipo, intente nuevamente!');
          console.log('response :', r);
        }
      });
    }
    
  }

  //Metodo que se dispara cuando se cambia la imagen
  private onChangeFile(file, fileList){
    console.log('cantidad de imágenes:', fileList.length);
    this.numImages = fileList.length;
  }

  private prepareEditClick(equipoEdit){   
    //Asignar el objeto al mismo que se agrega
    this.newEquipo = this.emptyEquipo();
    //Mapear equipo en nuevo objeto
    this.newEquipo.id = equipoEdit.id;
    this.newEquipo.nombre = equipoEdit.nombre;
    this.newEquipo.código = equipoEdit.código;
    this.newEquipo.fechaAdd = equipoEdit.fechaIngreso.substr(0, 10);
    this.newEquipo.Rango = equipoEdit.rango;
    this.newEquipo.MedidaRango = equipoEdit.medidaRango;
    this.newEquipo.MaxRango = equipoEdit.maxRango;
    this.newEquipo.DivisiónEscala = equipoEdit.divisiónEscala;
    this.newEquipo.Tipo = equipoEdit.tipo;
    this.newEquipo.serie = equipoEdit.serie;
    this.newEquipo.MarcaId = equipoEdit.marcaId;    
    this.newEquipo.AreaId = equipoEdit.areaId;
    this.newEquipo.estatusId = equipoEdit.estatusId;
    this.newEquipo.tipoInstrumentoId = equipoEdit.tipoInstrumentoId;
    this.newEquipo.imagePath = equipoEdit.imagePath;

    //Vaciar imagenes asignadas
    this.fileList = []; 
    
    //Si el equipo tiene imagen
    if (equipoEdit.imagePath) {
      this.fileList.push(
        {
          name:equipoEdit.imagePath,
          url:this.imageBase + equipoEdit.imagePath 
        });
    }    
    else{
      //Si el equipo no tiene imagen pero es de tipo metrología
      if (equipoEdit.estatusId == 471) {
        const urlMet = require('@/assets/images/ico_met_2.jpg');
        this.fileList.push(
        {
          name:this.imgMet,
          url:urlMet
        });
      }
      else{
        const urlIns = require('@/assets/images/ico_instru.png');
        this.fileList.push(
        {
          name:this.imgIns,
          url:urlIns
        });
      }        
    }

    //Cambiar el titulo de la edicion
    this.headDialogText = 'Edición de equipo';
    //Indicar que se está editando 
    this.isAdding = false;
    //Mostrar dialogo de edición 
    this.dialogAdd = true;
  }

  private emptyEquipo():Equipos {
    return {
      id:0,
      código : '',
      nombre:'',
      Rango:'',
      MedidaRango:0,
      MaxRango:'',
      DivisiónEscala:'',
      MarcaId:0,
      marcaDesc:'',
      Tipo:'',
      serie:'',
      FechaIngreso:new Date(),
      estatusId:0,
      estatusDesc:'',
      BuscarPor:'',
      tipoInstrumentoId:0,
      tipoInstrumentoDesc:'',
      AreaId:0,
      areaDesc:'',
      fechaAdd:this.getExactDate(),
      imagePath:''    
    };
  }

  private buildDialog(head,body){
      this.dialogVal = true;    
      this.bodyDgText = body;
      this.headDgText = head;
  }

  private async showHv(item){
    
    this.dialogLoad = true;

    this.newEquipo = Object.assign({}, item);
    console.log('this.newEquipo :', this.newEquipo);

    await this.getCalibracionesAsync(this.newEquipo.id).then((r) => {
            this.calibraciones = r.data;   
            this.dialogLoad = false;
            this.dlgHv = true;
    });

    //limpiar campos
      this.newCal = {tipo:'',idUsuario:0,observaciones:''};
      this.addCalCardVisible = false;
      this.txtCalAddbtn = 'Agregar calibración';
  }

  private async addAttachmentCal(data){

    if(data.file.size <= 31457280)
     { 
       data.data.idEquipo = this.newEquipo.id;
       this.dialogLoad = true;
       await this.addCalibracionAsync(data).then( (r) => {
            // console.log('r :', r.data.code);
            if (r.data.code == 1) {
              this.buildDialog('CALIBRACIONES','Calibración agregada correctamente!');
              //Recargar calibraciones
              this.getCalibracionesAsync(this.newEquipo.id).then((r) => {
                this.calibraciones = r.data;   
                this.dialogLoad = false;                
              });

              this.addCalCardVisible = false;   
              this.txtCalAddbtn = 'Agregar calibración';
            }
            else{
              this.buildDialog('CALIBRACIONES','¡Hubo un error al agregar calibración, intente nuevamente!');
              console.log('response :', r);
            }
          });   
      } 
      else{
         this.buildDialog('CALIBRACIONES','Asegúrese que el archivo no pese mas de 30 MB.');
      }
  }

  private handleRemoveCal(file, fileList) {
       console.log('cantidad archivos:', fileList.length);
       this.numFiles = fileList.length;
  }

  //Metodo que se dispara cuando se cambia la imagen
  private onChangeFileCal(file, fileList){
    console.log('cantidad de archivos:', fileList.length);
    this.numFiles= fileList.length;
  }

  private handleCalib(){
    this.addCalCardVisible = !this.addCalCardVisible;
    if (this.addCalCardVisible) {
      this.txtCalAddbtn = 'Cancelar';
    }
    else{
      this.txtCalAddbtn = 'Agregar calibración';
      //limpiar campos
      this.newCal = {tipo:'',idUsuario:0,observaciones:''};
    }
  }

  private async sumbitCal(){
    if (this.newCal.tipo.length > 0 &&
          this.newCal.idUsuario > 0 &&
            this.newCal.observaciones.length > 0   &&
              this.numFiles > 0) 
    {

      //Disparar evento de envío de imagen
      var env = (this.$refs.uploadCal as Vue & { submit: () => boolean });
      env.submit();
     
    }
    else{
      this.buildDialog('ATENCIÓN','Los campos de calibración no deben estar en blanco.');
    }    
  }
  
  private downloadFile(file){
   
    window.open( window.location.origin + '/api/Equipos/file?file=' + file, '_blank');
  }

  private async deleteItem(item){
     
    var res = confirm('¿Desea eliminar el registro que contiene : '+item.file_path + '?. Recuerde que el archivo será eliminado permanentemente.');
    if(res){
      this.dialogLoad = true;
      await this.deleteCalibracionAsync(item.id).then((r) => {
        
          if (r.data.code == 1) {
              
              //Recargar calibraciones
              this.getCalibracionesAsync(this.newEquipo.id).then((r) => {
                this.calibraciones = r.data;   
                this.dialogLoad = false;                
              });
          }
          else{
              this.buildDialog('ELIMINAR REGISTRO','No se pudo eliminar parámetro.');
              console.log('response :', r);
          }
      });      
      
    
    }      
  }
}

</script>

<style scoped>
.toolbar {
  color: white !important;
}
.margin-rx-10 {
  margin-right: 10px;
}
.filtri .subheading {
  padding: 10px 16px !important;
}
.filtri .v-list__tile__content {
  margin-bottom: 10px;
}
.filtri .v-list__tile__action {
  width: 98%;
}
.filtri {
  max-width: 400px;
}

.v-input {
    font-size: 13px;
}

.v-input input {
    font-size: 13px;
}

.v-list__tile{
  font-size: 13px;
}


@import url("//unpkg.com/element-ui@2.12.0/lib/theme-chalk/index.css");


</style>