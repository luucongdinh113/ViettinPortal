import React, { Component } from 'react';

import { handleResponse } from '../helpers/handle-response.js';
import { authenticationService } from '../services/authentication.service';

export class Account extends Component {
    static displayName = Account.name;

    constructor(props) {
        super(props);

        this.state = {
            oldPassword: '',
            password: '',
            confirmPassword: '',
            errorMessage: null,
            token: authenticationService.currentUserValue.token,
            lockSubmit: false,
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleBlur(event) {
        this.setState({ [event.target.name + "_touch"]: true });
    }

    handleSubmit(event) {

        this.setState({ errorMessage: "" });
        if (this.state.oldPassword === "") {

        }
        else if (this.state.password === "") {

        }
        else if (this.state.confirmPassword === "") {

        }
        else if (this.state.password !== this.state.confirmPassword) {
            this.setState({ errorMessage: "Mật khẩu và mật khẩu confirm khác nhau" });
        }
        else {

            this.setState({ errorMessage: "" });
            this.setState({ lockSubmit: true });

            const requestOptions = {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + this.state.token,
                },
                body: JSON.stringify({
                    NewPassword: this.state.password,
                    OldPassword: this.state.oldPassword,
                })
            };

            this.setState({ errorMessage: "Processing !!!" });

            fetch(`api/Account/ChangePass`, requestOptions)
                .then(handleResponse)
                .then(
                    (result) => {

                        var objResult = {
                            result: result,
                            error: null,
                        };

                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: "Đổi mật khẩu thành công" });

                        console.log("api/Account/ChangePass success: ");
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

                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: objResult.error });
                        return objResult;
                    });
        }

        event.preventDefault();
    }

  render () {
    return (
        <div class="row">
            <div class="col-md-12">
                <section>
                    <h4>Đổi password</h4>

                    <hr />
                    <form method="post" onSubmit={this.handleSubmit} action="Genkey/Generate">
                        <div asp-validation-summary="All" class="text-danger"></div>
                        <div class="form-group">
                            <label>Password cũ</label>
                            <input type="password" class="form-control" value={this.state.oldPassword} name="oldPassword" onChange={this.handleChange} onBlur={this.handleBlur} />
                            {this.state.oldPassword_touch && this.state.oldPassword === "" &&
                                <span id="oldPassword-validation" class="text-danger">Password cũ is required</span>
                            }
                        </div>
                        <div class="form-group">
                            <label>Password mới</label>
                            <input type="password" class="form-control" value={this.state.password} name="password" onChange={this.handleChange} onBlur={this.handleBlur} />
                            {this.state.password_touch && this.state.password === "" &&
                                <span id="password-validation" class="text-danger">Password mới is required</span>
                            }
                        </div>
                        <div class="form-group">
                            <label>Confirm Password</label>
                            <input type="password" class="form-control" value={this.state.confirmPassword} name="confirmPassword" onChange={this.handleChange} onBlur={this.handleBlur} />
                            {this.state.confirmPassword_touch && this.state.confirmPassword === "" &&
                                <span id="confirmPassword-validation" class="text-danger">Confirm pass is required</span>
                            }
                        </div>
                        <div class="form-group">
                            <span id="validation" class="text-danger">{this.state.errorMessage}</span>
                        </div>
                        <div class="form-group">
                            <button type="submit" class="btn btn-default btn-primary" disabled={this.state.lockSubmit}>Đổi pass</button>
                            {this.state.lockSubmit &&
                                <img src="../index.ajax-spinner-gif.gif" />
                            }
                        </div>
                    </form>
                </section >
            </div >
        </div>
    );
  }
}
