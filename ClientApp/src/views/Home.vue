<template>
  <v-container fluid>
    <v-layout column align-center>
      <img src="@/assets/met.png" alt="Vuetify.js" class="mb-5">
      <HelloWorld quote="Sistema para la gestión de equipos de metrología e instrumentación" author="En esta sección se visualizarán los reportes del sistema" />
    </v-layout>
    <hr class="mt-4"/>
    <v-slide-y-transition mode="out-in" >
      <v-layout column class="mt-4">        
        <h1 class="headline"> = Opciones disponibles en el lado izquierdo</h1>
      
          
         <div class="text-xs-left">
          <v-chip close>
            <v-avatar>
              <img src="https://www.ligentec.com/wp-content/uploads/2018/12/metrology.png" alt="metro...">
            </v-avatar>
            Sistema en construcción...
          </v-chip>
        </div>

        <div class="text-xs-left">
          <v-chip close>
            <v-avatar>
              <img src="https://www.lfc.com.sg/_assets/v2/dist/images/metrology.png" alt="metro...">
            </v-avatar>
            Versión 0.5 ...
          </v-chip>
        </div>
      
      </v-layout>
    </v-slide-y-transition>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import HelloWorld from '@/components/HelloWorld.vue'; // @ is an alias to /src

@Component({
  components: {
    HelloWorld,
  },
})
export default class Home extends Vue {}
</script>
