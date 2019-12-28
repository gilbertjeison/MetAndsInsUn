<template>
    <div class="about ma-5">
        <h1>Acerca de la Aplicación</h1>
        
         <div v-for="(service,index) in services" :key="index">
                <p>{{service.summary}}</p>
           
        </div>
        <!-- <button class="btn-primary" @click="callApi">Call Apia</button> -->
        <button @click="callSecureApi">Call Secure API</button>
    </div>
</template>

<script lang="ts">
import {Component, Vue, Prop} from 'vue-property-decorator';
import {State, Action, Getter} from 'vuex-class';
import axios from 'axios';

@Component({})
export default class ApiAboutView extends Vue {

    private values = ['No data yet'];
    private services = ['No data yet (services)'];


    private async callApi() {
        try {
            const response = await axios.get('http://localhost:5000/api/SampleData/WeatherForecasts');
            this.values = response.data;
        } catch (err) {
            this.values = [];
            this.values.push('Oppss' + err);
        }
    }

    private async callSecureApi() {
        try {
            const response = await axios.get('http://localhost:5000/api/SampleData/WeatherForecasts');
            this.services = response.data;
        } catch (err) {
            this.services = [];
            this.services.push('Oppss' + err);
        }
    }
}
</script>