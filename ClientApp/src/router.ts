import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import store from './store';

Vue.use(Router);

let router = new Router({
  mode: 'history',
  base:process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/counter',
      name: 'counter',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('./views/Counter.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/fetch-data',
      name: 'fetch-data',
      component: () => import('./views/FetchData.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/equipos',
      name: 'equipos',
      component: () => import('./views/Equipos.vue'),
       meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/about',
      name: 'about',
      component: () => import('./views/ApiAbout.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/callback',
      name: 'callback',
      component: () => import('./views/Callback.vue'),
    },
    {
      path: '/main',
      name: 'main',
      component: () => import('./views/account/Main.vue'),
      
    },
    {
      path: '/parameters',
      name: 'parameters',
      component: () => import('./views/TiposData.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/scheduler',
      name: 'scheduler',
      component: () => import('./views/Scheduler.vue'),
      meta: {
        requiresAuth: true,
      },
    },
  ],
});


router.beforeEach( async (to, from, next) => {
  
  if (store.getters['auth/isAthenticated']) {    
    // already signed in, we can navigate anywhere
    
    next();
  } else if (to.matched.some((record) => record.meta.requiresAuth)) {
    // authentication is required. Trigger the sign in process, including the return URI 
    /* tslint:disable */   
    store.dispatch('auth/authenticate','/').then(() => {
      console.log('authenticating a proctected url:  :', to.path );
      next();
    });
  } else {
    // No auth required. We can navigate
    next();
  }
});


export default router;
