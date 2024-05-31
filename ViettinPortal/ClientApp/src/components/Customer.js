import React, { Component } from 'react';

import { handleResponse } from '../helpers/handle-response.js';
import { authenticationService } from '../services/authentication.service';
import { customerService } from '../services/customer.service';

export class Customer extends Component {
    static displayName = Customer.name;

    constructor(props) {
        super(props);

        this.state = {

            errorMessage: 'test',
            token: authenticationService.currentUserValue.token,
            lockSubmit: true,

            customerId: '',
            customerName: '',
            description: '',

            customerIdFilter: '',
            customerNameFilter: '',

            customerList: [],
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleBlur = this.handleBlur.bind(this);

        this.handleKeyDown = this.handleKeyDown.bind(this);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleSubmitFilter = this.handleSubmitFilter.bind(this);

        this.filterCustomer = this.filterCustomer.bind(this);
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleBlur(event) {
        this.setState({ [event.target.name + "_touch"]: true });
    }

    componentDidMount() {
        this.filterCustomer();
    }

    handleSubmit(event) {

        this.setState({ errorMessage: "" });
        if (this.state.customerId === "" || this.state.customerName === "" || this.state.description === "") {

        }
        else {

            this.setState({ lockSubmit: true });

            var request = {
                CustomerId: this.state.customerId,
                CustomerName: this.state.customerName, 
                Description: this.state.description
            };

            customerService.addCustomer(request, this.state.token)
                .then(
                    (result) => {

                        this.setState({ lockSubmit: false });
                        if (!result.error) {

                            this.setState({ customerId: "" });
                            this.setState({ customerName: "" });
                            this.setState({ description: "" });
                            this.filterCustomer();
                        }
                        else {
                            this.setState({ errorMessage: result.error });
                        }
                    },
                    (error) => {

                        this.setState({ lockSubmit: false });
                        this.setState({ errorMessage: error.message });
            });
        
        }
        
        event.preventDefault();
    }

    handleSubmitFilter(event) {

        this.filterCustomer();
        event.preventDefault();
    }

    filterCustomer() {

        this.setState({ lockSubmit: true });

        var filter = {
            CustomerIdFilter: this.state.customerIdFilter,
            CustomerNameFilter: this.state.customerNameFilter
        };

        customerService.getCustomerList(filter, this.state.token)
            .then(
                (result) => {

                    this.setState({ lockSubmit: false });
                    if (!result.error) {

                        this.setState({ customerList: result.result });
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
        /**/
        this.setState({ errorMessage: "" });
    }

    handleKeyDown(e) {
        if (e.key === 'Enter') {
            this.filterCustomer();
        }
    }

    handleDeleteCustomer(customer, e) {

        this.setState({ lockSubmit: true });

        var request = {
            SysId: customer.sysId
        };

        customerService.deleteCustomer(request, this.state.token)
            .then(
                (result) => {

                    this.setState({ lockSubmit: false });
                    if (!result.error) {

                        this.filterCustomer();
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
        /**/
        this.setState({ errorMessage: "" });
    }

    render() {
        
        return (
            <div className="col-md-12">
                <section>
                    <h4>DANH MỤC KHÁCH HÀNG</h4>

                    <hr />

                    <div className="card">
                        <div className="card-header" id="headingOne">
                            <h2 className="mb-0">Bổ sung khách hàng</h2>
                        </div>

                        <div id="collapseGenkeyPlus" className="collapse show" aria-labelledby="headingOne" data-parent="#accordGenGroup">
                            <div className="card-body">
                                <div asp-validation-summary="All" className="text-danger">
                                    <ul>
                                        {this.state.errorMessage}
                                        {this.state.errorMessage !== "" &&
                                            <li><span id="errorMessage-validation" className="text-danger">{this.state.errorMessage}</span></li>
                                        }
                                        {this.state.customerId_touch && this.state.customerId === "" &&
                                            <li><span id="customerId-validation" className="text-danger">[ID khách hàng] yêu cầu bắt buộc</span></li>
                                        }
                                        {this.state.customerName_touch && this.state.customerName === "" &&
                                            <li><span id="customerName-validation" className="text-danger">[Tên khách hàng] yêu cầu bắt buộc</span></li>
                                        }
                                    </ul>
                                </div>
                                <hr />
                                <form method="post" onSubmit={this.handleSubmit}>
                                    <div asp-validation-summary="All" className="text-danger"></div>

                                    <div className="form-group row">
                                        <div className="col col-md-6">
                                            <label>Mã khách hàng</label>
                                            <input type="text" className="form-control" value={this.state.customerId} name="customerId" onChange={this.handleChange} onBlur={this.handleBlur} />
                                        
                                        </div>
                                        <div className="col col-md-6">
                                            <label>Tên khách hàng</label>
                                            <input type="text" className="form-control" value={this.state.customerName} name="customerName" onChange={this.handleChange} onBlur={this.handleBlur} />
                                        </div>
                                    </div>

                                    <div className="form-group row">
                                        <div className="col col-md-12">
                                            <label>Ghi chú</label>
                                            <input type="text" className="form-control" value={this.state.description} name="description" onChange={this.handleChange} onBlur={this.handleBlur} />
                                        </div>
                                    </div>

                                    <div className="form-group">
                                        <button type="submit" className="btn btn-default btn-primary" disabled={this.state.lockSubmit}>Tạo khách hàng</button>
                                        <div className="spinner-border text-primary" role="status">
                                            <span className="sr-only">Loading...</span>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </section >

                <hr />

                <section>
                    <div className="card">
                        <div className="card-header" id="headingOne">
                            <h2 className="mb-0">Lọc danh sách khách hàng</h2>
                        </div>

                        <div id="collapseGenkeyPlus" className="collapse show" aria-labelledby="headingOne" data-parent="#accordGenGroup">
                            <div className="card-body">

                                <form method="post" onSubmit={this.handleSubmitFilter}>
                                    <div className="row form-group">
                                        <div className="col col-md-6">
                                            <label>Mã khách hàng</label>
                                            <input type="text" className="form-control" value={this.state.customerIdFilter} name="customerIdFilter" onChange={this.handleChange} onBlur={this.handleBlur} />
                                        </div>
                                        <div className="col col-md-6">
                                            <label>Tên khách hàng</label>
                                            <input type="text" className="form-control" value={this.state.customerNameFilter} name="customerNameFilter" onChange={this.handleChange} onBlur={this.handleBlur} />
                                        </div>
                                    </div>

                                    <div className="row form-group">
                                        <div className="col col-md-12">
                                            <button type="submit" className="btn btn-default btn-primary" disabled={this.state.lockSubmit}>Lọc khách hàng</button>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </section >

                <hr />

                <section>
                    <div className="card">
                        <div className="card-header" id="headingOne">
                            <h2 className="mb-0">Danh sách khách hàng</h2>
                        </div>

                        <div id="collapseGenkeyPlus" className="collapse show" aria-labelledby="headingOne" data-parent="#accordGenGroup">
                            <div className="card-body">

                                <table className='table table-bordered table-striped'>
                                    <thead>
                                        <tr className="">
                                            <th className="">STT</th>
                                            <th className="">Mã khách hàng</th>
                                            <th className="">Tên khách hàng</th>
                                            <th className="">Hành động</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {this.state.customerList.map((customerInfo, index) =>
                                                <tr key={customerInfo.sysId} className='table table-bordered table-striped'>
                                                    <td>{index+1}</td>
                                                    <td>{customerInfo.customerId}</td>
                                                    <td>{customerInfo.customerName}</td>
                                                <td className="text-center">
                                                    <button className="btn btn-default btn-danger" onClick={(e) => this.handleDeleteCustomer(customerInfo, e)}>Delete</button>
                                                    </td>
                                                </tr>
                                            )
                                        }
                                        {this.state.customerList.length === 0 &&
                                            <tr className=""><td colSpan="3" className="text-center">Không có dữ liệu</td></tr>
                                        }
                                    </tbody>
                                </table>
                                
                            </div>
                        </div>
                    </div>
                </section >
            </div >
    );
  }
}
