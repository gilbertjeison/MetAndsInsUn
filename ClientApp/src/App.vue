<template>
  <v-app>
    <v-navigation-drawer
      persistent
      :mini-variant="miniVariant"
      :clipped="clipped"
      v-model="drawer"
      v-if="isAthenticated"
      enable-resize-watcher
      fixed
      app>

      <v-list dense>
        <template v-for="(item, i) in items">
          <v-subheader v-if="item.header" :key="i">{{ item.header }}</v-subheader>
          <v-list-tile v-else value="true" :key="i" :to="item.link">
            <v-list-tile-action>
              <v-icon v-html="item.icon"></v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title v-text="item.title"></v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
        </template>
      </v-list>
    </v-navigation-drawer>

    <v-toolbar dark app :clipped-left="clipped" color="primary"  v-if="isAthenticated">    
      <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon>
      <v-btn icon @click.stop="miniVariant = !miniVariant">
        <v-icon v-html="miniVariant ? 'chevron_right' : 'chevron_left'"></v-icon>
      </v-btn>
      <v-btn icon @click.stop="clipped = !clipped">
        <v-icon>web</v-icon>
      </v-btn>
      <v-toolbar-title v-text="title" class="hidden-sm-and-down"></v-toolbar-title>
      
      <v-spacer></v-spacer>

      <v-menu
        offset-y
        origin="center center"
        :nudge-right="140"
        :nudge-bottom="10"
        transition="scale-transition"
      >
        <v-btn icon large flat slot="activator">
          <v-avatar size="30px">
             <v-icon >person</v-icon>
          </v-avatar>
        </v-btn>
        <v-list class="pa-0">
          <v-list-tile
            v-for="(item,index) in menuItems"
            :to="!item.href ? { name: item.name } : null"
            :href="item.href"
            @click="item.click"
            ripple="ripple"
            :disabled="item.disabled"
            :target="item.target"
            rel="noopener"
            :key="index"
          >
            <v-list-tile-action v-if="item.icon">
              <v-icon>{{ item.icon }}</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>{{ item.title }}</v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
        </v-list>
      </v-menu>
    </v-toolbar>

    <v-content>
      <router-view />
    </v-content>

    <v-footer app>
      <span>&nbsp;Developed by Jeison Perlaza&nbsp;&copy;&nbsp;{{dateYear}}</span>
    </v-footer>
  </v-app>
</template>

<script lang="ts">
import HelloWorld from "@/components/HelloWorld.vue";
import { Component, Vue } from "vue-property-decorator";
import {State,Getter,Action,namespace} from 'vuex-class';

const auth_namespace = namespace('auth');
const equipo_namespace = namespace('equipo');

@Component({
  components: { HelloWorld }
})
export default class App extends Vue {
   @auth_namespace.Getter
  private isAthenticated!: boolean;

  @auth_namespace.Action
  private signOut: any;

  @equipo_namespace.Action
  private filterEquipos: any;

  private clipped: boolean = true;
  private dateYear: number = new Date().getFullYear();
  private drawer: boolean = true;
  private miniVariant: boolean = false;
  private right: boolean = true;
  private title: string = "Metrology Management";
  private items = [
    { header: "Menú principal" },
    { title: "Inicio", icon: "home", link: "/" },
    { title: "Equipos", icon: "settings_input_hdmi", link: "/equipos" },
    { title: "Parametrización", icon: "get_app", link: "/parameters" },
    { title: "Ubicaciones", icon: "navigation", link: "/location" },
    { title: "Programación anual", icon: "schedule", link: "/scheduler" },
    // { title: "Api About", icon: "info", link: "/about" }
  ];

  private menuItems = [
    {
      icon: "account_circle",
      href: "#",
      title: "Perfil",
      click: e => {
        console.log(e);
      }
    },    
    {
      icon: "fullscreen_exit",
      href: "#",
      title: "Cerrar sesión",
      click: this.signOut
    }
  ];


}
</script>
