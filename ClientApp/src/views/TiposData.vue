
<template>
    <v-container fluid>
        <v-card class="pa-3">
            <v-slide-y-transition mode="out-in">
                <v-layout column>
                    <div>
                        <v-alert style="border-radius:10px;" 
                        :value="true"
                        color="info"
                        icon="storage"
                        outline>  
                         <v-layout row wrap justify-end >
                             <v-flex> 
                                 <h1 class="text-md-center">Parametrización de datos</h1>
                            </v-flex>
                         </v-layout>                       
                            <v-select
                                :items="this.tipos"
                                item-text="descripcion"
                                item-value="id"
                                label="Seleccione el tipo de parámetro"                                 
                                v-model="tipo_selected"
                                v-on:change="changeTipo"
                            ></v-select>
                        </v-alert>
                    </div>
                    <v-divider></v-divider>

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

                    <!-- TABLA DE CONTENIDO -->                   
                     <v-card class="mt-4">
                        <v-card-title>
                        <!-- DIALOGO TIPOS DATA -->
                        <v-dialog persistent v-model="dialog" max-width="500px" v-if="tipo_selected != last_tipo_id">
                            <template v-slot:activator="{ on }">
                                <v-btn outline color="primary" v-on="on">
                                    Agregar
                                    <v-icon right dark>library_add</v-icon>
                                </v-btn>
                            </template>
                            <v-card>
                            <v-card-title style="background-color:#1e88e5;">
                                <span class="headline white--text">{{ formTitle }}</span>
                            </v-card-title>

                            <v-card-text>
                                <v-container grid-list-md>
                                <v-layout wrap>
                                    <v-flex xs12>
                                        <v-select
                                            :items="this.tipos"
                                            item-text="descripcion"
                                            item-value="id"
                                            label="Tipo de parámetro"                                 
                                            v-model="tipo_selected"
                                            :rules="[this.rules.required]"
                                        ></v-select>
                                    </v-flex>
                                    <v-flex xs12>
                                        <v-text-field 
                                            v-model="editedItem.descripcion" 
                                            label="Descripción"
                                            :rules="[this.rules.required]">
                                        </v-text-field>
                                    </v-flex>                                
                                </v-layout>
                                </v-container>
                            </v-card-text>

                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn color="blue darken-1" flat @click="close">Cancelar</v-btn>
                                <v-btn color="blue darken-1" flat @click="save(0)">Guardar</v-btn>
                            </v-card-actions>
                            </v-card>
                        </v-dialog>
                        <!-- DIALOGO TIPO INSTRUMENTO -->
                        <v-dialog persistent v-model="dialog_ti" max-width="500px" v-else>
                            <template v-slot:activator="{ on }">
                                <v-btn outline color="primary" v-on="on">
                                    Agregar
                                    <v-icon right dark>library_add</v-icon>
                                </v-btn>
                            </template>
                            <v-card>
                            <v-card-title style="background-color:#1e88e5;">
                                <span class="headline white--text">{{ formTitle }}</span>
                            </v-card-title>
                            <v-card-text>
                                <v-container grid-list-md>
                                <v-layout wrap>
                                    <v-flex xs12>
                                         <v-text-field 
                                            v-model="editedItem_ti.codigo" 
                                            label="Código"
                                            placeholder="ABC..."
                                            @change="change()"
                                            :loading="loadingCod"
                                            :rules="[this.rules.required]">
                                        </v-text-field>
                                    </v-flex>
                                    <v-flex xs12>
                                        <v-text-field 
                                            v-model="editedItem_ti.descripcion" 
                                            label="Descripción"
                                            :rules="[this.rules.required]">
                                        </v-text-field>
                                    </v-flex>                                
                                </v-layout>
                                </v-container>
                            </v-card-text>

                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn color="blue darken-1" flat @click="close">Cancelar</v-btn>
                                <v-btn color="blue darken-1" flat @click="save(1)">Guardar</v-btn>
                            </v-card-actions>
                            </v-card>
                        </v-dialog>
                        <v-spacer></v-spacer>
                        <v-text-field
                            v-model="busqueda"
                            append-icon="search"
                            label="Buscar..."
                            single-line
                            hide-details
                        ></v-text-field>
                        </v-card-title>
                        <hr class="ma-3"/>
                        <v-data-table
                        v-if="tipo_selected != last_tipo_id"
                        :headers="headers"
                        :items="items"
                        :search="busqueda">
                        <template v-slot:items="props">                          
                            <td >{{ props.item.id }}</td>
                            <td >{{ props.item.descripcion }}</td>
                            <td class="justify-center layout px-0">
                                <v-btn
                                    small
                                    color="blue"
                                    class="white--text"
                                    @click="editItem(props.item,0)">
                                    Editar
                                    <v-icon right dark>edit</v-icon>
                                </v-btn>
                                <v-btn
                                    small
                                    color="red"
                                    class="white--text"
                                    @click="deleteItem(props.item,0)"                                    >
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
                        <v-data-table
                        v-else
                        :headers="headers_ti"
                        :items="items_ti"
                        :search="busqueda">
                        <template v-slot:items="props">                          
                            <td >{{ props.item.id }}</td>
                            <td >{{ props.item.codigo }}</td>
                            <td >{{ props.item.descripcion }}</td>
                            <td class="justify-center layout px-0">
                                <v-btn
                                    small
                                    color="blue"
                                    class="white--text"
                                    @click="editItem(props.item,1)">
                                    Editar
                                    <v-icon right dark>edit</v-icon>
                                </v-btn>
                                <v-btn
                                    small
                                    color="red"
                                    class="white--text"
                                    @click="deleteItem(props.item,1)"                                    >
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
                    </v-card>
                </v-layout>
            </v-slide-y-transition>
        </v-card>
    </v-container>
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { State, Action, Getter,namespace,Mutation } from "vuex-class";
import { TiposData } from '../models/TiposData';
import { TipoInstruento } from '../models/TipoInstrumento';

