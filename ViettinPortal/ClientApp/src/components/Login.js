import React, { Component } from 'react';

import { handleResponse } from '../helpers/handle-response.js';
import { authenticationService } from '../services/authentication.service';

export class Login extends Component {
    static displayName = Login.name;

    constructor(props) {
        super(props);
        this.state = { email: '', password: '', redirectToReferrer: false, errorMessage: null };

        this.handleChange = this.handleChange.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleTestAuthorize = this.handleTestAuthorize.bind(this);

        // redirect to home if already logged in
        if (authenticationService.currentUserValue) {
            this.props.history.push('/');
        }

        if (props != null && props.location != null &&
            props.location.state != null && props.location.state.from != null) {

            this.setState({ from: props.location.state.from });

            //console.log('from: ' + props.location.state.from)
        }
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleBlur(event) {
        this.setState({ [event.target.name + "_touch"]: true });
    }

    handleTestAuthorize(event) {

        const requestOptions = {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
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

    handleSubmit(event) {

        if (this.state.email == "") {

        }
        else if (this.state.password == "") {

        }
        else {
            authenticationService.login(this.state.email, this.state.password)
                .then(
                    (result) => {

                        if (!result.error) {
                            const { from } = this.props.location.state || { from: { pathname: "/" } };
                            this.props.history.push(from);
                        }
                        else {
                            console.log("Receive data error processing !");
                            console.log(result);

                            this.setState({ errorMessage: result.error });
                        }
                    },
                    (error) => {
                        //setSubmitting(false);
                        //setStatus(error);

                        var objResult = {
                            result: null,
                            error: error,
                        };

                        console.log("Error processing !");
                        console.log(objResult);
                        return objResult;
                    }
                );
            //if (loginUser != null && loginUser.email != null) {

            //}
        }
        event.preventDefault();
    }

  render () {
    return (

          <div class="col-md-4">
              <section>
                <form method="post" onSubmit={this.handleSubmit} action="Account/Login">
                <h4>Sử dụng email để login.</h4>
                  <hr />
                  <div asp-validation-summary="All" class="text-danger"></div>
                  <div class="form-group">
                      <label>Email</label>
                        <input type="text" class="form-control" value={this.state.email} name="email" onChange={this.handleChange} onBlur={this.handleBlur} />
                        {this.state.email_touch && this.state.email == "" &&
                            <span id="email-validation" asp-validation-for="Email" class="text-danger">Username is required</span>
                        }
                  </div>
                  <div class="form-group">
                      <label>Password</label>
                        <input type="password" class="form-control" value={this.state.password} name="password" onChange={this.handleChange} onBlur={this.handleBlur} />
                        {this.state.password_touch && this.state.password == "" &&
                            <span id="password-validation" class="text-danger">Password is required</span>
                        }
                    </div>
                    <div class="form-group">
                        <span id="email-validation" asp-validation-for="Email" class="text-danger">{this.state.errorMessage}</span>
                    </div>
                  <div class="form-group">
                        <button type="submit" class="btn btn-default btn-primary">Đăng nhập</button>
                  </div>
            </form>
        </section >
    </div >
    );
  }
}
