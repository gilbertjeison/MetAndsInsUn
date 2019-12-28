import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';
import { RootState } from './types';
import { counter } from './counter/index';
import {auth} from './auth/index';
import {equipo} from './equipos/index';
import {tipo_data} from './tipos_data/index';
import {programaciones} from './programaciones/index';
import {usuarios} from './usuarios/index';

Vue.use(Vuex);

// Vuex structure based on https://codeburst.io/vuex-and-typescript-3427ba78cfa8

const store: StoreOptions<RootState> = {
  state: {
    version: '1.0.0', // a simple property
  },
  modules: {
    counter,
    auth,
    equipo,
    tipo_data,
    programaciones,
    usuarios
  },
};

export default new Vuex.Store<RootState>(store);