//Namespaces de vuex
const td_namespace = namespace('tipo_data');

@Component({})
export default class TiposDataView extends Vue{
  
  @td_namespace.Action
  private getTiposAsync: any;

  @td_namespace.Action
  private getAllTiposDataByTypeAsync: any;

  @td_namespace.Action
  private addTipoDataAsync: any;

  @td_namespace.Action
  private editTipoDataAsync: any;

  @td_namespace.Action
  private deleteTipoDataAsync: any;

  @td_namespace.Action
  private deleteTipoInstrumentoAsync: any;

  @td_namespace.Action
  private getAllTipoInstrumentoAsync: any;

  @td_namespace.Action
  private getTIByIdAsync: any;

  @td_namespace.Action
  private addTipoInstrumentoAsync: any;

  @td_namespace.Action
  private editTipoInstrumentoAsync: any;

  private search = '';
  private tipos: any = [];
  private tipo_selected = 1;
  private busqueda = '';
  private last_tipo_id = 0;
  private dialog = false;
  private dialog_ti = false;
  private bodyDgText = '';
  private headDgText = '';
  private dialogVal = false;
  private dialogLoad = true;
  private loadingCod = false;
  private desc_qr = '';
  private editedItem:TiposData = {
      id:0,
      idTipo:0,
      descripcion:''
  };

   private editedItem_ti:TipoInstruento = {
      id:0,
      codigo:'',
      descripcion:''
  };

  private editedIndex = -1;
  private rules = {
        required: value => !!value || 'Campo requerido...'
      }

  //Computed
  get formTitle() {
    return this.editedIndex === -1 ? 'Nuevo parámetro' : 'Editar parámetro'
  }

  //Datos de tabla 
  private headers = [
          { text: 'Id', align: 'left', value: 'id'},
          { text: 'Descripción', value: 'descripcion', sortable: true },
          { text: 'Acciones', value: 'name', sortable: false,align: 'center', }        
        ];
  private items = [];

  private headers_ti = [
          { text: 'Id', align: 'left', value: 'id'},
          { text: 'Código', value: 'codigo' },
          { text: 'Descripción', value: 'descripcion', sortable: true },
          { text: 'Acciones', value: 'name', sortable: false,align: 'center', }        
        ];
  private items_ti = [];

  private async beforeMount() {
    let self = this;  
         
      await this.getTiposAsync().then((r) => {
          self.tipos = r.data;
          
        if (self.tipos.length > 0) {
             self.last_tipo_id = self.tipos[ self.tipos.length-1].id;
        }                  
      });  
      
      this.getAllTiposDataByTypeAsync(this.tipo_selected).then((r) => {
            this.items = r.data;   
            this.dialogLoad = false;
        });
  }

  private async changeTipo(param){
       let self = this;  

       //Dependiendo la opción seleccionada en el combo, mostrar la data
       if (param != this.last_tipo_id) {
            await this.getAllTiposDataByTypeAsync(param).then((r) => {           
                self.items = r.data;
            });
       }
       else{
           //Tabla tipo instrumento
            await this.getAllTipoInstrumentoAsync().then((r) => {           
                self.items_ti = r.data;
            });
       }
  }

  private editItem(tde,i){
      if(i == 0){
        this.editedIndex = this.items.indexOf(tde);
        this.editedItem = Object.assign({}, tde);
        this.dialog = true;
      }
      else{
        this.editedIndex = this.items_ti.indexOf(tde);
        this.editedItem_ti = Object.assign({}, tde);
        this.dialog_ti = true;
      }      
  }

