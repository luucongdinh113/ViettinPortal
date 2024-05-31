import React, { Component } from 'react';

import { handleResponse } from '../helpers/handle-response.js';
import { authenticationService } from '../services/authentication.service';
import { genkeyservice } from '../services/genkey.service';

export class GenKey extends Component {
    static displayName = GenKey.name;

    constructor(props) {
        super(props);

        this.state = {

            errorMessage: null,
            token: authenticationService.currentUserValue.token,
            lockSubmit: true,

            applicationList: [], 
            applicationnFilter: '',
            generateKeyId: '',
            serial: '',

            applicationClientList: [], 
            applicationnClientFilter: '',
            generateClientKeyId: '',
            serialClient: '',

            appInstrumentList: [],
            appInstrumentFilter: '',
            genInstrumentKeyId: '',
            instrumentId: '',
            instrumentExpire: '',
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleBlur = this.handleBlur.bind(this);

        this.handleKeyDown = this.handleKeyDown.bind(this);
        this.handleKeyDownClient = this.handleKeyDownClient.bind(this);
        this.handleKeyDownInstrument = this.handleKeyDownInstrument.bind(this);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleSubmitClient = this.handleSubmitClient.bind(this);
        this.handleSubmitInstrument = this.handleSubmitInstrument.bind(this);

        this.createOptions = this.createOptions.bind(this);
        this.createOptionsClient = this.createOptionsClient.bind(this);
        this.createOptionsInstrument = this.createOptionsInstrument.bind(this);

        this.filterApplication = this.filterApplication.bind(this);
        this.filterApplicationClient = this.filterApplicationClient.bind(this);
        this.filterAppInstrument = this.filterAppInstrument.bind(this);
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleBlur(event) {
        this.setState({ [event.target.name + "_touch"]: true });
    }

    componentDidMount() {
        this.filterApplication();
        this.filterApplicationClient();
        this.filterAppInstrument();
    }

    handleSubmit(event) {

        this.setState({ errorMessage: "" });
        if (this.state.serial === "") {

        }
        else {

            this.setState({ lockSubmit: true });

            var applicationSubmit = this.state.generateKeyId;
            if (applicationSubmit == null || applicationSubmit == "") {
                if (this.state.applicationList != null && this.state.applicationList.length > 0) {
                    applicationSubmit = this.state.applicationList[0].generateKeyId;
                }
            }
            this.setState({ errorMessage: "Processing !!!" });

            genkeyservice.generateKey(applicationSubmit, this.state.serial, this.state.token)
                .then(
                    (result) => {

                        /*var objResult = {
                            result: result,
                            error: null,
                        };
                        */
                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: "Mã đã được gửi vào email" });

                        //console.log("api/GenKey/Generate success: ");
                        //console.log(objResult);
                        //return objResult;
                    },
                    (error) => {

                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: error.message });
                        //console.log("Error processing !");
                        //console.log(objResult);
                        //return objResult;
                    });
        }
        
        event.preventDefault();
    }

    handleSubmitClient(event) {

        this.setState({ errorMessage: "" });
        if (this.state.serialClient === "") {

        }
        else {

            this.setState({ lockSubmit: true });

            var applicationClientSubmit = this.state.generateClientKeyId;
            if (applicationClientSubmit == null || applicationClientSubmit === "") {
                if (this.state.applicationClientList != null && this.state.applicationClientList.length > 0) {
                    applicationClientSubmit = this.state.applicationClientList[0].generateKeyId;
                }
            }

            this.setState({ errorMessage: "Processing !!!" });

            genkeyservice.generateClientKey(applicationClientSubmit, this.state.serialClient, this.state.token)
                .then(
                    (result) => {

                        //var objResult = {
                        //    result: result,
                        //    error: null,
                        //};

                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: "Mã đã được gửi vào email" });

                        //console.log("api/GenKey/Generate success: ");
                        //console.log(objResult);
                        //return objResult;
                    },
                    (error) => {

                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: error.message });
                        //console.log("Error processing !");
                        //console.log(objResult);
                        //return objResult;
                    });
        }

        event.preventDefault();
    }

    handleSubmitInstrument(event) {

        this.setState({ errorMessage: "" });
        if (this.state.instrumentId === "" || this.state.instrumentExpire === "") {

        }
        else {
            this.setState({ lockSubmit: true });

            var instrumentIdSubmit = this.state.genInstrumentKeyId;
            if (instrumentIdSubmit == null || instrumentIdSubmit == "") {
                if (this.state.appInstrumentList != null && this.state.appInstrumentList.length > 0) {
                    instrumentIdSubmit = this.state.appInstrumentList[0].generateKeyId;
                }
            }
            this.setState({ errorMessage: "Processing !!!" });

            genkeyservice.genKeyInstrument(instrumentIdSubmit, this.state.instrumentId, this.state.instrumentExpire, this.state.token)
                .then(
                    (result) => {
                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: "Mã đã được gửi vào email" });
                    },
                    (error) => {

                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: error.message });
                    });

            this.setState({ lockSubmit: false });
        }

        event.preventDefault();
    }

    filterApplication() {

        this.setState({ lockSubmit: true });
        this.setState({ errorMessage: "Lấy danh sách ứng dụng !" });
        genkeyservice.getApplicationList(this.state.applicationnFilter, this.state.token)
            .then(
                (result) => {

                    this.setState({ lockSubmit: false });
                    if (!result.error) {

                        //console.log("Receive data !");
                        //console.log(result);
                        this.setState({ applicationList: result.result });
                    }
                    else {
                        //console.log("Receive data error processing !");
                        //console.log(result);

                        this.setState({ errorMessage: result.error });
                    }
                },
                (error) => {

                    //var objResult = {
                    //    result: null,
                    //    error: error,
                    //};

                    //console.log("Error processing !");
                    //console.log(error);

                    this.setState({ lockSubmit: false });
                    this.setState({ errorMessage: error.message });
                    //return objResult;
                }
        );
        this.setState({ errorMessage: "" });
    }

    filterApplicationClient() {

        this.setState({ lockSubmit: true });
        this.setState({ errorMessage: "Lấy danh sách ứng dụng client !" });
        genkeyservice.getApplicationClientList(this.state.applicationnClientFilter, this.state.token)
            .then(
                (result) => {

                    this.setState({ lockSubmit: false });
                    if (!result.error) {

                        //console.log("Receive data !");
                        //console.log(result);

                        this.setState({ applicationClientList: result.result });
                    }
                    else {
                        //console.log("Receive data error processing !");
                        //console.log(result);

                        this.setState({ errorMessage: result.error });
                    }
                },
                (error) => {

                    //var objResult = {
                    //    result: null,
                    //    error: error,
                    //};

                    console.log("Error processing !");
                    console.log(error);

                    this.setState({ lockSubmit: false });
                    this.setState({ errorMessage: error.message });
                    //return objResult;
                }
            );
        this.setState({ errorMessage: "" });
    }

    filterAppInstrument() {

        this.setState({ lockSubmit: true });
        this.setState({ errorMessage: "Lấy danh sách ứng dụng !" });

        genkeyservice.getApplicationList(this.state.appInstrumentFilter, this.state.token)
            .then(
                (result) => {

                    this.setState({ lockSubmit: false });
                    if (!result.error) {

                        this.setState({ appInstrumentList: result.result });
                    }
                    else {
                        this.setState({ errorMessage: result.error });
                    }
                },
                (error) => {

                    this.setState({ lockSubmit: false });
                    this.setState({ errorMessage: error.message });
                }
            );
        this.setState({ errorMessage: "" });
    }

    handleKeyDown(e) {
        if (e.key === 'Enter') {
            this.filterApplication();
        }
    }

    handleKeyDownClient(e) {
        if (e.key === 'Enter') {
            this.filterApplicationClient();
        }
    }

    handleKeyDownInstrument(e) {
        if (e.key === 'Enter') {
            this.filterAppInstrument();
        }
    }

    createOptions() {
        let options = []

        //console.log("createOptions:");
        //console.log(this.state.applicationList);

        if (this.state.applicationList != null && this.state.applicationList.length > 0) {
            // Outer loop to create parent
            for (let i = 0; i < this.state.applicationList.length; i++) {

                //Create the parent and add the children
                options.push(<option value={this.state.applicationList[i].generateKeyId}>{this.state.applicationList[i].displayText}</option>)
            }
        }
        return options
    }

    createOptionsClient() {
        let clientOptions = []

        //console.log("createOptions:");
        //console.log(this.state.applicationList);

        if (this.state.applicationClientList != null && this.state.applicationClientList.length > 0) {
            // Outer loop to create parent
            for (let i = 0; i < this.state.applicationClientList.length; i++) {

                //Create the parent and add the children
                clientOptions.push(<option value={this.state.applicationClientList[i].generateKeyId}>{this.state.applicationClientList[i].displayText}</option>)
            }
        }
        return clientOptions
    }

    createOptionsInstrument() {
        let options = []

        if (this.state.appInstrumentList != null && this.state.appInstrumentList.length > 0) {
            // Outer loop to create parent
            for (let i = 0; i < this.state.appInstrumentList.length; i++) {

                //Create the parent and add the children
                options.push(<option value={this.state.appInstrumentList[i].generateKeyId}>{this.state.appInstrumentList[i].displayText}</option>)
            }
        }
        return options
    }

    render() {
        
        return (
          <div class="row">
              <div class="col-md-12">
                  <section>
                      <h4>Cấp mã các module hệ thống - Mã sẽ được gửi vào email</h4>

                        <hr />

                        <div class="accordion" id="accordGenGroup">
                            <div class="card">
                                <div class="card-header" id="headingOne">
                                    <h2 class="mb-0">
                                        <button class="btn btn-primary" type="button" data-toggle="collapse" data-target="#collapseGenkeyPlus" aria-expanded="false" aria-controls="collapseGenkeyPlus">
                                            Cấp mã plus
                                        </button>
                                    </h2>
                                </div>

                                <div id="collapseGenkeyPlus" class="collapse show" aria-labelledby="headingOne" data-parent="#accordGenGroup">
                                    <div class="card-body">
                                        <div asp-validation-summary="All" class="text-danger"></div>
                                        <div class="form-group">
                                            <label>Lọc ứng dụng</label>
                                            <input type="text" class="form-control" value={this.state.applicationnFilter} name="applicationnFilter" onChange={this.handleChange} onKeyDown={this.handleKeyDown} />
                                        </div>

                                        <hr />
                                        <form method="post" onSubmit={this.handleSubmit}>
                                            <div asp-validation-summary="All" class="text-danger"></div>
                                            <div class="form-group">
                                                <label>Ứng dụng</label>
                                                <select class="form-control" id="Genkey_cmbApplication" name="generateKeyId" value={this.state.generateKeyId} onChange={this.handleChange}>
                                                    {this.createOptions()}
                                                </select>
                                            </div>
                                            <div class="form-group">
                                                <label>Mã Serial</label>
                                                <input type="text" class="form-control" value={this.state.serial} name="serial" onChange={this.handleChange} onBlur={this.handleBlur} />
                                                {this.state.serial_touch && this.state.serial === "" &&
                                                    <span id="serial-validation" class="text-danger">Serial is required</span>
                                                }
                                            </div>
                                            <div class="form-group">
                                                <span id="email-validation" asp-validation-for="Email" class="text-danger">{this.state.errorMessage}</span>
                                            </div>
                                            <div class="form-group">
                                                <button type="submit" class="btn btn-default btn-primary" disabled={this.state.lockSubmit}>Cấp mã</button>
                                                <div class="spinner-border text-primary" role="status">
                                                    <span class="sr-only">Loading...</span>
                                                </div>

                                                {this.state.lockSubmit &&
                                                    <img src="../index.ajax-spinner-gif.gif" />  
                                                }
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>

                            <div class="card">
                                <div class="card-header" id="headingTwo">
                                    <h2 class="mb-0">
                                        <button class="btn btn-success" type="button" data-toggle="collapse" data-target="#collapseGenkeyClient" aria-expanded="false" aria-controls="collapseGenkeyClient" id="btncollapseGenkeyClient">
                                            Cấp mã client
                                        </button>
                                    </h2>
                                </div>
                                <div id="collapseGenkeyClient" class="collapse" aria-labelledby="headingTwo" data-parent="#accordGenGroup">
                                    <div class="card-body">
                                        <div asp-validation-summary="All" class="text-danger"></div>
                                        <div class="form-group">
                                            <label>Lọc ứng dụng</label>
                                            <input type="text" class="form-control" value={this.state.applicationnClientFilter} name="applicationnClientFilter" onChange={this.handleChange} onKeyDown={this.handleKeyDownClient} />
                                        </div>

                                        <hr />

                                        <form method="post" onSubmit={this.handleSubmitClient}>
                                            <div asp-validation-summary="All" class="text-danger"></div>
                                            <div class="form-group">
                                                <label>Ứng dụng</label>
                                                <select class="form-control" id="Genkey_cmbApplicationClient" name="generateClientKeyId" value={this.state.generateClientKeyId} onChange={this.handleChange}>
                                                    {this.createOptionsClient()}
                                                </select>
                                            </div>
                                            <div class="form-group">
                                                <label>Mã Serial</label>
                                                <input type="text" class="form-control" value={this.state.serialClient} name="serialClient" onChange={this.handleChange} onBlur={this.handleBlur} />
                                                {this.state.serialClient_touch && this.state.serialClient === "" &&
                                                    <span id="serial-validation" class="text-danger">Serial is required</span>
                                                }
                                            </div>
                                            <div class="form-group">
                                                <span id="email-validation" asp-validation-for="Email" class="text-danger">{this.state.errorMessage}</span>
                                            </div>
                                            <div class="form-group">
                                                <button type="submit" class="btn btn-default btn-success" disabled={this.state.lockSubmit}>Cấp mã client</button>

                                                {this.state.lockSubmit &&
                                                    <img src="../index.ajax-spinner-gif.gif" />  
                                                }
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>

                            {/* Cấp mã id máy */}
                            <div class="card">
                                <div class="card-header" id="headingThree">
                                    <h2 class="mb-0">
                                        <button class="btn btn-warning" type="button" data-toggle="collapse" data-target="#collapseGenkeyInstrument" aria-expanded="false" aria-controls="collapseGenkeyInstrument">
                                            Cấp mã id máy
                                        </button>
                                    </h2>
                                </div>

                                <div id="collapseGenkeyInstrument" class="collapse" aria-labelledby="headingThree" data-parent="#accordGenGroup">
                                    <div class="card-body">
                                        <div asp-validation-summary="All" class="text-danger"></div>
                                        <div class="form-group">
                                            <label>Lọc ứng dụng</label>
                                            <input type="text" class="form-control" value={this.state.appInstrumentFilter} name="appInstrumentFilter" onChange={this.handleChange} onKeyDown={this.handleKeyDownInstrument} />
                                        </div>

                                        <hr />
                                        <form method="post" onSubmit={this.handleSubmitInstrument}>
                                            <div asp-validation-summary="All" class="text-danger"></div>
                                            <div class="form-group">
                                                <label>Ứng dụng</label>
                                                <select class="form-control" id="Genkey_cmbApplication" name="genInstrumentKeyId" value={this.state.genInstrumentKeyId} onChange={this.handleChange}>
                                                    {this.createOptionsInstrument()}
                                                </select>
                                            </div>
                                            <div class="form-group row">
                                                <div class="col col-md-6">
                                                    <label>Id máy</label>
                                                    <input type="text" class="form-control" value={this.state.instrumentId} name="instrumentId" onChange={this.handleChange} onBlur={this.handleBlur} />
                                                    {this.state.instrumentId_touch && this.state.instrumentId === "" &&
                                                        <span id="instrumentId-validation" class="text-danger">[Id máy] được yêu cầu</span>
                                                    }
                                                </div>
                                                <div class="col col-md-6">
                                                    <label>Thời hạn sử dụng</label>
                                                    <input type="date" class="form-control" value={this.state.instrumentExpire} name="instrumentExpire" onChange={this.handleChange} onBlur={this.handleBlur} />
                                                    {this.state.instrumentExpire_touch && this.state.instrumentExpire === "" &&
                                                        <span id="instrumentExpire-validation" class="text-danger">[Thời hạn sử dụng] được yêu cầu</span>
                                                    }
                                                </div>

                                            </div>
                                            <div class="form-group">
                                                <span id="genkeyInstrument-validation" class="text-danger">{this.state.errorMessage}</span>
                                            </div>
                                            <div class="form-group">
                                                <button type="submit" class="btn btn-default btn-warning" disabled={this.state.lockSubmit}>Cấp mã id máy</button>
                                                <div class="spinner-border text-primary" role="status">
                                                    <span class="sr-only">Loading...</span>
                                                </div>

                                                {this.state.lockSubmit &&
                                                    <img src="../index.ajax-spinner-gif.gif" />
                                                }
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>


                        </div>
                  </section >
              </div >
          </div>
    );
  }
}
