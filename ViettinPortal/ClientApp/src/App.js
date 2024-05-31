import React, { Component } from 'react';
import { Route, Redirect } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Login } from './components/Login';
import { Account } from './components/Account';
import { GenKey } from './components/GenKey';
import { Customer } from './components/Customer';

import { PrivateRoute } from './components/PrivateRoute';
import { history } from './helpers/history';
import { Role } from './helpers/role';
import { authenticationService } from './services/authentication.service';

//const fakeAuth = {
//    isAuthenticated: false,
//    authenticate(cb) {
//        this.isAuthenticated = true;
//        setTimeout(cb, 100); // fake async
//    },
//    signout(cb) {
//        this.isAuthenticated = false;
//        setTimeout(cb, 100);
//    }
//};

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        authenticationService.currentUser.subscribe(x => this.setState({
            currentUser: x,
            isAdmin: x && x.role === Role.Admin
        }));
    }

    logout() {
        authenticationService.logout();
        history.push('/login');
    }


  render () {
    return (
        <Layout>
            <PrivateRoute exact path='/' component={Home} />
            <PrivateRoute path='/account' component={Account}/>
            <Route path='/login' component={Login}/>
            <PrivateRoute path='/genkey' component={GenKey} />
            <PrivateRoute path='/customer' component={Customer} />
        </Layout>
    );
  }
}
