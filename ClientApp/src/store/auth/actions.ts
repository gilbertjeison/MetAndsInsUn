import { ActionTree } from 'vuex';
import { AuthState } from './types';
import { RootState } from '../types';
import mgr from '@/security/security';

export const actions: ActionTree<AuthState,RootState> = {

    async authenticate({commit,dispatch}, returnPath):Promise<any>{

        try 
        {
            const user = await dispatch('getUser');
            if (!!user) {
                commit('setAuthenticated',true);
                commit('setUser',user);
                console.log('user :', user);
            }
            else{
                await dispatch('signIn',returnPath);
            }
            
        } catch (error) 
        {
            console.log('Error action Auth:', error);
        }       
    },

    async getUser():Promise<any>{
        try 
        {
            let user = await mgr.getUser();            
            return user;
        } catch (error) 
        {
            console.log('Error action Auth:', error);
        }       
    },

    signIn(returnPath):any {
        returnPath ? mgr.signinRedirect({ state: returnPath })
        : mgr.signinRedirect();
    },

    signOut():any {
        mgr.signoutRedirect();
    }
    
}