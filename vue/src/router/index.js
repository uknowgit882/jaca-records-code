import { createRouter as createRouter, createWebHistory } from 'vue-router'
import { useStore } from 'vuex'

// Import components
import HomeView from '../views/HomeView.vue';
import LoginView from '../views/LoginView.vue';
import LogoutView from '../views/LogoutView.vue';
import RegisterView from '../views/RegisterView.vue';
import SearchBox from '../components/SearchBox.vue';
import LibraryView from '../views/LibraryView.vue'
import CollectionsView from '@/views/CollectionsView.vue'
import DataView from '@/views/DataView.vue'
import SearchResultView from'@/views/SearchResultView.vue'
import ProfilePageView from '@/views/Caleb/ProfilePageView.vue'
import HomeA from '@/views/Caleb/HomeAView.vue'
import popupViewTest from '@/views/Aseel/popupViewTEST.vue'
import JakeView from '@/views/Jake/JakeView.vue'
/**
 * The Vue Router is used to "direct" the browser to render a specific view component
 * inside of App.vue depending on the URL.
 *
 * It also is used to detect whether or not a route requires the user to have first authenticated.
 * If the user has not yet authenticated (and needs to) they are redirected to /login
 * If they have (or don't need to) they're allowed to go about their way.
 */
const routes = [
  {
    path: '/home',
    name: 'home',
    component: HomeView,
    meta: {
      requiresAuth: false
    }
  },
  {
    path: "/login",
    name: "login",
    component: LoginView,
    meta: {
      requiresAuth: false
    }
  },
  {
    path: "/logout",
    name: "logout",
    component: LogoutView,
    meta: {
      requiresAuth: false
    }
  },
  {
    path: "/register",
    name: "register",
    component: RegisterView,
    meta: {
      requiresAuth: false
    }
  },
  {
    path: "/search",
    name: "search",
    component: SearchBox,
    meta: {
      requiresAuth: false
    }
  },
  {
    path:"/Library",
    name: "Library",
    component: LibraryView,
    meta: {
      requiresAuth: false
    }
  },
  {
    path:"/collections",
    name: "Collections",
    component: CollectionsView,
    meta: {
      requiresAuth: false,
    }
  },
  {
    path: "/data",
    name: "data",
    component: DataView,
    meta: {
      requiresAuth: false,
    }
  },
  {
    path: "/SearchResult",
    name: "SearchResult",
    component: SearchResultView,
    meta: {
      requiresAuth: false,
    } 
    
  },
  {
    path: "/Profile",
    name: "Profile",
    component: ProfilePageView,
    meta: {
      requiresAuth: false,
    } 
  },
  
  {
    path: '/homeA',
    name: 'homeA',
    component: HomeA,
    meta: {
      requiresAuth: false
    }
  },
  {
    path: "/popup",
    name: "popup",
    component: popupViewTest,
    meta: {
      requiresAuth: false,
    } 
  },
  {
    path: "/JakeView",
    name: "JakeView",
    component: JakeView,
    meta: {
      requiresAuth: false,
    }
  }
];

// Create the router
const router = createRouter({
  history: createWebHistory(),
  routes: routes
});

router.beforeEach((to) => {

  // Get the Vuex store
  const store = useStore();

  // Determine if the route requires Authentication
  const requiresAuth = to.matched.some(x => x.meta.requiresAuth);

  // If it does and they are not logged in, send the user to "/login"
  if (requiresAuth && store.state.token === '') {
    return {name: "login"};
  }
  // Otherwise, do nothing and they'll go to their next destination
});

export default router;