  private async deleteItem(tde,i){
      if(i == 0){
        var res = confirm('¿Desea eliminar el parámetro: '+tde.descripcion + '?');
        if(res){
            await this.deleteTipoDataAsync(tde).then((r) => {
             
                if (r.data.code == 1) {
                    this.buildDialog('ELIMINAR PARÁMETRO','¡Dato eliminado correctamente!');
                    //Recargar equipos
                    this.getAllTiposDataByTypeAsync(this.tipo_selected).then((r) => {
                        this.items = r.data;                         
                        this.dialog = false;
                    });
                }
                else{
                    this.buildDialog('ELIMINAR PARÁMETRO','No se pudo eliminar parámetro porque ciertos Equipos dependen de éste');
                    console.log('response :', r);
                }
            });      
        }
      }
      else{
        var res = confirm('¿Desea eliminar el parámetro: '+tde.descripcion + '?');
        if(res){
            await this.deleteTipoInstrumentoAsync(tde).then((r) => {
                if (r.data.code == 1) {
                    this.buildDialog('ELIMINAR PARÁMETRO','¡Dato eliminado correctamente!');
                    //Recargar equipos
                    this.getAllTipoInstrumentoAsync().then((r) => {
                        this.items_ti = r.data;                         
                        this.dialog_ti = false;
                    });
                }
                else{
                    this.buildDialog('ELIMINAR PARÁMETRO','No se pudo eliminar parámetro porque ciertos Equipos dependen de éste');
                    console.log('response :', r);
                }
            });      
        }
      }      
  }

  private close(){
      this.dialog = false;
      this.dialog_ti = false;
  }

  private async save(i){
      //Tipo data
      if (i == 0) {
          if (this.tipo_selected > 0
                && this.editedItem.descripcion.length > 0) {
            //Editar elemento
            if (this.editedIndex > 0) {
                await this.editTipoDataAsync(this.editedItem).then((r) => {
                    if (r.data.code == 1) {
                        this.buildDialog('EDICIÓN DE PARÁMETRO','¡Dato editado correctamente!');
                        //Recargar equipos
                        this.getAllTiposDataByTypeAsync(this.tipo_selected).then((r) => {
                            this.items = r.data;                        
                            this.dialog = false;
                        });
                    }
                    else{
                        this.buildDialog('EDICIÓN DE PARÁMETRO','¡Hubo un error al realizar acción, intente nuevamente!');
                        console.log('response :', r);
                    }
                });              
            }
            //Agregar elemento
            else{
                this.editedItem.idTipo = this.tipo_selected;
                await this.addTipoDataAsync(this.editedItem).then((r) => {
                    if (r.data.code == 1) {
                        this.buildDialog('CREACIÓN DE PARÁMETRO','¡Dato agregado correctamente!');
                        //Recargar equipos
                        this.getAllTiposDataByTypeAsync(this.tipo_selected).then((r) => {
                            this.items = r.data;                         
                            this.dialog = false;
                        });
                    }
                    else{
                        this.buildDialog('CREACIÓN DE PARÁMETRO','¡Hubo un error al realizar acción, intente nuevamente!');
                        console.log('response :', r);
                    }                    
                });
            }
        }
        else{
            this.buildDialog('ATENCIÓN','Los campos no deben estar en blanco.');
        }
      }
      //Tipo instrumento
      else{
          if (this.editedItem_ti.codigo.length > 0
                && this.editedItem_ti.descripcion.length > 0) {
            //Editar elemento
            if (this.editedIndex > 0) {                
                await this.editTipoInstrumentoAsync(this.editedItem_ti).then((r) => {
                    if (r.data.code == 1) {
                        this.buildDialog('EDICIÓN DE PARÁMETRO','¡Dato editado correctamente!');
                        //Recargar equipos
                        this.getAllTipoInstrumentoAsync().then((r) => {
                            this.items_ti = r.data;                        
                            this.dialog_ti = false;
                        });
                    }
                    else{
                        this.buildDialog('EDICIÓN DE PARÁMETRO','¡Hubo un error al realizar acción, intente nuevamente!');
                        console.log('response :', r);
                    }
                });              
            }
            //Agregar elemento
            else{
                await this.addTipoInstrumentoAsync(this.editedItem_ti).then((r) => {
                    if (r.data.code == 1) {
                        this.buildDialog('CREACIÓN DE PARÁMETRO','¡Dato agregado correctamente!');
                        //Recargar equipos
                        this.getAllTipoInstrumentoAsync().then((r) => {
                            this.items_ti = r.data;                         
                            this.dialog_ti = false;
                        });
                    }
                    else{
                        this.buildDialog('CREACIÓN DE PARÁMETRO','¡Hubo un error al realizar acción, intente nuevamente!');
                        console.log('response :', r);
                    }                    
                });
            }
        }
        else{
            this.buildDialog('ATENCIÓN','Los campos no deben estar en blanco.');
        }
      }      
  }
    
  private async change(){
      this.loadingCod = true;
      var self = this;
       await this.getTIByIdAsync(this.editedItem_ti.codigo).then((r) => {
           if (r.data.code == 1) {
               this.desc_qr = r.data.list.descripcion;            
                alert('El código ('+this.editedItem_ti.codigo+') ya está en uso por el parámetro ('+this.desc_qr+')');
                self.editedItem_ti.codigo = '';          
           }
           this.loadingCod = false;
       });
  }

  private buildDialog(head,body){
      this.dialogVal = true;    
      this.bodyDgText = body;
      this.headDgText = head;
  }
}
</script>