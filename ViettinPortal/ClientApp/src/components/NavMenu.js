import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

import { history } from '../helpers/history';
import { handleResponse } from '../helpers/handle-response.js';
import { authenticationService } from '../services/authentication.service';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
      super(props);

      this.state = {
          currentUser: props.currentUser,
          collapsed: true
      };

      this.toggleNavbar = this.toggleNavbar.bind(this);
      this.logout = this.logout.bind(this);
      this.handleTestAuthorize = this.handleTestAuthorize.bind(this);
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
    }

    componentDidMount() {
        authenticationService.currentUser.subscribe(x => {
            this.setState({
                currentUser: x,
            });

            console.log(x);
        });
    }

    handleTestAuthorize(event) {

        const requestOptions = {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + this.state.currentUser.token,
            },
            body: JSON.stringify({
                Email: 'test',
                Password: 'test',
            })
        };

        fetch(`api/TestAuthorize/Test`, requestOptions)
            .then(handleResponse)
            .then(
                (result) => {

                    var objResult = {
                        result: result,
                        error: null,
                    };

                    console.log("api/TestAuthorize/Test success: ");
                    console.log(objResult);
                    return objResult;
                },
                (error) => {
                    var objResult = {
                        result: null,
                        error: error,
                    };

                    console.log("Error processing !");
                    console.log(objResult);
                    return objResult;
                });
    }

    logout() {
        authenticationService.logout();
        history.push('/login');
    }

    render() {

    return (

        <header>
            {this.state.currentUser &&
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/">Viettin Portal</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>


                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/genkey">Cấp mã</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/encrypt">Mã hóa</NavLink>
                                </NavItem>

                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/customer">Khách hàng</NavLink>
                            </NavItem>

                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/Account">{this.state.currentUser.result.fullName}</NavLink>
                                </NavItem>

                                <NavItem>
                                    <button className="btn btn-primary" onClick={this.logout}>Logout</button>
                            </NavItem>
                            </ul>

                        </Collapse>
                    </Container>
                </Navbar>
            }
      </header>
    );
  }
}
